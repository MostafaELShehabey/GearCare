using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainLayer.Dto;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ServiceLayer.ApplicationUserServices
{
    public class ApplicationUserServices : IApplicationUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public ApplicationUserServices(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }



        public async Task<Response> CompletePersonalData(string userEmail, AddCarTypeToUser cartype)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new Response {IsDone=false, Messege = "User not found." };
            }
            
            user.CarType = cartype.CarType;
            user.available = false;
            var finduser = await _userManager.UpdateAsync(user);
            if (!finduser.Succeeded)
            {
                return new Response { IsDone = false, Messege = "Failed to update user data." };
            }
            await _context.SaveChangesAsync();
            var result= _mapper.Map<CompleteUserData>(user);
            return new Response { IsDone=true,Model = result , StatusCode=200};
        }



        // get lis of availabe service provider 
        public async Task<Response> GetServiceProviderAvailable(UserType userType, string location, string Cartype)
        {
            if (userType.ToString() == "WinchDriver") {

                if (location == null || Cartype == null)
                {
                    var serviceProviderQuery = await _context.Users
                    .Where(sp => sp.UserType==userType && sp.available)
                    .ToListAsync();
                    var serviceProviderDtos = _mapper.Map<List<Service_ProviderDto>>(serviceProviderQuery);
                    return new Response { IsDone = true, Model = serviceProviderDtos };
                }
                else
                {
                    var serviceProviderQuery = _context.Users
                            .Where(sp => sp.UserType == userType && (
                             EF.Functions.Like(sp.Location, $"%{location}%") ||
                             EF.Functions.Like(sp.CarTypeToRepaire, $"%{Cartype}%")) &&
                                         sp.available)
                            .OrderBy(o => o.Location)
                            .ThenBy(o => o.Rate)
                            .ToList();

                    return new Response { IsDone = true, Model = serviceProviderQuery };
                }

            }else
            {
                if (location == null || Cartype == null)
                {
                    var serviceProviderQuery = await _context.Users
                    .Where(sp => sp.UserType == userType && sp.available)
                    .ToListAsync();
                    var serviceProviderDtos = _mapper.Map<List<Service_ProviderDto>>(serviceProviderQuery);
                    return new Response { IsDone = true, Model = serviceProviderDtos };
                }
                else
                {
                    var serviceProviderQuery = _context.Users
                               .Where(sp => sp.UserType == userType && (
                                EF.Functions.Like(sp.Location, $"%{location}%") ||
                                EF.Functions.Like(sp.CarTypeToRepaire, $"%{Cartype}%")) &&
                                            sp.available)
                               .OrderBy(o => o.Location)
                               .ThenBy(o => o.Rate)
                               .ToList();
                    var serviceProviderDtos = _mapper.Map<List<Service_ProviderDto>>(serviceProviderQuery);
                    return new Response { IsDone = true, Model = serviceProviderDtos, StatusCode = 200 };
                }
            }
            
          
        }





        // get list of sellers  
        public async Task<Response> GetSellers( string? location)
        {
            var usertype = UserType.Seller;
            IQueryable<ApplicationUser> query = _context.Users.Where(x => x.UserType == usertype);

            if (!string.IsNullOrEmpty(location))
            {
                var locationMatches = query.Where(x => EF.Functions.Like(x.Location,$"%{location}%"));
                if (await locationMatches.AnyAsync()) {
                    query = locationMatches.OrderBy(o => o.Location).ThenBy(r => r.Rate);
                }
                else
                {
                    query = query.OrderBy(r => r.Rate);
                }
            }
            var Sellers = await query.OrderBy(o => o.Location).ThenBy(r => r.Rate).ToListAsync();
            var result= _mapper.Map<List<Service_ProviderDto>>(Sellers);
            return new Response { IsDone=true,Model = result,StatusCode=200};
        }






        // create repare order 
        public async Task<Response> CreateRepareOrder(string clintId , RepareOrderDto repareOrderDto)
        {
            // select a technician to send him a request 
            var user = await _context.Users.Where(x => x.Id == repareOrderDto.ServiceProvierId).FirstAsync();
            if (user is  null)
            {
                throw new KeyNotFoundException($"User with ID {repareOrderDto.ServiceProvierId} was not found.");
            }
            var repareOrder = new RepareOrder
            {
                ClientId = repareOrderDto.ServiceProvierId,
                Date = DateTime.Now,
                Price = repareOrderDto.Price,
                Status = Status.PendingApproval,
                ProblemDescription = repareOrderDto.ProblemDescription,
                Client = user
            };
            repareOrder.ClientId = repareOrderDto.ServiceProvierId;
            _context.RepareOrders.Add(repareOrder);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<RepareOrderDto>(repareOrder);
            return new Response { IsDone=true,Model = result,StatusCode=200};
        }





        //get all product (randoum )
        public async Task<Response> GetAllProducts(string? search)
        {
            var query =  _context.Products.AsQueryable();
            // filter products based on name, price, or category
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    p.price.ToString().Contains(search) || // Assuming price is a string, adjust accordingly if it's a number
                    p.Description.ToLower().Contains(search) ||
                    p.Categorys.Name.ToLower().Contains(search));

                var result = await query.ToListAsync();
                if (!result.Any())
                {
                    return new Response { IsDone = false, Messege = "the product not Exit , try to search with differnt Name or Category ! ", StatusCode = 404 };
                }
                return new Response { IsDone = true, Model = result, StatusCode = 200 };
            }
            else
            {
                // Shuffle the products randomly
                // Fetch the data into memory and then order it randomly
                // Fetch data into memory
                var product = await query.OrderBy(p => Guid.NewGuid()).ToListAsync();// Shuffle in memory
                return new Response { IsDone = true, Model = query ,StatusCode=200};
            }
        }




        // Get all the products in the shopping cart
        public async Task<Response> GetAllProductsInShoppingCart(string userEmail)
        {
            // Validate user ID
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("The provided user ID is incorrect or the user does not exist.");
            }
            var productsInCart = await _context.product_Shoppingcarts
            .Where(ps => ps.ShoppingCart.ClientId == user.Id)
            .Select(ps => ps.Product)
            .ToListAsync();
             var result = _mapper.Map<List<ProductDto>>(productsInCart);
            return new Response { IsDone=true , Model = result,StatusCode=200 };
           // return productsInCart;
        }


        // Get all categories
        public async Task<Response> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            var result = _mapper.Map<List<CategoryDto>>(categories);
            return new Response { IsDone=true,Model = result,StatusCode = 200 };
        }

      


        // filter by category id 
        public async Task<Response> FilterByCategory (string id)
        {
            var products =await _context.Products.Where(o => o.Categorys.Id == id).ToListAsync();
            var result = _mapper.Map<List<ProductDto>>(products);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }



        //add product to my shopping cart 
        public async  Task<Response> AddProductToShoppingCart(string userEmail, string productId)
        {
            //Validate user ID
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("The provided user ID is incorrect or the user does not exist.");
            }
            // Validate product ID
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("The provided product ID is incorrect or the product does not exist.");
            }
            // Get the user's shopping cart, or create a new one if it doesn't exist
            var shoppingCart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(sc => sc.ClientId == user.Id); // Assuming UserId is the correct property
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    ClientId = user.Id,
                    ProductId = productId
                };
                _context.ShoppingCarts.Add(shoppingCart);
                await _context.SaveChangesAsync();
            }
            // Create a new link between product and shopping cart
            var productShoppingCart = new Product_Shoppingcart
            {
                ShoppingCart = shoppingCart, // Use navigation property
                ProductId = productId
            };
            _context.product_Shoppingcarts.Add(productShoppingCart);
            await _context.SaveChangesAsync();
            return new Response { IsDone = true,Messege= "Product added Successfully", StatusCode = 200 };
        }




        // remove product from my shopping card 
        public async Task<Response> RemoveProductFromShoppingCart(string userEmail, string productId)
        {
            // Validate user ID
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("The provided user ID is incorrect or the user does not exist.");
            }
            // Validate product ID
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("The provided product ID is incorrect or the product does not exist.");
            }
            // Get the user's shopping cart
            var shoppingCart = await _context.ShoppingCarts
                .Include(sc => sc.Client)
                .Include(sc => sc.product_Shoppingcart)
                .FirstOrDefaultAsync(sc => sc.Client.Id == user.Id);
            if (shoppingCart == null)
            {
                throw new InvalidOperationException("Shopping cart not found.");
            }
            // Find the product in the shopping cart
            var productShoppingCart = shoppingCart.product_Shoppingcart
                .FirstOrDefault(psc => psc.Product.Id == productId);
            if (productShoppingCart == null)
            {
                throw new InvalidOperationException("The product is not in the shopping cart.");
            }
            // Remove the product from the shopping cart
            shoppingCart.product_Shoppingcart.Remove(productShoppingCart);
            await _context.SaveChangesAsync();
            return new Response { IsDone = true,Messege= "Product removed Successfully ", StatusCode = 200 };
        }



        // Get the 20 best-selling product 
        public async Task<Response> GetBestSellingProduct()
        {
            var bestSellingProduct = await _context.product_Shoppingcarts
                .GroupBy(psc => psc.Product)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(20).ToListAsync(); 

            if (bestSellingProduct == null)
            {
                throw new InvalidOperationException("No products found.");
            }

            var result =  _mapper.Map<List<ProductDto>>(bestSellingProduct);
            return new Response { IsDone=true, StatusCode = 200 , Model=result};
        }




        // Get best sellers based on the number of times their products are added to shopping carts
        public async Task<Response> GetBestSellers()
        {
            var users = await _context.Users
                .Where(u => u.UserType == UserType.Seller)
                .Include(u => u.Products)
                .ThenInclude(p => p.product_Shoppingcart)
                .ToListAsync();

            var bestSellers = users
                .Select(u => new
                {
                    User = u,
                    ProductCount = u.Products?.Sum(p => p.product_Shoppingcart.Count) ?? 0
                })
                .OrderByDescending(u => u.ProductCount)
                .Select(u => u.User)
                .ToList();
            var result = _mapper.Map<List<ApplicationUserDto>>(bestSellers);
          return new Response { Model = result , IsDone=true , StatusCode=200};
        }



        // give rate to seller 
        public async Task<Response> GiveRateToSeller(string userId, string sellerId, int rate)
        {
            var user= await _userManager.FindByEmailAsync(userId);

            if ((rate<0||rate>5))
            {
                return new Response { Messege = "Rate must be between 1 and 5." , StatusCode=200 , IsDone=false};
            }
            // Find the user who is giving the rate
            if (user == null)
            {
                return new Response { Messege = "User not found.", StatusCode = 200, IsDone = false };

               
            }
            // Find the seller who is being rated
            var seller = await _context.Users.FindAsync(sellerId);
            if (seller == null || seller.UserType != UserType.Seller)
            {
                return new Response { Messege = "Seller not found.", StatusCode = 200, IsDone = false };

            }
            // Check if the user has added any product from the seller to their shopping cart
            var hasProductInCart = await _context.product_Shoppingcarts
                .AnyAsync(psc => psc.ShoppingCart.ClientId == userId && psc.Product.SellerId == sellerId);
            if (!hasProductInCart)
            {
                return new Response { Messege = "You must add a product from this seller to your shopping cart before rating.", StatusCode = 200, IsDone = false };
            }
            // Update seller's rating and number of ratings
            seller.Rate += rate;
            seller.NumberOfRates++;
            await _context.SaveChangesAsync();
            var result = _mapper.Map<Service_ProviderDto>(seller);
            return new Response { StatusCode=200, Model=result, IsDone = true };
        }



        // update personal data 
        public async Task<Response> UpdateUserData(string userEmail,UpdateApplicationUserDataDto applicationUserDto)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                throw new InvalidOperationException("User not found for update.");
            }
            user.Name = applicationUserDto.Name;
            user.PhoneNumber = applicationUserDto.PhoneNumber;
            user.Location = applicationUserDto.Location;
            user.CarType = applicationUserDto.CarType;
            user.CarTypeToRepaire = null;
            user.available = false;
            user.Isbanned = false;
            await _context.SaveChangesAsync();
            var  result =_mapper.Map<ApplicationUserDto>(user);
            return new Response { StatusCode=200,Model=result, IsDone = true };
        }

       
    }
}




