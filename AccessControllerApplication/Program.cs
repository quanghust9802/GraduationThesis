﻿using Application.AuthProvide;
using Application.DTOs.AuthDTOs;
using Application.IRepositories;
using Application.Middleware;
using Application.Services;
using Application.Services.AccessRequestServices;
using Application.Services.ImageServices;
using Application.Services.NotificationService;
using Application.Validations;
using CloudinaryDotNet;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    //options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Db connection
//var connectionString = builder.Configuration.GetConnectionString("ProcessManager");
//builder.Services.AddDbContext<ProcessContext>(options => options.UseSqlServer(connectionString));
var connectionString = builder.Configuration.GetConnectionString("ProcessManager");

builder.Services.AddDbContext<AccessControllContext>(options =>
    options.UseSqlServer(connectionString, b =>
        b.MigrationsAssembly("ProcessManagement")) // Ch? ??nh assembly ch?a migrations
);


//Add Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Add JWT
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = TokenService.GetTokenValidationParameters(builder.Configuration);
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
    });


//Add Swagger authen
builder.Services.AddSwaggerGen(opt =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    opt.AddSecurityDefinition("Bearer", securitySchema);
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    });


});

//Add the CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.SetIsOriginAllowed(origin => true)
               .AllowCredentials()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Authorization", "Content-Disposition");
    });
});
// Token services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAccessRequestService, AccessRequestService>();



//User services
builder.Services.AddScoped<IUserRepository, UserRepository>();


//Add FluentValidation services
builder.Services.AddTransient<IValidator<UserDTO>, UserValidator>();

//config cloudinary
Account account = new Account(
    "dmgge2gak",
    "256172932723765",
    "HHMoBYSkQ-v8SJGcjpesMrzommE"
);

Cloudinary cloudinary = new Cloudinary(account);
builder.Services.AddSingleton(cloudinary);





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();