using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DomainLayer.Dto;
using DomainLayer.Helpers;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServiceLayer.WinchDriverService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace ServiceLayer.WinchDriverServise
{
    public class WinchDriverServise : IWinchDriverService
    {


        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public WinchDriverServise(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IOptions<CloudinarySettings> config)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }



        private async Task<string> UploadPhotoAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException("file", "No file uploaded");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = folder
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.Uri.ToString();
            }
        }


        public async Task<Response> CompleteWinchData(string userEmail, WinchModel winchdata, IFormFileCollection WinchlicencePhoto, IFormFileCollection winchPhoto)
        {
            var driver = await _userManager.FindByEmailAsync(userEmail);
            if (driver == null)
            {
                return new Response { IsDone = false, Message = "User not found.", StatusCode = 404 };
            }

            if (WinchlicencePhoto.Count != 2)
            {
                return new Response { IsDone = false, Message = "Please add exactly 2 pictures: front and back of the winch license.", StatusCode = 400 };
            }

            driver.Spezilization = winchdata.Spezilization;

            var winch = await _context.Winchs.FirstOrDefaultAsync(x => x.DriverId == driver.Id);
            if (winch == null)
            {
                winch = new Winch { DriverId = driver.Id };
                await _context.Winchs.AddAsync(winch);
            }

            var winchPhotoUrls = new List<string>();
            foreach (var photo in winchPhoto)
            {
                var photoPath = await UploadPhotoAsync(photo, "WinchPhotos");
                winchPhotoUrls.Add(photoPath);
            }

            var licenceUrls = new List<string>();
            foreach (var photo in WinchlicencePhoto)
            {
                var photoPath = await UploadPhotoAsync(photo, "WinchLicencePhotos");
                licenceUrls.Add(photoPath);
            }

            winch.Model = winchdata.Model;
            winch.Availabile = true;
            winch.Licence = licenceUrls;
            winch.photo = winchPhotoUrls;

            await _context.SaveChangesAsync();

            var result = _mapper.Map<WinchOutputDTO>(winch);
            result.Spezilization = driver.Spezilization;

            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }




        private async Task<string> WinchDriverIDPhotos(IFormFile file)
        {

            if (file == null || file.Length == 0)
                throw new ArgumentNullException("file", "No file uploaded");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "WinchDriverIDPhotos"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return uploadResult.Uri.ToString();
            }
        }






        public async Task<Response> HandleOrderAction(string userEmail, string orderId, OrderAction action)
        {
            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                return new Response { Message = $"Invalid or non-existent user ID: {userEmail}", StatusCode = 404, IsDone = false };
            }

            // Find the order by DriverId and orderId
            var order = await _context.WinchOrders.FirstOrDefaultAsync(sp => sp.DriverId == user.Id && sp.Id == orderId);
            if (order == null)
            {
                return new Response { Message = "This user ID or order ID does not exist.", StatusCode = 404, IsDone = false };
            }


            // Handle the action
            switch (action)
            {
                case OrderAction.Accept:
                    order.Status = Status.inProgress;
                    user.available = false;
                    break;
                case OrderAction.Refuse:
                    order.Status = Status.PendingApproval;
                    user.available = true;
                    break;
                case OrderAction.Cancel:
                    order.Status = Status.Cancelled;
                    user.available = true;
                    break;
                case OrderAction.Completed:
                    order.Status = Status.Completed;
                    user.available = true;
                    break;
                default:
                    return new Response { Message = $"Invalid action: {action}", StatusCode = 400, IsDone = false };
            }

            // Create result DTO
            var result = new WinchOrderToAccept
            {
                OrderId = order.Id,
                ClientId = order.ClientId,
                Date = order.Date,
                ProblemDescription = order.ProblemDescription,
                DriverId = user.Id,
                cartype=order.cartype,
                location=order.location,
                Status = order.Status
            };


            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return response
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }




        public async Task<Response> GetMyOrderToAccept(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            var userid = user.Id;
            if (user == null)
            {
                return new Response { Message = $"Invalid or non-existent user ID: {user.Id}", StatusCode = 404, IsDone = false };
            }

            var myOrder = new List<WinchOrder>();
            var orders = await _context.WinchOrders.Where(x => x.DriverId == userid).ToListAsync();

            foreach (var order in orders)
            {
                if (order.Status == Status.PendingApproval)
                {
                    myOrder.Add(order);
                }
            }
            var result = myOrder;
            return new Response { Model = result, StatusCode = 200 };
        }


        public async Task<Response> GetOrdersHistory(string userEmail, Enums.OrderBy orderBy)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            var userid = user.Id;
            if (user == null)
            {
                return new Response { Message = $"Invalid or non-existent user ID: {userEmail}", StatusCode = 404, IsDone = false };
            }
            if (orderBy == Enums.OrderBy.status)
            {
                var myorder = _context.WinchOrders.Where(x => x.DriverId == userid).
                    OrderBy(x => x.Status == Status.Completed)
                                      .ThenBy(x => x.Status == Status.inProgress)
                                      .ThenBy(x => x.Status == Status.PendingApproval)
                                      .ThenBy(x => x.Status == Status.Cancelled).ToList();
                var result = _mapper.Map<List<RepareOrderToAccept>>(myorder);
                return new Response { StatusCode = 200, Model = result };
            }
            else
            {
                var myorder = _context.WinchOrders.OrderBy(x => x.Date);
                var result = _mapper.Map<List<RepareOrderToAccept>>(myorder);
                return new Response { Model = result, StatusCode = 200 };
            }

        }

        public async Task<Response> UpdatePersonalData(string userEmail, WinchDriverDto driverDto, IFormFile photo)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                return new Response { Message = "User not found for update.", StatusCode = 404, IsDone = false };
            }
            var winch = _context.Winchs.FirstOrDefault(x => x.DriverId == user.Id);

            user.Name = driverDto.Name;
            user.CarType = driverDto.winchModel;
            user.PhotoId = await UploadPhotoAsync(photo,"winchDriverPhotos");                      
            user.Spezilization = driverDto.Spezilization;
            winch.Model = driverDto.winchModel;
            await _context.SaveChangesAsync();
            var result = _mapper.Map<WinchDriverDto>(user);
            return new Response { Model = result, StatusCode = 200, Message = "your data is updated successfully " };
        }

        public Task<Response> CurrentOrder(string userEmail, Enums.OrderBy orderBy)
        {
            throw new NotImplementedException();
        }
    }
}