using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Intermediary.Interfaces;
using Intermediary.Mappers;
using Intermediary.Services;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using DataAccess.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(UserProfile));

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register CORS policies
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

// Create Autofac container builder
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Register services with Autofac
    containerBuilder.RegisterType<EfUserService>().As<IUserService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
    // containerBuilder.Register(context => new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>())))
    //     .As<IMapper>()
    //     .SingleInstance();
});

var app = builder.Build();
// var dbContext = app.Services.GetRequiredService<AppDbContext>();
// dbContext.Database.Migrate();

// Configure the HTTP request pipeline.
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("DevCors");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();