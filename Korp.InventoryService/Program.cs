using Korp.InventoryService.Features.Product.AddProduct;
using Korp.InventoryService.Features.Product.ReserveProducts;
using Korp.InventoryService.Features.Product.DeleteProduct;
using Korp.InventoryService.Infraestructure;
using Korp.Shared.Middlewares;
using Microsoft.EntityFrameworkCore;
using Korp.InventoryService.Features.Product.GetProducts;
using Korp.InventoryService.Features.Product.UpdateProduct;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddLogging();

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<GetProductsHandler>();
builder.Services.AddScoped<AddProductHandler>();
builder.Services.AddScoped<UpdateProductHandler>();
builder.Services.AddScoped<DeleteProductHandler>();
builder.Services.AddScoped<ReserveProductsHandler>();
builder.Services.AddScoped<RollbackReserveProductsHandler>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var database = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    database.Database.Migrate();

    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();