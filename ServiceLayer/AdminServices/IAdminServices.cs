using DomainLayer.Dto;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace ServiceLayer.AdminServices
{
    public interface IAdminServices //: IBaseService<ApplicationUser>
    {
        IQueryable<Service_ProviderDto> GetServiceProviderType(UserType UserType);
        int NumOfUser();
        int NumOfEachServiceProvider(UserType UserType);
        int NumOfSellers(UserType UserType);
        int NumOfWinchrivers();
        int NumOfAllBuyingTransaction();
        int NumOfAllRepareOrders(Status status);
        int NumOfAllWinchOrders(Status status);
        Task<bool> BanServiceProviderByIdAsync(string id);
        Task<List<RepareOrderDto>> GetAllRepairOrdersAsync(Status statusType, string search, int page, int pageSize);
        Task<Service_ProviderDto> GetServiceProviderByIdAsync(string id);
        Task<List<Service_ProviderDto>> GetServiceProvidersAsync(int page, int pageSize, UserType UserType, string search);
        IQueryable<Service_ProviderDto> GetTopFiveServiceProvider(UserType userType);
    }
}
