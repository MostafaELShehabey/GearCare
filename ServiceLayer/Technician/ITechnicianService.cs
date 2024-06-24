using DomainLayer.Dto;
using DomainLayer.Helpers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ServiceLayer.Technician
{
    public interface ITechnicianService
    {
        Task<Response> CompletePersonalData(string userEmail, ServiceProvideroutDTO user, IFormFile IDphoto);
        Task<Response> GetAllRepareOrderToAccept(string userEmail);
        Task<Response> GetOrderHistory(string userEmail, Enums.OrderBy orderBy);// canceled or completed 
        Task<Response> CurrentOrder(string userEmail, Enums.OrderBy orderBy);// currunt order , status = inprogress to can handel it 
        Task<Response> HandleOrderAction(string userEmail, string orderId, Enums.OrderAction action);
        Task<Response> UpdatePersonaldata(string userEmail, UpdateServiceProviderDataDTO serviceProviderDto, IFormFile photo );
    }
}
