using AutoMapper;
using DomainLayer.Dto;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.ApplicationUserServices;
using ServiceLayer.SellerServices;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace GearCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Seller")]
    public class SellerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration configuration;

        private readonly ISellerServices _sellerService;
        public SellerController(ISellerServices sellerServices, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _sellerService = sellerServices;
            _userManager = userManager;
            this.configuration = configuration;
        }

       

        [HttpPost("AddIDpicture")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddIDpicture(IFormFile photo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _sellerService.AddIDphoto(photo, userEmail);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


       

        // Add product
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto? ProductDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { Message = "User ID not found in token." });
                }

                var userEmail = userIdClaim.Value;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User ID is empty." });
                }

                var Product = await _sellerService.AddProduct(ProductDto, userEmail);
                return Ok(Product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" internal server error {ex.Message}");
            }
        }

        // Update product
        [HttpPut("UpdateProduct/{productId}")]
        public async Task<IActionResult> UpdateProduct(string productId, [FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is null.");
            }

            var result = await _sellerService.UpdateProduct(productId, productDto);
            return Ok(result);
        }

        // Delete product
        [HttpDelete("DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            await _sellerService.DeleteProduct(productId);
            return Ok();
        }


        [HttpPost("MakeDiscount")]
        public async Task<IActionResult> MakeDiscount(DiscountDto DiscountDto)
        {
            try
            {
                var Discount = await _sellerService.MakeDiscount(DiscountDto);
                return Ok(Discount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException);
            }
        }

        [HttpGet("GetMyDiscounts")]
        public async Task<IActionResult> GetMyDiscounts()
        {
            var result = _sellerService.GetMyDiscounts();
            return Ok(result);
        }

        // Add discount to product
        [HttpPost("AddDiscountToProduct/{productId}/{discountId}")]
        public async Task<IActionResult> AddDiscountToProduct(string productId, string discountId)
        {
            if (discountId == null)
            {
                return BadRequest("Discount data is null.");
            }

            var result = await _sellerService.AddDiscountToProduct(productId, discountId);
            return Ok(result);
        }

        // Remove discount from product
        [HttpPost("RemoveDiscountFromProduct/{productId}")]
        public async Task<IActionResult> RemoveDiscountFromProduct(string productId)
        {
            var result = await _sellerService.RemoveDiscountFromProduct(productId);
            return Ok(result);
        }

        // Get my products
        [HttpGet("GetMyProducts")]
        public async Task<IActionResult> GetMyProducts()
        {
            var userId = _userManager.GetUserId(User);
            var result = _sellerService.GetMyProducts(userId);
            return Ok(result);
        }

        [HttpPut("UpdatePersonalData")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateSellerDataDTO user)
        {
            try
            {

                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _sellerService.UpdatePersonalData(userEmail, user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $" internal server error {ex.Message}");
            }

        }

    }
}
