//using DomainLayer.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using ServiceLayer.AdminServices;
//using ServiceLayer.AuthServices;

//namespace GearCareAPI.Controllers
//{
//    // [Authorize]
//    [ApiController]
//    [Route("api/[controller]")]
//   // [Authorize(Roles = "Admin")]
//    public class AdminController : ControllerBase
//    {
//        private readonly IAuthService _authService;

//        public AdminController(IAuthService authService)
//        {
//            _authService = authService;
//        }
//        //[HttpGet("GetNumberOfUser")]
//        //public IActionResult NumOfUser()
//        //{
//        //    var result = _adminService.NumOfUser();
//        //    return Ok(result);
//        //}

//        //[HttpGet("NumOfEachServiceProvider")]
//        //public IActionResult NumOfEachServiceProvider(UserType serviceProviderType)
//        //{
//        //    var result = _adminService.NumOfEachServiceProvider(serviceProviderType);
//        //    return Ok(result);
//        //}
          

//            [HttpPost("AddRole")]
//            public async Task<IActionResult> AddRoleAsync([FromBody] AddToRoleModel model)
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }
//                var result = await _authService.AddToRoleasync(model);
//                if (!string.IsNullOrEmpty(result))
//                {
//                    return BadRequest(result);
//                }

//                return Ok(model);
//            }

//            //[HttpGet("GetNumberOfWinchDrivers")]
//            //public IActionResult NumOfWinchDrivers()
//            //{
//            //    var result = _adminService.NumOfWinchrivers();
//            //    return Ok(result);
//            //}

//            //[HttpGet("GetNumberOfSellers")]
//            //public IActionResult NumOfSellers()
//            //{
//            //    var result = _adminService.NumOfSellers(ServiceProviderType.Seller);
//            //    return Ok(result);
//            //}

//            //[HttpGet("GetNumberOfRepareOrders")]
//            //public IActionResult NumOfAllRepareOrders([FromQuery] Status status)
//            //{
//            //    var result = _adminService.NumOfAllRepareOrders(status);
//            //    return Ok(result);
//            //}

//            //[HttpGet("GetNumberOfWinchOrders")]
//            //public IActionResult NumOfAllWinchOrders([FromQuery] Status status)
//            //{
//            //    var result = _adminService.NumOfAllWinchOrders(status);
//            //    return Ok(result);
//            //}

//            //[HttpGet("GetNumberOfBuyingTransaction")]
//            //public IActionResult NumOfAllBuyingTransaction()
//            //{
//            //    var result = _adminService.NumOfAllBuyingTransaction();
//            //    return Ok(result);
//            //}

//            //[HttpGet("GetTopFiveServiceProvider")]
//            //public IActionResult GetTopFiveServiceProvider([FromQuery] UserType serviceProviderType)
//            //{
//            //    var topFive = _adminService.GetTopFiveServiceProvider(serviceProviderType);
//            //    return Ok(topFive);
//            //}

//            //[HttpGet("GetServiceProviders")]
//            //public async Task<IActionResult> GetServiceProvidersAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] UserType serviceProviderType = UserType.Mechanic, [FromQuery] string search = "")
//            //{
//            //    var serviceProvider = await _adminService.GetServiceProvidersAsync(page, pageSize, serviceProviderType, search);
//            //    return Ok(serviceProvider);
//            //}

//            //[HttpGet("GetAllRepairOrders")]
//            //public async Task<IActionResult> GetAllRepairOrdersAsync([FromQuery] Status statusType, [FromQuery] string search, [FromQuery] int page, [FromQuery] int pageSize)
//            //{
//            //    var repairOrders = await _adminService.GetAllRepairOrdersAsync(statusType, search, page, pageSize);
//            //    return Ok(repairOrders);
//            //}

//            //[HttpPut("BanServiceProvider/{id}")]
//            //public async Task<IActionResult> BanServiceProviderByIdAsync(string id)
//            //{
//            //    var result = await _adminService.BanServiceProviderByIdAsync(id);
//            //    return Ok(result);
//            //}
//        }
//    }
