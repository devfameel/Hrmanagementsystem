using IdentityService.data;

using IdentityService.security;
using IdentityService.services;
using IdentityService.services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "IdentityConnection")));

builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT key is missing.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{

    //=======================================

    //Employee

    //=========================================


    options.AddPolicy("CanCreateEmployee", policy => policy.RequireClaim("Permission", "employee:create"));

    options.AddPolicy("CanEditEmployee", policy => policy.RequireClaim("Permission", "employee:edit"));

    options.AddPolicy("CanDeleteEmployee", policy => policy.RequireClaim("Permission", "employee:delete"));

    options.AddPolicy("CanViewEmployee", policy => policy.RequireClaim("Permission", "employee:read"));

    //=======================================

    //Department

    //=========================================

    options.AddPolicy("CanCreateDep", policy => policy.RequireClaim("Permission", "department:create"));

    options.AddPolicy("CanEditDep", policy => policy.RequireClaim("Permission", "department:edit"));

    options.AddPolicy("CanDeleteDep", policy => policy.RequireClaim("Permission", "department:delete"));

    options.AddPolicy("CanViewDep", policy => policy.RequireClaim("Permission", "department:read"));
    //=======================================

    //

    //=========================================




});
      


builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.UseAuthentication();   // IMPORTANT
app.UseAuthorization();    // IMPORTANT

app.MapControllers();

app.Run();