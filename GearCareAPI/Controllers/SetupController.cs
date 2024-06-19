using AutoMapper;
using DomainLayer.Dto;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;

namespace GearCareAPI.Controllers
{
    [Route("api/[controller]")] // api/setup
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        protected readonly ILogger<SetupController> _logger;
        private readonly IMapper _mapper;
        public SetupController(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<SetupController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        //get all user 
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            // var users = await _userManager.Users.ToListAsync();
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }


        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                //create the roles and seed them to the database: Question 1
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return Ok(new { result = $"Role {roleName} added successfully" });
                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return BadRequest(new { error = $"Issue adding the new {roleName} role" });
                }
            }

            return BadRequest(new { error = "Role already exist" });
        }

        // Get all user
       
        [HttpPost("AddCategory")]

        public async Task<IActionResult> AddCategories(CategoryDto categoryDto)
        {
            var categoryExist = await _context.Categories.FirstOrDefaultAsync(x => x.Name == categoryDto.Name);
            if (categoryExist != null)
            {
                throw new InvalidOperationException("This category already exists!");
            }
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        // Add User to role
        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} added to the {roleName} role");
                    return Ok(new { result = $"User {user.Email} added to the {roleName} role" });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to add user {user.Email} to the {roleName} role");
                    return BadRequest(new { error = $"Error: Unable to add user {user.Email} to the {roleName} role" });
                }
            }

            // User doesn't exist
            return BadRequest(new { error = "Unable to find user" });
        }

        // Get specific user role
        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            // Resolve the user via their email
            var user = await _userManager.FindByEmailAsync(email);
            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        // Remove User to role
        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} removed from the {roleName} role");
                    return Ok(new { result = $"User {user.Email} removed from the {roleName} role" });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to removed user {user.Email} from the {roleName} role");
                    return BadRequest(new { error = $"Error: Unable to removed user {user.Email} from the {roleName} role" });
                }
            }

            // User doesn't exist
            return BadRequest(new { error = "Unable to find user" });
        }
    }
}
