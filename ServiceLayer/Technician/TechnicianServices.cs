using AutoMapper;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DomainLayer.Dto;
using DomainLayer.Helpers;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;
using Microsoft.Extensions.Options;

namespace ServiceLayer.Technician
{
    public class TechnicianServices : ITechnicianService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Cloudinary _cloudinary;


        public TechnicianServices(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IOptions<CloudinarySettings> config)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            var acc = new Account(
                 config.Value.CloudName,
                 config.Value.ApiKey,
                 config.Value.ApiSecret
             );

            _cloudinary = new Cloudinary(acc);
        }


        private async Task<string> SaveIDPhotoAsync(IFormFile file)
        {

            if (file == null || file.Length == 0)
                throw new ArgumentNullException("file", "No file uploaded");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "IDPhotos"
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

        public async Task<Response> CompletePersonalData(string userEmail, ServiceProvideroutDTO? ServiceProvider,IFormFile? IDphoto)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new Response { IsDone = false, Message = "User not found." , StatusCode=404};
            } 
            if (IDphoto == null)
            {
                return new Response { IsDone = false, Message = " ID picture is required ." , StatusCode=404};
            } 
            
            if (ServiceProvider.CarTypeToRepaire == null||ServiceProvider.Spezilization.Count()==0)
            {
                return new Response { IsDone = false, Message = " ar type and specilization are required " , StatusCode=404};
            }
            user.CarTypeToRepaire=ServiceProvider.CarTypeToRepaire;
            user.Spezilization=ServiceProvider.Spezilization;
            user.available = true;
            user.IdPicture = await SaveIDPhotoAsync(IDphoto);
            var finduser = await _userManager.UpdateAsync(user);
            if (!finduser.Succeeded)
            {
                return new Response { IsDone = false, Message = "Failed to update user data." , StatusCode = 404};
            }
            await _context.SaveChangesAsync();
            var result = _mapper.Map<ServiceProvideroutDTO>(user);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }


        public async Task<Response> GetAllRepareOrderToAccept(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                return new Response { Message = $"Invalid or non-existent user ID: {userEmail}" , StatusCode=404, IsDone=false};
            }

            var orders = await _context.RepareOrders
                .Where(x => x.ServiceProviderId == user.Id && x.Status == Status.PendingApproval)
                .ToListAsync();


            var result = _mapper.Map<List<RepareOrderToAccept>>(orders); 
            return new Response { Model = orders, StatusCode = 200 };
        }

        public async Task<Response> HandleOrderAction(string userEmail, string orderId, OrderAction action)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                return new Response { Message = $"Invalid or non-existent user ID: {userEmail}", StatusCode = 404, IsDone = false };
            }

            var order = await _context.RepareOrders.FirstOrDefaultAsync(sp => sp.ServiceProviderId == user.Id && sp.OrderId == orderId);
            if (order == null)
            {
                return new Response { Message = "This user ID or order ID does not exist.", StatusCode = 404, IsDone = false };
             
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
                    user.available = true;
                    break;
                case OrderAction.Completed:
                    order.Status = Status.Completed;
                    user.available = true;
                    break;
                default:
                    return new Response { Message = $"Invalid action: {action}", StatusCode = 400, IsDone=false };
            }

            await _context.SaveChangesAsync();

         // var result = _mapper.Map<RepareOrderToAccept>(order);
            var client =_context.Users.Where(x=>x.Id==order.ClientId);
            var result = new RepaireOrderOutDto
            {
                OrderId = orderId,
                ClientId = order.ClientId,
                ServiceProviderId = user.Id,
                cartype = order.cartype,
                location = order.location,
                Date = order.Date,
                Status = order.Status,
                Client = _mapper.Map<SellerDto>(client)

            };
            return new Response { Model = result, StatusCode = 200 };
        }

        public async Task<Response> UpdatePersonaldata(string userEmail, UpdateServiceProviderDataDTO serviceProviderDto,IFormFile photo)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                return new Response { Message = $"Invalid or non-existent user : {userEmail}", StatusCode = 404, IsDone = false };
            }

            user.Name = serviceProviderDto.Name;
            user.PhoneNumber = serviceProviderDto.Number;
            user.Email = serviceProviderDto.CarTypeToRepaire;
            user.Location = serviceProviderDto.Location;
            user.Spezilization = serviceProviderDto.Spezilization;
            user.PhotoId = await SavePersonalPhotoAsync(photo);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<Service_ProviderDto>(user);
            return new Response { Model = result, StatusCode = 200, Message = "Your data is updated successfully" };
        }

        public async Task<Response> GetOrderHistory(string userEmail, Enums.OrderBy orderBy)//Return Canceled or Completed
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                return new Response { Message = $"Invalid or non-existent user ID: {userEmail}", StatusCode = 404, IsDone = false };
            }


            var query = _context.RepareOrders.Where(x => x.ServiceProviderId == user.Id && (x.Status == (Status.Completed) || (x.Status == Status.Cancelled)));

            query.Select(x => x.User.applicationUsers);
            if (orderBy == Enums.OrderBy.status)
            {
                query = query.OrderBy(x => x.Status == Status.Completed)
                             .ThenBy(x => x.Status == Status.Cancelled);
            }
            else
            {
                query = query.OrderBy(x => x.Date);
            }

            var orders = await query.ToListAsync();

            var result = _mapper.Map<List<RepareOrderToAccept>>(orders);
            // var result = _mapper.Map<List<RepareOrderToAccept>>(orders);

           
            return new Response { StatusCode = 200, Model = result };
        }

        public async Task<Response> CurrentOrder(string userEmail, Enums.OrderBy orderBy)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                return new Response { Message = $"Invalid or non-existent user ID: {userEmail}", StatusCode = 404, IsDone = false };
            }


            var query = _context.RepareOrders.Where(x => x.ServiceProviderId == user.Id && (x.Status == (Status.inProgress) || (x.Status == Status.PendingApproval)));

            query.Select(x => x.User.applicationUsers);
            if (orderBy == Enums.OrderBy.status)
            {
                query = query.OrderBy(x => x.Status == Status.inProgress)
                             .ThenBy(x => x.Status == Status.PendingApproval);
            }
            else
            {
                query = query.OrderBy(x => x.Date);
            }

            var orders = await query.ToListAsync();

            var result = _mapper.Map<List<RepareOrderToAccept>>(orders);
            // var result = _mapper.Map<List<RepareOrderToAccept>>(orders);


            return new Response { StatusCode = 200, Model = result };
        }
    }
}
