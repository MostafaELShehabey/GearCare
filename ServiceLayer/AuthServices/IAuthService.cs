using DomainLayer.Dto;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.AuthServices
{
    public interface IAuthService
    {
        public Task<AuthModel> RegisterAsync(ApplicationUserDto appUserDto);
        public Task<ApplicationUserDto> AddPersonalphoto(IFormFile photo, string userid);
        public  Task<AuthModel> LoginAsync(LoginDto loginDto);
        public Task<AuthModel> ChangePasswordAsync(ChangepasswordDTO model);
      //public Task<Response> LogoutAsync();
        public Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
        public Task<AuthModel> GetJwtToken(LoginDto Dto);
        Task<string?> AddToRoleasync(AddToRoleModel model);
    }
}
