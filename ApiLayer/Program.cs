using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
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
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging(logging =>
{
    logging.AddConsole(); // Logs to terminal
});

var logger = builder.Logging.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

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
            // ValidateIssuer = true,
            // ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                logger.LogError($"Authentication failed: {context.Exception.Message}");
                if (context.Exception is SecurityTokenExpiredException)
                {
                    logger.LogError("Token has expired.");
                }
                else if (context.Exception is SecurityTokenInvalidSignatureException)
                {
                    logger.LogError("Invalid token signature.");
                }
                else if (context.Exception is SecurityTokenInvalidIssuerException)
                {
                    logger.LogError("Invalid token issuer.");
                }
                else if (context.Exception is SecurityTokenInvalidAudienceException)
                {
                    logger.LogError("Invalid token audience.");
                }
                else
                {
                    logger.LogError($"Unhandled token error: {context.Exception}");
                }

                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                logger.LogWarning("Token validation challenge triggered.");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new() { Title = "Your API", Version = "v1" });

    // Add security definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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
    containerBuilder.RegisterType<EfProfilePicService>().As<IProfilePicService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<ImageRepository>().As<IProfilePicRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<JwtService>().As<IJwtService>().InstancePerLifetimeScope(); // Register JwtService
    containerBuilder.RegisterInstance(builder.Configuration).As<IConfiguration>()
        .SingleInstance(); // Add this line here
});

builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddScoped<IValidator<UserToAddDto>, UserToAddDtoValidator>();
builder.Services.AddScoped<IValidator<UserRegisterDto>, UserRegisterDtoValidator>(); // Register validator
builder.Services.AddScoped<IProfilePicService, EfProfilePicService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>(); // Add PasswordHasher


builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
    options.AddPolicy("ProdCors", (corsBuilder) =>
    {
        corsBuilder.WithOrigins("https://myProductionSite.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

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

app.UseCors("DevCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();