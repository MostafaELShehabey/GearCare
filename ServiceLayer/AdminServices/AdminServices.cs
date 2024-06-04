using AutoMapper;
using DomainLayer.Dto;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace ServiceLayer.AdminServices
{
    public class AdminServices// :BaseService<ApplicationUser>,IBaseService<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AdminServices(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IQueryable<Service_ProviderDto> GetServiceProviderType(UserType serviceProviderType)
        {
            var serviceProvidersQuery = _context.Users
           .Where(c => c.UserType == serviceProviderType);
            var serviceProviderDtos = _mapper.ProjectTo<Service_ProviderDto>(serviceProvidersQuery);
            return serviceProviderDtos;
        }
        //number of user 
        //public int NumOfUser()
        //{
        //    var result = _context.Users.Count();
        //    return result;
        //}

        //public int NumOfWinchrivers()
        //{
        //    throw new NotImplementedException();
        //}

        // Num Of Each Service Provider
        public int FilterByUser(UserType usertypeType)
        {

            var serviceProvidersQuery = _context.Users
           .Where(c => c.UserType == usertypeType);
            var serviceProviderDtos = _mapper.ProjectTo<Service_ProviderDto>(serviceProvidersQuery);
            return serviceProviderDtos.Count();
           // return GetServiceProviderType(ServiceProviderType).ToList().Count();
        }
        // number of each user 
        public int NumOfSellers(ServiceProviderType serviceProviderType)
        {
            var sellers = _context.Users.Where(t => t.UserType == UserType.Seller).Count();
            return sellers;
        }

        //Num Of All Buying Transaction
        public int NumOfAllBuyingTransaction()
        {
            return _context.ShoppingCarts.Count();//Need check it later 
        }
        //Num Of All RepareOrders
        public int NumOfAllRepareOrders(Status status)
        {
            return _context.RepareOrders.Where(u => u.Status == status).Count();
        }

        // Num Of All Winch Orders
        public int NumOfAllWinchOrders(Status status)
        {
            return _context.WinchOrders.Where(u => u.Status == status).Count();
        }

        // Ban ServiceProvider By Id Async
        public async Task<bool> BanServiceProviderByIdAsync(string id)
        {
            var serviceprovider = await _context.Users.FindAsync(id);
            if (serviceprovider != null)
            {
                serviceprovider.Isbanned = true;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        // Get All Repair Orders Async
        public async Task<List<RepareOrderDto>> GetAllRepairOrdersAsync(Status statusType, string search, int page, int pageSize)
        {
            var query = await _context.RepareOrders.Include(x => x.Client)
                                             .Where(ro => ro.Status == statusType)
                                                 .ToListAsync();
            var  stateus = statusType.ToString();
            if (!string.IsNullOrEmpty(stateus))
            {
                query = query.Where(ro =>
                    ro.Client.Name.Contains(search) ||
                    ro.Client.PhoneNumber.Contains(search) ||
                    ro.Client.CarType.Contains(search) ||
                    ro.Client.Email.Contains(search) ||
                    ro.Client.Location.Contains(search)
                ).ToList();
            }
            var repairOrders = query.Skip((page - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToList();
            var serviceProviderDtos = _mapper.Map<List<RepareOrderDto>>(repairOrders);
            return serviceProviderDtos;
        }
        //Get ServiceProvider By Id Async
        public async Task<Service_ProviderDto> GetServiceProviderByIdAsync(string id)
        {
            var service = await _context.Users.FindAsync(id);
            var serviceProviderDtos = _mapper.Map<Service_ProviderDto>(service);
            return serviceProviderDtos;
        }

        public Task<List<Service_ProviderDto>> GetServiceProvidersAsync(int page, int pageSize, UserType serviceProviderType, string search)
        {
            var query = _context.Users.Where(sp => sp.UserType == serviceProviderType).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(sp => sp.Name.Contains(search) ||
                                          sp.Location.Contains(search) ||
                                          sp.Spezilization.Contains(search) ||
                                          sp.CarTypeToRepaire.Contains(search)
                                          ).ToList();
            }
            var service = query.Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();
            var serviceProviderDtos = _mapper.Map<List<Service_ProviderDto>>(service);

            return Task.FromResult(serviceProviderDtos);
        }

        public IQueryable<Service_ProviderDto> GetTopFiveServiceProvider(UserType serviceProviderType)
        {

            var topFiveProviders = _context.Users.Where(sp => sp.UserType == serviceProviderType) // Direct comparison
                .OrderByDescending(o => o.Rate)
                .Take(5)
                .ToList().AsQueryable(); // Materialize the query into a list

            var dtos = topFiveProviders.Select(sp => _mapper.Map<Service_ProviderDto>(sp)); // Project to DTOs

            return dtos;
        }

        
    }
}
