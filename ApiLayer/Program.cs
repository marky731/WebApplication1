using Autofac;
using Autofac.Extensions.DependencyInjection;
using Intermediary.Interfaces;
using Intermediary.Mappers;
using Intermediary.Services;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using DataAccess.DbContext;
using EntityLayer.Dtos;
using FluentValidation;
using Intermediary.Validators;
using EntityLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using ApiLayer.Filters;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt");

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Your API", Version = "v1" });

    // Add security definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Register services with Autofac
    containerBuilder.RegisterType<EfUserService>().As<IUserService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<EfRoleService>().As<IRoleService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<EfAddressService>().As<IAddressService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<AddressRepository>().As<IAddressRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<EfImageService>().As<IProfilePicService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<ImageRepository>().As<IProfilePicRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<JwtService>().As<IJwtService>().InstancePerLifetimeScope(); 
    containerBuilder.RegisterInstance(builder.Configuration).As<IConfiguration>().SingleInstance();
});

builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddScoped<IValidator<UserToAddDto>, UserToAddDtoValidator>();
builder.Services.AddScoped<IValidator<UserRegisterDto>, UserRegisterDtoValidator>(); 

builder.Services.AddSingleton<IAuthorizationHandler, UserIdAuthorizationHandler>(); 

builder.Services.AddScoped<IProfilePicService, EfImageService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>(); 


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserIdPolicy", policy =>
        policy.Requirements.Add(new UserIdRequirement()));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();
// var dbContext = app.Services.GetRequiredService<AppDbContext>();
// dbContext.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();