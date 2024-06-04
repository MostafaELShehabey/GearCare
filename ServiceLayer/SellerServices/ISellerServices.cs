using DomainLayer.Dto;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.SellerServices
{
    public interface ISellerServices
    {
        Task<Response> AddIDphoto(IFormFile photo, string userEmail);

        Task<Response> AddProduct(ProductDto productDto,string userId);
        Task<Response> UpdateProduct(string productId, ProductDto productDto);
        Task< Response >DeleteProduct(string productId);

        Task<Response> MakeDiscount(DiscountDto discountDto);
        Response GetMyDiscounts();

        Task<Response> AddDiscountToProduct(string productId, string discountid);
        Task<Response> RemoveDiscountFromProduct(string productId );
        Response GetMyProducts(string email);
        Task<Response> UpdatePersonalData(string userId, UpdateSellerDataDTO userDto);

    }
}
