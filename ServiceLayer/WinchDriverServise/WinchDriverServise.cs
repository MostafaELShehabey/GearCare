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

        public WinchDriverServise(ApplicationDbContext context, IMapper mapper,UserManager<ApplicationUser> userManager , IOptions<CloudinarySettings> config)
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
        private async Task<string> WinchlicencePhotos(IFormFile file)
        {

            if (file == null || file.Length == 0)
                throw new ArgumentNullException("file", "No file uploaded");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "WinchlicencePhotos"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return uploadResult.Uri.ToString();
            }
        }

        private async Task<string> WinchPhotos(IFormFile file)
        {

            if (file == null || file.Length == 0)
                throw new ArgumentNullException("file", "No file uploaded");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "WinchPhotos"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                return uploadResult.Uri.ToString();
            }
        }

        private async Task<string> SavePersonalPhotoAsync(IFormFile file)
        {

            if (file == null || file.Length == 0)
                throw new ArgumentNullException("file", "No file uploaded");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "PersonalPhotos"
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
                return new Response { IsDone = false, Messege = "User not found." };
            }
            driver.Spezilization = winchdata.Spezilization;
            var winch =  await _context.Winchs.FirstOrDefaultAsync(x => x.WinchDriver.Id == driver.Id);

            var winchphotoURL = new List<string>();
            foreach(var photo in winchPhoto)
            {
                var photopath = await WinchPhotos(photo);
                winchphotoURL.Add(photopath);
            }

            if (WinchlicencePhoto.Count() > 1)
            {
                return   new Response { IsDone = false, Messege= "Add 2 picture , front and back of wich licence ",  StatusCode = 400 };
            }

            var licenceURL = new List<string>();    
            for (int i = 0; i <=1;i++)
            {
                var photopath = await WinchlicencePhotos(WinchlicencePhoto[i]);
                licenceURL.Add(photopath);
            }

            var data = new Winch
            {
                Model = winchdata.Model,
                DriverId = driver.Id,
                Availabile = true,
                Licence = licenceURL,
                photo = winchphotoURL

            };

            await _context.Winchs.AddAsync(data);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<WinchOutputDTO>(data);
            return new Response { IsDone = true,Model = result, StatusCode =200};
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



        public async Task<Response> AddIDphoto(IFormFileCollection photo, string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            var IDURL = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                var photopath = await WinchlicencePhotos(photo[i]);
                IDURL.Add(photopath);
            }

            await _context.SaveChangesAsync();

            var result = _mapper.Map<CompleteSelerData>(user);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }


       

      

        public async Task<Response> HandleOrderAction(string userEmail, string orderId, OrderAction action)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                throw new InvalidOperationException($"Invalid or non-existent user ID: {userEmail}");
            }

            var order = await _context.WinchOrders.FirstOrDefaultAsync(sp => sp.DriverId == user.Id && sp.Id == orderId);
            if (order == null)
            {
                throw new InvalidOperationException("This user ID or order ID does not exist.");
            }

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
                    user.available = true ;
                    break;
                case OrderAction.Completed:
                    order.Status = Status.Comlpeted;
                    user.available = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), $"Invalid : {action}");
            }
            await _context.SaveChangesAsync();
            var result = _mapper.Map<WinchDriverDto>(order);
            return new Response { Model = result, StatusCode = 200 };
        }




        public async Task<Response> GetMyOrderToAccept(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            var userid = user.Id;
            if (user == null)
            {
                throw new InvalidOperationException($"Invalid or non-existent user ID: {userEmail}");
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
                throw new InvalidOperationException($"Invalid or non-existent user ID: {userEmail}");
            }
            if (orderBy == Enums.OrderBy.status)
            {
                var myorder = _context.WinchOrders.Where(x => x.DriverId == userid).
                    OrderBy(x => x.Status == Status.Comlpeted)
                                      .ThenBy(x => x.Status == Status.inProgress)
                                      .ThenBy(x => x.Status == Status.PendingApproval)
                                      .ThenBy(x => x.Status == Status.Cancelled).ToList();
                var result = _mapper.Map<List<WinchOrderDTO>>(myorder);
                return new Response { StatusCode = 200, Model = result };
            }
            else
            {
                var myorder = _context.RepareOrders.OrderBy(x => x.Date);
                var result = _mapper.Map<List<WinchDriverDto>>(myorder);
                return new Response { Model = result, StatusCode = 200 };
            }

        }

        public async Task<Response> UpdatePersonalData(string userEmail, WinchDriverDto driverDto, IFormFile photo)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("User not found for update.");
            }
            var winch = _context.Winchs.FirstOrDefault(x => x.DriverId == user.Id);

            user.Name = driverDto.Name;
            user.CarType = driverDto.winchModel;
            user.PhotoId = await SavePersonalPhotoAsync(photo);
            user.Location =driverDto.Location;
            user.Spezilization =driverDto.Spezilization;
            winch.Model = driverDto.winchModel;
            await _context.SaveChangesAsync();
            var result = _mapper.Map<WinchDriverDto>(user);
            return new Response { Model = result, StatusCode = 200, Messege = "your data is updated successfully " };

        }

      

    }
}
