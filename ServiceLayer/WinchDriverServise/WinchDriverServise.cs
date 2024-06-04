using AutoMapper;
using DomainLayer.Dto;
using DomainLayer.Helpers;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public WinchDriverServise(ApplicationDbContext context, IMapper mapper,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Response> CompleteWinchData(string userEmail, WinchModel winchdata)
        {
            var driver = await _userManager.FindByEmailAsync(userEmail);
            if (driver == null)
            {
                return new Response { IsDone = false, Messege = "User not found." };
            }
            driver.Spezilization = winchdata.Spezilization;
            var winch =  await _context.Winchs.FirstOrDefaultAsync(x => x.WinchDriver.Id == driver.Id);
            var data = new Winch
            {
                Model = winchdata.Model,
                DriverId = driver.Id,
                Availabile = true,
                Licence=null,
                photo=null
      
            };

            await _context.Winchs.AddAsync(data);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<WinchDto>(data);
            return new Response { IsDone = true,Model = result, StatusCode =200};
        }


        public async Task<Response> AddIDphoto(IFormFile photo, string userEmail)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new InvalidOperationException("The photo is not added");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var photoUpload = Path.Combine(Directory.GetCurrentDirectory(), "images", "WinchDriverIDphotos");
            if (!Directory.Exists(photoUpload))
            {
                Directory.CreateDirectory(photoUpload);
            }
            var photoUniquname = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
            var photoPath = Path.Combine(photoUpload, photoUniquname).Replace("\\", "/");
            using (var stream = new FileStream(photoPath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            // Save photo path to database
            if (user.IdPicture != null)
            {
                // Update existing photo path
                user.IdPicture = photoPath;
            }
            else
            {
                // Create new photo entry
                user.IdPicture = photoPath;
            }

            await _context.SaveChangesAsync();


            var result = _mapper.Map<CompleteSelerData>(user);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }


        public async Task<Response> AddLicencePhoto(IFormFile photo, string userEmail)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new InvalidOperationException("The photo is not added");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            var winch = await _context.Winchs.FirstOrDefaultAsync(x => x.DriverId == user.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var photoUpload = Path.Combine(Directory.GetCurrentDirectory(), "images", "WinchDriverLicencephotos");
            if (!Directory.Exists(photoUpload))
            {
                Directory.CreateDirectory(photoUpload);
            }
            var photoUniquname = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
            var photoPath = Path.Combine(photoUpload, photoUniquname).Replace("\\", "/");
            using (var stream = new FileStream(photoPath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }
            // Save photo path to database
            if (winch.Licence != null)
            {
                // Update existing photo path
                winch.Licence = photoPath;
            }
            else
            {
                // Create new photo entry
                winch.Licence = photoPath;
            }
            await _context.SaveChangesAsync();
            var result = _mapper.Map<WinchDto>(winch);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }


        public async Task<Response> AddWinchPhoto(IFormFile photo, string userEmail)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new InvalidOperationException("The photo is not added");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            var winch = await _context.Winchs.FirstOrDefaultAsync(x => x.DriverId == user.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var photoUpload = Path.Combine(Directory.GetCurrentDirectory(), "images", "Winchphotos");
            if (!Directory.Exists(photoUpload))
            {
                Directory.CreateDirectory(photoUpload);
            }
            var photoUniquname = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
            var photoPath = Path.Combine(photoUpload, photoUniquname).Replace("\\", "/");
            using (var stream = new FileStream(photoPath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            // Save photo path to database
            if (winch.photo != null)
            {
                // Update existing photo path
                winch.photo = photoPath;
            }
            else
            {
                // Create new photo entry
                winch.photo = photoPath;
            }

            await _context.SaveChangesAsync();

            winch.DriverId=user.Id;
            var result = _mapper.Map<WinchDto>(winch);
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

        public async Task<Response> UpdatePersonalData(string userEmail, WinchDriverDto driverDto)
        {
            var userdata = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            var userid = userdata.Id;

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                throw new InvalidOperationException("User not found for update.");
            }
            user.Name = driverDto.Name;
            user.CarType = driverDto.winchModel;
            user.Location =driverDto.Location;
            user.Spezilization =driverDto.Spezilization;
            await _context.SaveChangesAsync();
            var result = _mapper.Map<Service_ProviderDto>(user);
            return new Response { Model = result, StatusCode = 200, Messege = "your data is updated successfully " };
        }

       
    }
}
