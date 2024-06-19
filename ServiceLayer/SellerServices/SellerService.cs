using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DomainLayer.Dto;
using DomainLayer.Helpers;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.SellerServices
{
    public class SellerService : ISellerServices
    {
        private readonly UserManager<ApplicationUser> _userManager ;
        private readonly IMapper _mapper ;
        private readonly ApplicationDbContext _context ;
        private readonly Cloudinary _cloudinary;
        public SellerService(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context, IOptions<CloudinarySettings> config)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;

                var acc = new Account(
                    config.Value.CloudName,
                    config.Value.ApiKey,
                    config.Value.ApiSecret
                );

            _cloudinary = new Cloudinary(acc);
                
        }

        // Add ID photo 

        public async Task<Response> AddIDphoto(IFormFile photo, string userEmail)
        {
            if (photo == null || photo.Length == 0)
            {
                return new Response { Message = "The photo is not added", StatusCode = 400 , IsDone=false  };          }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);

            if (user == null)
            {
                return new Response {Message= "User not found", IsDone = false , StatusCode=404 };
            }

           
            var photoPath = await SaveSellerIDPhotoAsync(photo);
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





        private async Task<string> SaveProductPhotoAsync(IFormFile file)
        {

            if (file == null || file.Length == 0)
                throw new ArgumentNullException("file", "No file uploaded");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "ProductPhotos"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.Uri.ToString();
            }
        }

        private async Task<string> SaveSellerIDPhotoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException("file", "No file uploaded");

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "SellerID"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.Uri.ToString();
            }
        }



        // Add product
        public async Task<Response> AddProduct(ProductDto? productDto,IFormFileCollection images ,string userEmail)
        {

            var seller = _context.Users.First(x=>x.Email==userEmail);

            var sellerId = seller.Id;
            if (productDto.Name==null || productDto.Price <= 0 || productDto.Description==null || sellerId==null)// Logical Error 
            {
                return new Response { Message = "Invalid product or seller information." , StatusCode = 404, IsDone = false };
            }

            var category = await _context.Categories.AnyAsync(x => x.Id == productDto.CategoryId);
            if(!category)
            {
                return new Response { IsDone = false , Message = "Category NOT Found !", StatusCode=404};
            }
            var urls = new List<string>();
            foreach (var photo in images)
            {
                var  photoPath =await SaveProductPhotoAsync(photo);
                urls.Add(photoPath);
            }

           
            
            var product = new Product
            { 
                Name = productDto.Name,
                price = productDto.Price,
                Description = productDto.Description,
                PictureURL = urls,
                SellerId = sellerId,
                Categoryid = productDto.CategoryId,
                Seller= seller,
                instock = true,
                deleted = false,
                Discount = null
            };
            // Ensure the foreign key entities exist in the database before adding the product
            if (!await _context.Users.AnyAsync(u => u.Id == sellerId))
            {
                return new Response { Message = "Seller not found", IsDone=false , StatusCode=404};
            }

            if (!await _context.Categories.AnyAsync(c => c.Id == productDto.CategoryId))
            {
                return new Response {Message= "Category not found" , StatusCode = 404 , IsDone=false};
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ProductOutputDTO>(product);
            return new Response { IsDone = true, Model = result,StatusCode=200 };
        }

      

        //Delete product photo 
        public async Task<Response> DeleteProductPhoto(string productId, string photoUrl, string userEmail)
        {
            // Fetch the seller
            var seller = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (seller == null)
            {
                return new Response { Message = "Seller not found.", StatusCode = 404 , IsDone = false };
            }

            // Fetch the product
            var product = await _context.Products.Include(p => p.PictureURL).FirstOrDefaultAsync(p => p.Id == productId && p.SellerId == seller.Id);
            if (product == null)
            {
                return new Response { Message = "Product not found or you're not authorized.", StatusCode = 404 , IsDone=false};
            }

            // Check if the photo exists in the product's list of photos
            var photo = product.PictureURL.FirstOrDefault(p => p == photoUrl);
            if (photo == null)
            {
                return new Response { Message = "Photo not found in the product.", StatusCode = 404 , IsDone = false };
            }

            // Remove the photo from the product's list of photos
            product.PictureURL.Remove(photo);

            // Optionally, delete the photo file from storage
            // This assumes you have a method DeleteFileAsync to handle file deletion
            //await DeleteFileAsync(photoUrl);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return new Response { IsDone = true, Message = "Photo deleted successfully.", StatusCode = 200 };
        }

        //private async Task DeleteFileAsync(string photoUrl)
        //{
        //    try
        //    {
        //        if (File.Exists(photoUrl))
        //        {
        //            File.Delete(photoUrl);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
               
        //        Console.WriteLine($"Error deleting file: {ex.Message}");
        //        throw;
        //    }

        //    await Task.CompletedTask;
        //}


        // Update product
        public async Task<Response> UpdateProduct(string productId, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new Response  { Message= "Product not found." , StatusCode=404 , IsDone=false};
            }
            _mapper.Map(productDto, product);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            var result= _mapper.Map<ProductDto>(product);
            return new Response { StatusCode = 200, IsDone = true, Model = result };
        }


        // Delete product
        public async Task<Response> DeleteProduct(string productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new Response { StatusCode = 404, IsDone = false, Message = "Product not found." };
            }
            product.deleted = true;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return new Response { StatusCode = 200, Message = "Product deleted successfully !" , IsDone= true};
        }

        // make discount 
        public async Task<Response> MakeDiscount( DiscountDto discountDto)
        {

            if (discountDto.Persentage == null)
            {
                return new Response { StatusCode = 404, IsDone = false, Message = "Discount not found !"};
            }

            var discount = _mapper.Map<Discount>(discountDto);
            _context.discounts.Add(discount);
            await _context.SaveChangesAsync();
            return new Response { IsDone = true , StatusCode=200, Model= discount};
            
        }



        // Add discount to product
        public async Task<Response> AddDiscountToProduct(string productId, string discountId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            var discount = await _context.discounts.FindAsync(discountId);

            if (product == null)
            {
                return new Response { Message = "Product not found.", StatusCode=404,IsDone=false };
            }
            if (discount == null)
            {
                return new Response { Message = "Discount not found." , StatusCode=404,IsDone=false };
            }

            // Associate the discount with the product
            product.Discount = discount;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            // Map the updated product to a ProductDto
            var products = new Product
            {
                Id = productId,
                Name = product.Name,
                price = product.price,
                newPrice = (product.price*discount.Persentage)/100,
                Description = product.Description,
                Categoryid = product.Categoryid,
                Discount = new Discount
                {
                    Persentage = discount.Persentage
                }
            };

            var result= _mapper.Map<ProductOutputDTO>(products);
            return new Response { IsDone=false, StatusCode=200, Model= result };    
        }


        // Remove discount from product
        public async Task<Response> RemoveDiscountFromProduct(string productId)
        {
            var product = await _context.Products
                                .Include(p => p.Discount) // Ensure Discount is loaded
                                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                return new Response {Message= "Product not found." , IsDone=false , StatusCode=404};
            }

            if (product.Discount == null)
            {
                return new Response { Message = "Product has no discount to remove!" , StatusCode = 404, IsDone = false };
            }
            // Remove the discount from the product
            product.Discount = null;
            product.newPrice = 0;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
           var result = _mapper.Map<ProductOutputDTO>(product);
            return new Response { Model = result , StatusCode =200 };
        }

        // Get My Product
        public Response GetMyProducts(string userEmail)
        {
            var sellerId = _context.Users.First(x => x.Email == userEmail).Id;
            if (string.IsNullOrWhiteSpace(sellerId))
            {
               return  new Response {Message= "Invalid  seller information." , IsDone=false , StatusCode=400};
            }

            var products = _context.Products
             .Where(p => p.SellerId == sellerId && !p.deleted)
            .Select(s => new Product
            {
                Id = s.Id,
                Name = s.Name,
                price = s.price,
                Description=s.Description,
                Discount = s.Discount,
                Categoryid= s.Categoryid,
                Seller = s.Seller
            });

            var result= _mapper.Map<List<ProductDto>>(products);
           return new Response { Model = result ,StatusCode =200 , IsDone= true };
        }


        //update personal data 
        public async Task<Response> UpdatePersonalData(string userEmail, UpdateSellerDataDTO userDto)
        {
            if (userDto.Name==null||userDto.PhoneNumber==null||userDto.Location==null)
            {
                return new Response { Message = "Complete data to update !" , StatusCode=400, IsDone= false };
            }
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new Response { Message = "User not found." , StatusCode=404 , IsDone =false};
            }

            user.Name = userDto.Name;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Location = userDto.Location;
            

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Response { StatusCode = 400, IsDone = false, Message = "Failed to update user data." };
            }

           var data =  _mapper.Map<CompleteSelerData>(user);
            return new Response { Model = data ,StatusCode = 200 , IsDone= true };
        }

        public Response GetMyDiscounts()
        {
           var result =  _context.discounts.ToList();
            if (result.Count == 0)
            {
                return new Response { Message = "your discount list is impty , Add Discount to Implement "};
            }
           return new Response { Model = result ,IsDone= true , StatusCode=200};
        }

       
    }
}
