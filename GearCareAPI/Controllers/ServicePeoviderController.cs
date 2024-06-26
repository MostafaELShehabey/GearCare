using DomainLayer.Dto;
using DomainLayer.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Technician;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace GearCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Mechanic,Electrician")]
    public class ServiceProviderController : ControllerBase
    {
        private readonly ITechnicianService _technicianService;

        public ServiceProviderController(ITechnicianService technicianService)
        {
            _technicianService = technicianService;
        }



        [HttpPost("CompletePersonalData")]
        public async Task<IActionResult> CompletePersonalData([FromForm]ServiceProvideroutDTO ?userDto, IFormFile ?IDphoto)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var providers = await _technicianService.CompletePersonalData(userEmail, userDto,IDphoto);
                return Ok(providers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.Message}");
            }
        }


        [HttpGet("GetAllRepareOrderToAccept")]
        public async Task<IActionResult> GetAllRepareOrderToAccept()
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

                var orders = await _technicianService.GetAllRepareOrderToAccept(userEmail);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("MyOrdersHistory")]
        public async Task<IActionResult> GetOrderHistory(Enums.OrderBy orderBy)
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

                var orders = await _technicianService.GetOrderHistory(userEmail, orderBy);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("CurrentOrder")]
        public async Task<IActionResult> CurrentOrder()
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

                var orders = await _technicianService.CurrentOrder(userEmail);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("HandleOrderAction")]
        public async Task<IActionResult> HandleOrderAction([FromForm] OrderActionRequest request)
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

                var response = await _technicianService.HandleOrderAction(userEmail, request.OrderId, request.Action);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("UpdatePersonaldata")]
        public async Task<IActionResult> UpdatePersonaldata([FromForm] UpdateServiceProviderDataDTO serviceProviderDto,IFormFile personalPhoto)
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

                var result = await _technicianService.UpdatePersonaldata(userEmail, serviceProviderDto, personalPhoto);
                if (result != null)
                {
                    return Ok(new { message = "Personal data updated successfully." });
                }
                else
                {
                    return BadRequest(new { message = "Failed to update personal data." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

   
}
