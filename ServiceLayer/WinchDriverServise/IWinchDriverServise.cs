using System.Threading.Tasks;
using DomainLayer.Dto;
using Microsoft.AspNetCore.Http;
using static DomainLayer.Helpers.Enums;

namespace ServiceLayer.WinchDriverService
{
    public interface IWinchDriverService
    {
        Task<Response> CompleteWinchData(string userEmail, WinchModel winchdataIFormFile, IFormFileCollection WinchlicencePhoto , IFormFileCollection winchPhoto);
       // Task<Response> AddIDphoto(IFormFile photo, string userEmail);
        Task<Response> HandleOrderAction(string userEmail, string orderId, OrderAction action);
        Task<Response> GetMyOrderToAccept(string userEmail);
        Task<Response> GetOrdersHistory(string userEmail, OrderBy orderBy);
        Task<Response> UpdatePersonalData(string userEmail, WinchDriverDto serviceProviderDto, IFormFile photo);
    }
}
