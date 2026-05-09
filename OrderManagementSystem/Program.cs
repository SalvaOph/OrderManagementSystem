using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Controllers;
using OrderManagementSystem.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Dependency injection for Clean Architecture
builder.Services.AddScoped<ICustomerService, OrderManagementSystem.Application.Services.CustomerService>();
builder.Services.AddScoped<ICustomerRepository, OrderManagementSystem.Infrastructure.Repositories.CustomerRepository>();
builder.Services.AddScoped<IOrderService, OrderManagementSystem.Application.Services.OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderManagementSystem.Infrastructure.Repositories.OrderRepository>();
builder.Services.AddScoped<IProductService, OrderManagementSystem.Application.Services.ProductService>();
builder.Services.AddScoped<IProductRepository, OrderManagementSystem.Infrastructure.Repositories.ProductRepository>();
builder.Services.AddScoped<IOrderDetailService, OrderManagementSystem.Application.Services.OrderDetailService>();
builder.Services.AddScoped<IOrderDetailRepository, OrderManagementSystem.Infrastructure.Repositories.OrderDetailRepository>();
builder.Services.AddScoped<IEmployeeService, OrderManagementSystem.Application.Services.EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, OrderManagementSystem.Infrastructure.Repositories.EmployeeRepository>();
builder.Services.AddScoped<IShipperService, OrderManagementSystem.Application.Services.ShipperService>();
builder.Services.AddScoped<IShipperRepository, OrderManagementSystem.Infrastructure.Repositories.ShipperRepository>();
builder.Services.AddScoped<IOrderDetailViewService, OrderManagementSystem.Application.Services.OrderDetailViewService>();
builder.Services.AddScoped<IOrderDetailViewRepository, OrderManagementSystem.Infrastructure.Repositories.OrderDetailViewRepository>();

builder.Services.AddHttpClient<IGoogleMapsService, GoogleMapsService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowAll");
app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();