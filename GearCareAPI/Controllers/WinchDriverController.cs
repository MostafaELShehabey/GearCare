using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DomainLayer.Dto;
using static DomainLayer.Helpers.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ServiceLayer.WinchDriverService
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "WinchDriver")]
    public class WinchDriverController : ControllerBase
    {
        private readonly IWinchDriverService _winchDriverService;

        public WinchDriverController(IWinchDriverService winchDriverService)
        {
            _winchDriverService = winchDriverService;
        }


        [HttpPost("CompleteWinchData")]
        public async Task<IActionResult> CompleteWinchData(WinchModel userDto)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var providers = await _winchDriverService.CompleteWinchData(userEmail, userDto);
                return Ok(providers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error  {ex.InnerException}");
            }
        }


        [HttpPost("AddIDphoto")]
        public async Task<IActionResult> AddIDphoto(IFormFile photo)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User ID not found or empty in token." });
                }

                var response = await _winchDriverService.AddIDphoto(photo, userEmail);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

       

        [HttpPost("AddWinchPhoto")]
        public async Task<IActionResult> AddWinchPhoto(IFormFile photo)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User ID not found or empty in token." });
                }

                var response = await _winchDriverService.AddWinchPhoto(photo, userEmail);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("AddLicencePhoto")]
        public async Task<IActionResult> AddLicencePhoto(IFormFile photo )
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User ID not found or empty in token." });
                }

                var response = await _winchDriverService.AddLicencePhoto(photo,userEmail);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

       


        // Endpoint for handling order actions (accept, refuse, cancel)
        [HttpPost("HandleOrderAction")]
        public async Task<IActionResult> HandleOrderAction([FromForm] OrderActionRequest request)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User ID not found or empty in token." });
                }

                var response = await _winchDriverService.HandleOrderAction(userEmail, request.OrderId, request.Action);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Endpoint for getting orders to accept
        [HttpGet("GetMyOrderToAccept")]
        public async Task<IActionResult> GetMyOrderToAccept()
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User ID not found or empty in token." });
                }

                var response = await _winchDriverService.GetMyOrderToAccept(userEmail);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Endpoint for getting order history
        [HttpGet("GetOrdersHistory")]
        public async Task<IActionResult> GetOrdersHistory([FromQuery] OrderBy orderBy)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User ID not found or empty in token." });
                }

                var response = await _winchDriverService.GetOrdersHistory(userEmail, orderBy);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Endpoint for updating personal data
        [HttpPut("UpdatePersonalData")]
        public async Task<IActionResult> UpdatePersonalData([FromBody] WinchDriverDto serviceProviderDto)
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(new { Message = "User ID not found or empty in token." });
                }

                var response = await _winchDriverService.UpdatePersonalData(userEmail, serviceProviderDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
