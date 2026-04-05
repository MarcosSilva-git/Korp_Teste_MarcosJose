using Hangfire;
using Hangfire.Storage.SQLite;
using Korp.InvoiceService.Infraestructure;
using Korp.InvoiceService.Infraestructure.Http;
using Korp.Shared.Extensions;
using Korp.Shared.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();

builder.Services.AddDbContext<InvoiceDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services
    .AddApplicationHandlers()
    .AddDispatcher();

GlobalConfiguration.Configuration
    .UseSQLiteStorage("InvoiceServiceDb.db");

builder.Services.AddHangfire(configuration => configuration
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage("InvoiceServiceDb.db"));

builder.Services.AddHangfireServer();

builder.Services.AddHttpClient<InventoryServiceHttpClient>(http =>
    {
        var baseUri = builder.Configuration["InventoryService:BaseUrl"];
        http.BaseAddress = new Uri(baseUri!);
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var database = scope.ServiceProvider.GetRequiredService<InvoiceDbContext>();
    database.Database.Migrate();

    app.UseHangfireDashboard();
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();