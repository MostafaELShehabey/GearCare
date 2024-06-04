using AutoMapper;
using DomainLayer.Dto;
using DomainLayer.Models;
using DomainLayer.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.HttpResults;
using Castle.Core.Configuration;
using static DomainLayer.Helpers.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ServiceLayer.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthService(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManage,
            IMapper mapper,
            IOptions<JWT> jwt,
            IHttpContextAccessor httpContextAccessor  )
        {
            this._userManager = userManager;
            this._roleManager = roleManage;
            this._jwt = jwt.Value;
            this._mapper = mapper;
            this._context = context;
            this._contextAccessor = httpContextAccessor;
            this._signInManager = signInManager;

        }
        public async Task<AuthModel> RegisterAsync(ApplicationUserDto appUserDto)
        {
            try
            {

                if (!Enum.IsDefined(typeof(UserType), appUserDto.UserType))
                {
                    return new AuthModel { Message = "Invalid user type specified." };
                }

                string role = appUserDto.UserType.ToString();

                var roleExist = _context.Roles.Where(x=>x.NormalizedName==role.Normalize());
                if(roleExist== null)
                {
                    return new AuthModel { Message = "Role not Exist , add role first " };

                }

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return new AuthModel { Message = "Role corresponding to the specified user type does not exist." };
                }

                if (await _userManager.FindByNameAsync(appUserDto.Username) is not null)
                {
                    return new AuthModel { Message = "User is already registered ! " };
                }
                if (await _userManager.FindByEmailAsync(appUserDto.Email) is not null)
                {
                    return new AuthModel { Message = "Email is already register ! " };
                }

                var user = _mapper.Map<ApplicationUser>(appUserDto);
                user.UserType = appUserDto.UserType;
                user.PhotoId = "images/Personalphoto/DefaultPersonalPhoto.jpg";
                var result = await _userManager.CreateAsync(user, appUserDto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errors += $"{error.Description},";
                    }
                    return new AuthModel { Message = errors };
                }
                await _userManager.AddToRoleAsync(user, role);

                var jwtSecurityToken = await CreateJwtToken(user);
               return new AuthModel
               {
                    Email = user.Email,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    UserName = user.UserName,
                    IsAuthenticated = true,
                    Roles = new List<string> {role },
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
               };

        }
            catch (Exception ex)
            {
                return new AuthModel { Message = $"An error occurred during registration.{ex.InnerException}" };
}
        }

        public async Task<AuthModel> GetJwtToken(LoginDto Dto) 
        {
            var authmodel = new AuthModel();
            var user = await _userManager.FindByNameAsync(Dto.username);
            var pass = await _userManager.CheckPasswordAsync(user,Dto.password);
            if (user is null || !pass)
            {
                authmodel.Message = "Username Or Password is incorrect CHeck YOU Credantials ";
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);
            authmodel.UserName = user.UserName;
            authmodel.Roles=rolesList.ToList();
            authmodel.IsAuthenticated = true;
            authmodel.Email=user.Email;
            authmodel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authmodel.ExpiresOn = jwtSecurityToken.ValidTo;
            return authmodel;
        }

        public async Task<string> AddToRoleasync(AddToRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            await _roleManager.RoleExistsAsync(model.Role);
            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
            {
                return "User id or Role is not Valid !";
            }
            if(await _userManager.IsInRoleAsync(user,model.Role)) 
            {
                return "User is already assiged to this Role!";
            }
            var result = await _userManager.AddToRoleAsync(user,model.Role);
            return result.Succeeded ? String.Empty : "Something went wrong please try again ! ";

        }


        public  async Task<JwtSecurityToken>CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleclaims = new List<Claim>();
            foreach (var role in roles)
                roleclaims.Add(new Claim("roles",role));

            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            }.Union(userClaims)
            .Union(roleclaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: Claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials:signingCredentials
                );
            return jwtSecurityToken;
        }


              public  async Task<AuthModel> LoginAsync(LoginDto loginDto)
              {
                    var user = await _userManager.FindByNameAsync(loginDto.username);
                    if (user == null)
                    {
                        return new AuthModel { Message = "Invalid Username or Password!" };
                    }

                    var result = await _userManager.CheckPasswordAsync(user, loginDto.password);
                    if (!result)
                    {
                        return new AuthModel { Message = "Invalid Email or Password!" };
                    }

                    var token = await CreateJwtToken(user);
                    var roles =await _userManager.GetRolesAsync(user);
                    return new AuthModel
                    {
                        IsAuthenticated = true,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Email = user.Email,
                        UserName = user.UserName,
                        ExpiresOn = token.ValidTo,
                        Roles = roles.ToList()
                    };
              }




        public async Task<ApplicationUserDto> AddPersonalphoto(IFormFile photo, string userEmail)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new InvalidOperationException("The photo is not added");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email==userEmail);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var photoUpload = Path.Combine(Directory.GetCurrentDirectory(), "images", "Personalphoto");
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
            if (user.PhotoId != null)
            {
                // Update existing photo path
                user.PhotoId = photoPath;
            }
            else
            {
                // Create new photo entry
                user.PhotoId = photoPath;
            }

            await _context.SaveChangesAsync();


            return _mapper.Map<ApplicationUserDto>(user);
        }


        public async Task<ApplicationUser> GetCurrentUserASync()
        {
            ClaimsPrincipal userIdclaim = _contextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(userIdclaim);
        }


        public async Task<AuthModel> ChangePasswordAsync(ChangepasswordDTO changePasswordDto)
        {
            var authModel = new AuthModel();

            //// Get the current user ID from the HttpContext
            var userEmail = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userEmail))
            {
                authModel.Message = "User is not authenticated.";
                return authModel;
            }

            // var userId = GetCurrentUserASync();         
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                authModel.Message = "User not found.";
                return authModel;
            }

            // Change the user's password
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            user.Password = changePasswordDto.NewPassword;
            _context.SaveChanges();
            if (!result.Succeeded)
            {
                authModel.Message = "Password change failed. " + string.Join(", ", result.Errors.Select(e => e.Description));
                return authModel;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                authModel.Message = "the new password doesnt saves ! ";
                return authModel;
            }

            authModel.IsAuthenticated = true;
            authModel.Message = "Password changed successfully.";
            return authModel;
        }


        //public async Task<Response> LogoutAsync()
        //{
        //    var Response = new Response();
        //    var userEmail = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(userEmail))
        //    {
        //         Response.Messege= "User is not authenticated.";
        //        return Response;
        //    }
        //    var user = await _userManager.FindByEmailAsync(userEmail);
        //    if (user is null)
        //    {
        //        return new Response { Messege = "UnAuthorize user", StatusCode = 401, IsDone = false };
        //    }
        //    await _signInManager.SignOutAsync();
        //    return new Response { Messege = "user logged out Successfully" };
        //}

       
        //public async Task<ApplicationUser> RegisterWithoutTokenAsync(ApplicationUserDto user)
        //{
        //    var data = _mapper.Map<ApplicationUser>(user);
        //    data.UserType = UserType.Client;
        //    IdentityResult result = await _userManager.CreateAsync(data, user.Password);
        //    if (result.Succeeded)
        //    {
        //        return data;
        //    }
        //    else
        //    {
        //        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        //        throw new Exception($"Registration failed: {errors}");
        //    }
        //}




        //public async Task<bool> AddPersonalphoto(IFormFile photo, string userid)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userid);

        //    if (user == null)
        //    {
        //             throw new InvalidOperationException("User not found");
        //    }
        //    if (photo == null || photo.Length == 0)
        //    {
        //        throw new InvalidOperationException(" the photo not added");
        //    }

        //    var photoUpload = Path.Combine(Directory.GetCurrentDirectory(), "images", "Personalphoto");
        //    if (!Directory.Exists(photoUpload))
        //    {
        //        Directory.CreateDirectory(photoUpload);
        //    }
        //    var photoUniquname = Guid.NewGuid().ToString() + "_" + photo.Name;
        //    var photoPath = Path.Combine(photoUpload, photoUniquname);
        //    using (var stream = new FileStream(photoPath, FileMode.Create))
        //    {
        //        await photo.CopyToAsync(stream);
        //    }
        //    // save photo path to database
        //    if (user.PhotoId != null)
        //    {
        //        // Update existing photo path
        //        user.PhotoId = photoPath;
        //        _context.ApplicationUsers.Update(user);
        //    }
        //    else
        //    {
        //        // Create new photo entry
        //        var newPhoto = new Photo
        //        {
        //            userid = userid,
        //            photoURL = photoPath
        //        };
        //        await _context.photos.AddRangeAsync(newPhoto);
        //    }
        //    //   await _context.photos.AddRangeAsync(photospath);
        //    _context.SaveChanges();
        //    return true;
        //}

    }

}

