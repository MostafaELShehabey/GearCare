using DomainLayer.Dto;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace ServiceLayer.ApplicationUserServices
{
    public interface IApplicationUserServices
    {

        Task<Response> CompletePersonalData(string userId, AddCarTypeToUser userDto);

        // Get a list of available service providers based on type, location, and car type.
        Task<Response> GetServiceProviderAvailable(UserType userType, string? location, string? Cartype, string userEmail);
        Task<Response> GetAvailableWinchDriver(UserType userType, string location, string Cartype);

        //Get list of sellers.
        Task<Response> GetSellers(string? location);

        //Create a repair order.
        Task<Response> CreateRepareOrder(string userId ,RepareOrderDto repareOrderDto);

        //Get all products(optionally filtered by search criteria)
        Response GetAllProducts(string? search);

        //Get all products in the shopping cart for a user.
        Task<Response> GetAllProductsInShoppingCart(string userId);
        
        // Get all categories.
        Task<Response> GetAllCategories();

        // Filter products by category ID.
        Task<Response> FilterByCategory(string name);

        // Add a product to the shopping cart for a user
        Task<Response> AddProductToShoppingCart(string userId, string productId);

         Task<Response> RemoveProductFromShoppingCart(string userId, string productId);

        // Get the best-selling product.
        Task<Response> GetBestSellingProduct();

        // Get the best sellers based on the number of times their products are added to shopping carts.
        Task<Response> GetBestSellers();

        // Give a rating to a seller.
        Task<Response> GiveRateToSeller(string userId, string sellerId, int rate);

        // Update user data.
        Task<Response> UpdateUserData(string userEmail, UpdateApplicationUserDataDto applicationUserDto);

    }



    //massage the service provider 


    // get notification when recieve massage 

}

