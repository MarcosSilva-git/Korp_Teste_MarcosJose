using Korp.InventoryService.Infraestructure;
using Korp.Shared.Extensions;
using Korp.Shared.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:49896")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddProblemDetails();
builder.Services.AddLogging();

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddApplicationHandlers()
    .AddDispatcher();

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

app.UseCors("AngularAppPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();