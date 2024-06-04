using DomainLayer.Dto;
using DomainLayer.Helpers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ServiceLayer.Technician
{
    public interface ITechnicianService
    {
        Task<Response> AddIDphoto(IFormFile photo, string userEmail);
        Task<Response> CompletePersonalData(string userEmail, ServiceProvideroutDTO user);
        Task<Response> GetAllRepareOrderToAccept(string userEmail);
        Task<Response> GetOrderHistory(string userEmail, Enums.OrderBy orderBy);
        Task<Response> HandleOrderAction(string userEmail, string orderId, Enums.OrderAction action);
        Task<Response> UpdatePersonaldata(string userEmail, Service_ProviderDto serviceProviderDto);
    }
}
