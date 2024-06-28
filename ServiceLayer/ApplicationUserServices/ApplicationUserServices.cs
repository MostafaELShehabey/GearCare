using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet;
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
                return new Response { IsDone = false, Message = "User not found.", StatusCode = 404 };
            }

            user.CarType = cartype.CarType;
            user.available = false;
            var finduser = await _userManager.UpdateAsync(user);
            if (!finduser.Succeeded)
            {
                return new Response { IsDone = false, Message = "Failed to update user data.", StatusCode = 400 };
            }
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CompleteUserDataDTO>(user);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }



        // get lis of availabe service provider 
        public async Task<Response> GetServiceProviderAvailable(UserType userType, string? location, string? Cartype, string userEmail)
        {
            // If the userType is WinchDriver
            if (userType == UserType.WinchDriver)
            {
                var query = _context.Users
                    .Where(sp => sp.UserType == userType && sp.available);

                if (!string.IsNullOrEmpty(location))
                {
                    query = query.Where(sp => sp.Location.ToLower().Contains(location.ToLower()));
                }

                //if (!string.IsNullOrEmpty(Cartype))
                //{
                //    query = query.Where(sp => sp.WinchDriver.Winch.Model.ToLower().Contains(Cartype.ToLower()));
                //}

                var winchDrivers = await query
                    .Select(driver => new WinchDriverOUTDto
                    {
                        Id = driver.Id,
                        Name = driver.Name,
                        Number = driver.PhoneNumber,
                        Location = driver.Location,
                        latitude=driver.latitude,
                        longitude=driver.longitude,
                        Spezilization = driver.Spezilization,
                        winchModel = driver.WinchDriver.Winch.Model
                    })
                    .ToListAsync();

                return new Response { IsDone = true, Model = winchDrivers };
            }

            // If the userType is Mechanic or Electrician
            if (userType == UserType.Mechanic || userType == UserType.Electrician)
            {
                var query = _context.Users
                    .Where(sp => sp.UserType == userType && sp.available);

                if (!string.IsNullOrEmpty(location))
                {
                    query = query.Where(sp => sp.Location.ToLower().Contains(location.ToLower()));
                }

                if (!string.IsNullOrEmpty(Cartype))
                {
                    query = query.Where(sp => sp.CarType.ToLower().Contains(Cartype.ToLower()));
                }

                var serviceProviders = await query.ToListAsync();
                var serviceProviderDtos = _mapper.Map<List<Service_ProviderDto>>(serviceProviders);

                return new Response { IsDone = true, Model = serviceProviderDtos, StatusCode = 200 };
            }

            // If the userType is not recognized
            return new Response { IsDone = false, Message = "Invalid user type.", StatusCode = 200 };


        }


  

        // get list of sellers  
        public async Task<Response> GetSellers(string? location)
        {
            var usertype = UserType.Seller;
            IQueryable<ApplicationUser> query = _context.Users.Where(x => x.UserType == usertype);

            if (!string.IsNullOrEmpty(location))
            {
                var locationMatches = query.Where(x => EF.Functions.Like(x.Location, $"%{location}%"));
                if (await locationMatches.AnyAsync())
                {
                    query = locationMatches.OrderBy(o => o.Location).ThenBy(r => r.Rate);
                }
                else
                {
                    query = query.OrderBy(r => r.Rate);
                }
            }
            var Sellers = await query.OrderBy(o => o.Location).ThenBy(r => r.Rate).ToListAsync();
            var result = _mapper.Map<List<Service_ProviderDto>>(Sellers);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }

        // create repare order 
        public async Task<Response> CreateRepareOrder(string userEmail, RepareOrderDto repareOrderDto)
        {
            // select a technician to send him a request 
            var user = await _userManager.FindByEmailAsync(userEmail);
            var serviceProvider = await _context.Users.Where(x => x.Id == repareOrderDto.ServiceProvierId).FirstAsync();
            if (user is null)
            {
                return new Response { Message = $"User with ID {repareOrderDto.ServiceProvierId} was not found.", StatusCode = 404, IsDone = false };
            }
            var repareOrder = new RepareOrder
            {
                ClientId = user.Id,
                Date = DateTime.Now,
                Status = Status.PendingApproval,
                cartype = repareOrderDto.cartype,
                location = repareOrderDto.location,
                ProblemDescription = repareOrderDto.ProblemDescription,
                ServiceProviderId = serviceProvider.Id,
                ClientName = user.Name,
                ClientPhoto=user.PhotoId,
                PhoneNumber=user.PhoneNumber
               
                

            };

            await _context.RepareOrders.AddAsync(repareOrder);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<RepareOrderToAccept>(repareOrder);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }



        // create winch order 
        public async Task<Response> CreateWinchOrder(string userEmail, RepareOrderDto repareOrderDto)
        {
            // Find the client (user) by email
            var client = await _userManager.FindByEmailAsync(userEmail);
            if (client is null)
            {
                return new Response { Message = $"Client with email {userEmail} was not found.", StatusCode = 404, IsDone = false };
            }

            // Find the driver by the provided ServiceProviderId (assuming repareOrderDto.ServiceProviderId is the driver's Id)
            var driver = await _context.Users.FirstOrDefaultAsync(x => x.Id == repareOrderDto.ServiceProvierId);
            if (driver is null)
            {
                return new Response { Message = $"Driver with ID {repareOrderDto.ServiceProvierId} was not found.", StatusCode = 404, IsDone = false };
            }

            // Create a new winch order
            var order = new WinchOrder
            {
                ClientId = client.Id,
                Date = DateTime.Now,
                DriverId = driver.Id,
                Status = Status.PendingApproval,
                cartype = repareOrderDto.cartype,
                location = repareOrderDto.location,
                ProblemDescription = repareOrderDto.ProblemDescription,
                ClientName = driver.Name,
                ClientPhoto = driver.PhotoId,
                PhoneNumber = driver.PhoneNumber

            };

            // Add the order to the context
            await _context.WinchOrders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Map the order and related entities to the output DTO
            var result = new WinchOrderOutDto
            {
                OrderId = order.OrderId,
                ClientId = client.Id,
                DriverId = driver.Id,
                Client = new SellerDto
                {
                    Id = client.Id,
                    Name = client.Name,
                    Location = client.Location,
                    latitude=client.latitude,
                    longitude=client.longitude,
                    PhotoId = client.PhotoId,
                    Available = client.available,
                    Specialization = client.Spezilization,
                    NumberOfRates = client.NumberOfRates,
                    UserType = client.UserType
                },
                Driver = new SellerDto
                {
                    Id = driver.Id,
                    Name = driver.Name,
                    Location = driver.Location,
                    latitude = client.latitude,
                    longitude = client.longitude,
                    PhotoId = driver.PhotoId,
                    Available = driver.available,
                    Specialization = driver.Spezilization,
                    NumberOfRates = driver.NumberOfRates,
                    Rate = driver.Rate,
                    UserType = driver.UserType
                },
                Date = order.Date,
                ProblemDescription = order.ProblemDescription,
                cartype = order.cartype,
                location = order.location,
                Status = order.Status
            };

            // Return the response
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }



        //get all product (randoum )

        public Response GetAllProducts(string? search)
        {
            // Get the queryable products from the database
            var query = _context.Products.ToList();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(p =>
                    (p.Name != null && p.Name.ToLower().Contains(search)) ||
                     p.price.ToString().Contains(search) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)) ||
                    (p.Categorys != null && p.Categorys.Name != null && p.Categorys.Name.ToLower().Contains(search))
                    ).ToList();
            }

            // Fetch and project the products
            var products = query.Select(p => new NewProductDto
            {
                Id = p.Id,
                Name = p.Name,
                PictureURL = p.PictureURL,
                Price = p.price,
                NewPrice = p.newPrice,
                Description = p.Description,
                InStock = p.instock,
                SellerId = p.SellerId,
                CategoryName = p.CategoryName != null ? p.CategoryName.ToLower() : null,
                Seller = p.Seller != null ?
                new SellerDto
                {
                    Id = p.Seller.Id,
                    Name = p.Seller.Name,
                    Location = p.Seller.Location,
                    longitude=p.Seller.longitude,
                    latitude=p.Seller.latitude,
                    PhotoId = p.Seller.PhotoId,
                    Available = p.Seller.available,
                    NumberOfRates = p.Seller.NumberOfRates,
                    Rate = p.Seller.Rate,
                    Specialization = p.Seller.Spezilization,
                    UserType = p.Seller.UserType
                } :null,
                Category = p.Categorys,
                Discount = p.Discount
            }).ToList();

            // Check if products were found
            if (products.Count() == 0 && !string.IsNullOrEmpty(search))
            {
                var result = new List<ProductDto>();
                return new Response
                {
                    Model = result,
                    IsDone = false,
                    Message = "The product does not exist, try to search with a different Name or Category!",
                    StatusCode = 404
                };
            }

            // Shuffle the products if no search term was provided
            if (string.IsNullOrEmpty(search))
            {
                products = products.OrderBy(p => Guid.NewGuid()).ToList();
            }

            return new Response
            {
                IsDone = true,
                Model = products,
                StatusCode = 200
            };
        }



        // Get all the products in the shopping cart
        public async Task<Response> GetAllProductsInShoppingCart(string userEmail)
        {
            // Validate user ID
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new Response { Message = "The provided user ID is incorrect or the user does not exist.", StatusCode = 404, IsDone = false };
            }
            var productsInCart = await _context.Product_Shoppingcarts
            .Where(ps => ps.ShoppingCart.ClientId == user.Id)
            .Select(ps => ps.Product)
            .ToListAsync();
            var result = _mapper.Map<List<ProductOutputDTO>>(productsInCart);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
            // return productsInCart;
        }


        // Get all categories
        public async Task<Response> GetAllCategories()
        {
            var categories = await _context.categories.ToListAsync();
            var result = _mapper.Map<List<CategoryDto>>(categories);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }




        // filter by category id 
        public async Task<Response> FilterByCategory(string name)
        {
            var products = await _context.Products.Where(o => o.Categorys.Name == name).ToListAsync();
            var result = _mapper.Map<List<ProductDto>>(products);
            return new Response { IsDone = true, Model = result, StatusCode = 200 };
        }



        //add product to my shopping cart 
        public async Task<Response> AddProductToShoppingCart(string userEmail, string productId)
        {
            //Validate user ID
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new Response { Message = "The provided user ID is incorrect or the user does not exist.", StatusCode = 404, IsDone = false };
            }
            // Validate product ID
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new Response { Message = "The provided product ID is incorrect or the product does not exist.", StatusCode = 404, IsDone = false };
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
            _context.Product_Shoppingcarts.Add(productShoppingCart);
            await _context.SaveChangesAsync();
            return new Response { IsDone = true, Message = "Product added Successfully", StatusCode = 200 };
        }




        // remove product from my shopping card 
        public async Task<Response> RemoveProductFromShoppingCart(string userEmail, string productId)
        {
            // Validate user ID
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new Response { Message = "The provided user ID is incorrect or the user does not exist.", StatusCode = 404, IsDone = false };
            }

            // Validate product ID
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new Response { Message = "The provided product ID is incorrect or the product does not exist.", StatusCode = 404, IsDone = false };
            }

            // Get the user's shopping cart
            var shoppingCart = await _context.ShoppingCarts
                .Include(sc => sc.product_Shoppingcart)
                .ThenInclude(psc => psc.Product)
                .FirstOrDefaultAsync(sc => sc.ClientId == user.Id);
            if (shoppingCart == null)
            {
                return new Response { Message = "Shopping cart not found.", StatusCode = 404, IsDone = false };
            }

            if (shoppingCart.product_Shoppingcart == null)
            {
                return new Response { Message = "Shopping cart is empty.", StatusCode = 404, IsDone = false };
            }

            // Find the product in the shopping cart
            var productShoppingCart = shoppingCart.product_Shoppingcart
                .FirstOrDefault(psc => psc.Product.Id == productId);
            if (productShoppingCart == null)
            {
                return new Response { Message = "This product does not exist in your shopping cart.", StatusCode = 404, IsDone = false };
            }

            // Remove the product from the shopping cart
            shoppingCart.product_Shoppingcart.Remove(productShoppingCart);
            _context.Product_Shoppingcarts.Remove(productShoppingCart);  
            await _context.SaveChangesAsync();

            return new Response { IsDone = true, Message = "Product removed successfully.", StatusCode = 200 };
        }



        // Get the 20 best-selling product 
        public async Task<Response> GetBestSellingProduct()
        {
            var bestSellingProduct = await _context.Product_Shoppingcarts
                .GroupBy(psc => psc.Product)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(20).ToListAsync();
            if (bestSellingProduct == null)
            {
                return new Response { Message = "No products found.", StatusCode = 404, IsDone = false };
            }
            var result = _mapper.Map<List<ProductDto>>(bestSellingProduct);
            return new Response { IsDone = true, StatusCode = 200, Model = result };
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
            return new Response { Model = result, IsDone = true, StatusCode = 200 };
        }



        // give rate to seller 
        public async Task<Response> GiveRateToSeller(string userId, string sellerId, int rate)
        {
            var user = await _userManager.FindByEmailAsync(userId);
            if ((rate < 0 || rate > 5))
            {
                return new Response { Message = "Rate must be between 1 and 5.", StatusCode = 400, IsDone = false };
            }
            // Find the user who is giving the rate
            if (user == null)
            {
                return new Response { Message = "User not found.", StatusCode = 404, IsDone = false };
            }
            // Find the seller who is being rated
            var seller = await _context.Users.FindAsync(sellerId);
            if (seller == null || seller.UserType != UserType.Seller)
            {
                return new Response { Message = "Seller not found.", StatusCode = 404, IsDone = false };
            }
            // Check if the user has added any product from the seller to their shopping cart
            var hasProductInCart = await _context.Product_Shoppingcarts
                .AnyAsync(psc => psc.ShoppingCart.ClientId == userId && psc.Product.SellerId == sellerId);
            if (!hasProductInCart)
            {
                return new Response { Message = "You must add a product from this seller to your shopping cart before rating.", StatusCode = 400, IsDone = false };
            }
            // Update seller's rating and number of ratings
            seller.Rate += rate;
            seller.NumberOfRates++;
            await _context.SaveChangesAsync();
            var result = _mapper.Map<Service_ProviderDto>(seller);
            return new Response { StatusCode = 200, Model = result, IsDone = true };
        }



        // update personal data 
        public async Task<Response> UpdateUserData(string userEmail, UpdateApplicationUserDataDto applicationUserDto)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new Response { Message = "User not found for update.", StatusCode = 404, IsDone = false };
            }
            user.Name = applicationUserDto.Name;
            user.PhoneNumber = applicationUserDto.PhoneNumber;
            user.Location = applicationUserDto.Location;
            user.CarType = applicationUserDto.CarType;
            user.CarTypeToRepaire = null;
            user.available = false;
            user.Isbanned = false;
            await _context.SaveChangesAsync();
            var result = _mapper.Map<ApplicationUserDto>(user);
            return new Response { StatusCode = 200, Model = result, IsDone = true };
        }

        public async Task<Response> CheckProductInShoppingCart(string userEmail, string productId)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new Response { Message = "The provided user ID is incorrect or the user does not exist.", StatusCode = 404, IsDone = false };
            }

            // Validate product ID
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new Response { Message = "The provided product ID is incorrect or the product does not exist.", StatusCode = 404, IsDone = false };
            }

            // Get the user's shopping cart
            var shoppingCart = await _context.ShoppingCarts
                .Include(sc => sc.product_Shoppingcart)
                .ThenInclude(psc => psc.Product)
                .FirstOrDefaultAsync(sc => sc.ClientId == user.Id);
            if (shoppingCart == null)
            {
                return new Response { Message = "Shopping cart not found.", StatusCode = 404, IsDone = false };
            }

            if (shoppingCart.product_Shoppingcart == null)
            {
                return new Response { Message = "Shopping cart is empty.", StatusCode = 404, IsDone = false };
            }

            // Find the product in the shopping cart
            var productShoppingCart = shoppingCart.product_Shoppingcart
                .FirstOrDefault(psc => psc.ProductId == productId);
            if (productShoppingCart == null)
            {
                return new Response { Message = "False", StatusCode = 200, IsDone = false };
            }
            return new Response { Message = "True", StatusCode = 404, IsDone = true };


           
        }

        
    }
       
   
}




