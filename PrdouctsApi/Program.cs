using Microsoft.EntityFrameworkCore;
using PrdouctsApi.Data;
using Serilog;
using ProductInventoryManagerAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().WriteTo.File("logs/detailedLogs.txt", rollingInterval: RollingInterval.Hour,
    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error).CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddDbContext<ProductDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

// Configure services to the container.
builder.Services.AddProductIntialApiServices();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(_ => { });
app.UseAuthorization();

app.MapControllers();

app.Run();
