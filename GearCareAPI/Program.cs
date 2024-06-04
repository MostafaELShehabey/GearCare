using AutoMapper;
using DomainLayer.Helpers;
using DomainLayer.Models;
using GearCareAPI.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ServiceLayer;
using ServiceLayer.AdminServices;
using ServiceLayer.ApplicationUserServices;
using ServiceLayer.AuthServices;
using ServiceLayer.Mapping;
using ServiceLayer.SellerServices;
using ServiceLayer.Technician;
using ServiceLayer.WinchDriverService;
using ServiceLayer.WinchDriverServise;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>(options =>{ options.UseSqlite("Data Source=GearCare1.db"); });
builder.Services.AddDbContext<ApplicationDbContext>(optient => optient.UseSqlServer(builder.Configuration.GetConnectionString("MyConn")));

builder.Services.AddCors();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager<SignInManager<ApplicationUser>>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<ITechnicianService, TechnicianServices>();
builder.Services.AddScoped<IApplicationUserServices,ApplicationUserServices>();
builder.Services.AddScoped<IWinchDriverService,WinchDriverServise>();

builder.Services.AddScoped<ISellerServices, SellerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMapper, Mapper>();

builder.Services.AddCustomJwtAuth(builder.Configuration);
builder.Services.AddSwaggerGenJwtAuth();
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//app.UseExceptionHandler(appBuilder =>
//{
//    appBuilder.Run(async context =>
//    {
//        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//        context.Response.ContentType = "application/json";
//        var error = context.Features.Get<IExceptionHandlerFeature>();
//        if (error != null)
//        {
//            var ex = error.Error;

//            var result = JsonConvert.SerializeObject(new { message = ex.Message });
//            await context.Response.WriteAsync(result);
//        }
//    });
//});

app.MapControllers();

app.Run();

