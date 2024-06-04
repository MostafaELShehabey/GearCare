using AutoMapper;
using DomainLayer.Dto;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public SellerService(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
        }

        // Add ID photo 

        public async Task<Response> AddIDphoto(IFormFile photo, string userEmail)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new InvalidOperationException("The photo is not added");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userEmail);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var photoUpload = Path.Combine(Directory.GetCurrentDirectory(), "images", "SellerIDphotos");
            if (!Directory.Exists(photoUpload))
            {
                Directory.CreateDirectory(photoUpload);
            }
            var photoUniquname = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
            var photoPath = Path.Combine(photoUpload, photoUniquname).Replace("\\", "/");
            using (var stream = new FileStream(photoPath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

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


            var result= _mapper.Map<CompleteSelerData>(user);
            return new Response { IsDone=true,Model = result, StatusCode=200};
        }


        // Add product
        public async Task<Response> AddProduct(ProductDto productDto, string userEmail)
        {

            var sellerId = _context.Users.First(x=>x.Email==userEmail).Id;
            if (string.IsNullOrWhiteSpace(productDto.Name) || productDto.Price <= 0 || string.IsNullOrWhiteSpace(productDto.Description) || string.IsNullOrWhiteSpace(sellerId))
            {
                throw new ArgumentException("Invalid product or seller information.");
            }

            var category = _context.Categories.Where(x => x.Id == productDto.CategoryId);
            if(category is null)
            {
                return new Response { IsDone = false ,Messege="category id not correct !"};
            }
            var product = new Product
            { 
                //Id = Guid.NewGuid().ToString(),
                Name = productDto.Name,
                price = productDto.Price,
                Description = productDto.Description,
                SellerId = sellerId,
                Categoryid = productDto.CategoryId,
                instock = true,
                deleted = false,
                Discount = null
            };
            // Ensure the foreign key entities exist in the database before adding the product
            if (!await _context.Users.AnyAsync(u => u.Id == sellerId))
            {
                throw new Exception("Seller not found");
            }
            if (!await _context.Categories.AnyAsync(c => c.Id == productDto.CategoryId))
            {
                throw new Exception("Category not found");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<ProductOutputDTO>(product);
            return new Response { IsDone = true, Model = result,StatusCode=200 };
        }

        //add product photo 
        public async Task<Response> AddProductPhoto(IFormFileCollection photos, string productid)
        {
            if (photos == null || photos.Count == 0)
            {
                throw new InvalidOperationException(" NO photo selected !");
            }
            var product = await _context.Products.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == productid);
            var photoUpload = Path.Combine(Directory.GetCurrentDirectory(), "images", "Prouctphotos");
            if (!Directory.Exists(photoUpload))
            {
                Directory.CreateDirectory(photoUpload);
            }
            var photoList = new List<Photo>();
            foreach (var photo in photos)
            {
                if (photo.Length > 0)
                {
                    var uniquePhotoName = Guid.NewGuid().ToString() + "_" +photo.Name;
                    var photoPath = Path.Combine(photoUpload, uniquePhotoName);
                    using (var stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                    // Save the file path to the database
                    var newphoto = new Photo
                    {
                        photoURL = photoPath, 
                        productID = productid
                    };
                    photoList.Add(newphoto);
                }
            }

            // Save all photos to the database
            await _context.photos.AddRangeAsync(photoList);
            await _context.SaveChangesAsync();

            var productDto = new AllProductDataDTO
            {
                Name = product.Name,
                OriginalPrice = product.price,
                Description = product.Description,
                SellerId = product.SellerId,
                CategoryId = product.Categoryid,
                Discount = product.Discount != null ? new DiscountDto { Persentage = product.Discount.Persentage } : null,
                PictureURL = product.Photos.Select(p => p.photoURL).ToList()
            };

            return new Response { IsDone = true, Model = productDto , StatusCode=200};
        }

        // Update product
        public async Task<Response> UpdateProduct(string productId, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
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
                return new Response { StatusCode = 200, IsDone = true, Messege = "Product not found." };
            }
            product.deleted = true;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return new Response { StatusCode = 200, Messege = "Product deleted successfully !" , IsDone= true};
        }

        // make discount 
        public async Task<Response> MakeDiscount( DiscountDto discountDto)
        {

            if (discountDto == null)
            {
                return new Response { StatusCode = 200, IsDone = true, Messege = "discount is null !"};
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
                throw new InvalidOperationException("Product not found.");
            }
            if (discount == null)
            {
                throw new InvalidOperationException("Discount not found.");
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
                throw new InvalidOperationException("Product not found.");
            }

            if (product.Discount == null)
            {
                return new Response { Messege = "Product has no discount to remove!" , StatusCode = 200 };
            }
            // Remove the discount from the product
            product.Discount = null;
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
                throw new ArgumentException("Invalid  seller information.");
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
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new Response { Messege = "User not found." , StatusCode=200 , IsDone =false};
            }

            user.Name = userDto.Name;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Location = userDto.Location;
            

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new Response { StatusCode = 200, IsDone = false, Messege = "Failed to update user data." };
            }

           var data =  _mapper.Map<CompleteSelerData>(user);
            return new Response { Model = data ,StatusCode = 200 , IsDone= true };
        }

        public Response GetMyDiscounts()
        {
               var result =  _context.discounts.ToList();
           return new Response { Model = result ,IsDone= true , StatusCode=200};
        }
    }
}
