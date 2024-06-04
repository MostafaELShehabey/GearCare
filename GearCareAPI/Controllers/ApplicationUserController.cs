using DomainLayer.Dto;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.ApplicationUserServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static DomainLayer.Helpers.Enums;

namespace GearCareAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Client")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration configuration;

        private readonly IApplicationUserServices _applicationUserService;
        public ApplicationUserController(IApplicationUserServices applicationUserService, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            this.configuration = configuration;
        }


        [HttpPost("CompletePersonalData")]
        public async Task<IActionResult>  CompletePersonalData(AddCarTypeToUser userDto)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var providers = await _applicationUserService.CompletePersonalData(userEmail, userDto);
                return Ok(providers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }



        [HttpGet("GetServiceProviderAvailable")]
        public async Task<IActionResult> GetServiceProviderAvailable(UserType userType, string location, string carType)
        {
            try
            {
                var providers = await _applicationUserService.GetServiceProviderAvailable(userType, location, carType);
                return Ok(providers);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"internal server error  {ex.Message}");
            }
        }

        [HttpGet("GetSellers")]
        public async Task<IActionResult> GetSellers(string? location)
        {
            try
            {
                var sellers = await _applicationUserService.GetSellers(location);
                return Ok(sellers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }

        [HttpPost("CreateRepareOrder")]
        public async Task<IActionResult> CreateRepareOrder([FromForm]RepareOrderDto repareOrderDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var order = await _applicationUserService.CreateRepareOrder(userId ,repareOrderDto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts(string? search)
        {
            try
            {
                var products = await _applicationUserService.GetAllProducts(search);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }

        [HttpGet("GetAllProductsInShoppingCart")]
        public async Task<IActionResult> GetAllProductsInShoppingCart()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var products = await _applicationUserService.GetAllProductsInShoppingCart(userEmail);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _applicationUserService.GetAllCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"intrenal server error {ex.InnerException}");
            }
        }


        [HttpGet("FilterProductByCategory/{categoryId}")]
        public async Task<IActionResult> FilterByCategory(string categoryId)
        {
            try
            {
                var products = await _applicationUserService.FilterByCategory(categoryId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }

        [HttpPost("AddProductToShoppingCart/{productId}")]
        public async Task<IActionResult> AddProductToShoppingCart( string productId)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _applicationUserService.AddProductToShoppingCart(userEmail, productId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"internal server error {ex.InnerException}");
            }
        }

        [HttpDelete("RemoveProductFromShoppingCart/{productId}")]
        public async Task<IActionResult> RemoveProductFromShoppingCart( string productId)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _applicationUserService.RemoveProductFromShoppingCart(userEmail, productId);
                return Ok("prodeuct deleted successfly !");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }

        [HttpGet("GetBestSellingProduct")]
        public async Task<IActionResult> GetBestSellingProduct()
        {
            try
            {
                var bestSellingProduct = await _applicationUserService.GetBestSellingProduct();
                return Ok(bestSellingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }

        [HttpGet("GetBestSellers")]
        public async Task<IActionResult> GetBestSellers()
        {
            try
            {
                var bestSellers = await _applicationUserService.GetBestSellers();
                return Ok(bestSellers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }


        // the user should put and product from this seller to his shopping card 
        [HttpPost("GiveRateToSeller/{sellerId}/{rate}")]
        public async Task<IActionResult> GiveRateToSeller( string sellerId, int rate)
        {
            try
            {
               var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
               var result= await _applicationUserService.GiveRateToSeller(userId, sellerId, rate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }

        [HttpPost("UpdatePersonalData")]
        public async Task<IActionResult> UpdateUserData(UpdateApplicationUserDataDto applicationUserDto)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _applicationUserService.UpdateUserData(userEmail, applicationUserDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }
    }
}




//[HttpPost("Register")]
//public async Task<IActionResult> CreateNewAccount([FromBody] ApplicationUserDto applicationUser)
//{
//    try
//    {
//        if (applicationUser == null)
//        {
//            return BadRequest("ApplicationUser is required");
//        }
//        var newUser = await _applicationUserService.CreateNewAccount(applicationUser);

//        if (newUser == null)
//        {
//            return BadRequest("Failed to create new account");
//        }
//        return Ok(newUser);
//    }
//    catch (Exception ex)
//    {
//        return StatusCode(500, $"Internal server error: {ex.Message}");
//    }
//}

//[HttpPost("Login")]
//public async Task<IActionResult> Login(LoginDto login)
//{
//    if (ModelState.IsValid)
//    {
//        ApplicationUser? user = await _userManager.FindByNameAsync(login.username);
//        if (user != null)
//        {
//            if (await _userManager.CheckPasswordAsync(user, login.password))
//            {
//                var claims = new List<Claim>();
//                //claims.Add(new Claim("tokenNo", "75"));
//                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
//                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
//                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
//                var roles = await _userManager.GetRolesAsync(user);
//                foreach (var role in roles)
//                {
//                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
//                }
//                //signing Credentials
//                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
//                var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


//                var token = new JwtSecurityToken(
//                    claims: claims,
//                    issuer: configuration["JWT:Issuer"],
//                    audience: configuration["JWT:Audience"],
//                    expires: DateTime.Now.AddDays(30),
//                    signingCredentials: sc
//                    );
//                var _token = new
//                {
//                    token = new JwtSecurityTokenHandler().WriteToken(token),
//                    expiration = token.ValidTo

//                };
//                return Ok(_token);
//            }
//            else
//            {
//                return Unauthorized();
//            }

//        }
//        else
//        {
//            ModelState.AddModelError("", "Username is invalid");
//        }
//    }
//    return BadRequest(ModelState);
//}