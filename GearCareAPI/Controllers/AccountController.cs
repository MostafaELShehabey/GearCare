using AutoMapper;
using DomainLayer.Dto;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.AuthServices;
using System.Security.Claims;
using static DomainLayer.Dto.Response;

namespace GearCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAuthService authService, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            this._authService = authService;
            this.configuration = configuration;
            this._userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser([FromForm] ApplicationUserRegisterDTO applicationUserDto , IFormFile ?photo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authService.RegisterAsync(applicationUserDto , photo);

                if (!result.IsAuthenticated)
                {
                    return BadRequest(new Response
                    {
                        Messege = result.Message,
                        IsDone = false,
                        StatusCode = 200
                    });
                }
                return Ok(result);
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.InnerException}");
            }
        }

       

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.LoginAsync(login);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }


        [HttpPost("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePasswordAsync(ChangepasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var Response = await _authService.ChangePasswordAsync(model);
                if (Response.IsAuthenticated == true)
                {
                    return StatusCode(StatusCodes.Status200OK, Response.Message);
                }
                return StatusCode(StatusCodes.Status400BadRequest, Response.Message);
            }
            return BadRequest("Failed Process To Change Password");

        }

    }
}
