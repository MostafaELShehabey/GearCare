using AutoMapper;
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

namespace ServiceLayer.Technician
{
    public class TechnicianServices : ITechnicianService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TechnicianServices(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
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

            var photoUpload = Path.Combine(Directory.GetCurrentDirectory(), "images", "TechniccianIDphotos");
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


        public async Task<Response> CompletePersonalData(string userEmail, ServiceProvideroutDTO ServiceProvider)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new Response { IsDone = false, Messege = "User not found." };
            }
            user.CarTypeToRepaire=ServiceProvider.CarTypeToRepaire;
            user.Spezilization=ServiceProvider.Spezilization;
            user.available = true;
            var finduser = await _userManager.UpdateAsync(user);
            if (!finduser.Succeeded)
            {
                return new Response { IsDone = false, Messege = "Failed to update user data." };
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
                throw new InvalidOperationException($"Invalid or non-existent user ID: {userEmail}");
            }

            var orders = await _context.RepareOrders
                .Where(x => x.ClientId == user.Id && x.Status == Status.PendingApproval)
                .ToListAsync();

            var result = _mapper.Map<List<RepareOrderToAccept>>(orders);
            return new Response { Model = result, StatusCode = 200 };
        }

        public async Task<Response> HandleOrderAction(string userEmail, string orderId, OrderAction action)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                throw new InvalidOperationException($"Invalid or non-existent user ID: {userEmail}");
            }

            var order = await _context.RepareOrders.FirstOrDefaultAsync(sp => sp.ClientId == user.Id && sp.OrderId == orderId);
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
                    user.available = true;
                    break;
                case OrderAction.Completed:
                    order.Status = Status.Comlpeted;
                    user.available = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), $"Invalid action: {action}");
            }

            await _context.SaveChangesAsync();

            var result = _mapper.Map<RepareOrderToAccept>(order);
            return new Response { Model = result, StatusCode = 200 };
        }

        public async Task<Response> UpdatePersonaldata(string userEmail, Service_ProviderDto serviceProviderDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                throw new InvalidOperationException($"Invalid or non-existent user ID: {userEmail}");
            }

            user.Name = serviceProviderDto.Name;
            user.PhoneNumber = serviceProviderDto.Number;
            user.Email = serviceProviderDto.CarTypeToRepaire;
            user.Location = serviceProviderDto.Location;
            user.Spezilization = serviceProviderDto.Spezilization;
            user.PhotoId = serviceProviderDto.PhotoId;
            await _context.SaveChangesAsync();

            var result = _mapper.Map<Service_ProviderDto>(user);
            return new Response { Model = result, StatusCode = 200, Messege = "Your data is updated successfully" };
        }

        public async Task<Response> GetOrderHistory(string userEmail, Enums.OrderBy orderBy)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                throw new InvalidOperationException($"Invalid or non-existent user ID: {userEmail}");
            }

            var query = _context.RepareOrders.Where(x => x.ClientId == user.Id);

            if (orderBy == Enums.OrderBy.status)
            {
                query = query.OrderBy(x => x.Status == Status.Comlpeted)
                             .ThenBy(x => x.Status == Status.inProgress)
                             .ThenBy(x => x.Status == Status.PendingApproval)
                             .ThenBy(x => x.Status == Status.Cancelled);
            }
            else
            {
                query = query.OrderBy(x => x.Date);
            }

            var orders = await query.ToListAsync();
            var result = _mapper.Map<List<RepareOrderToAccept>>(orders);

            return new Response { StatusCode = 200, Model = result };
        }
    }
}
