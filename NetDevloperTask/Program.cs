using NetDevloperTask.Data;
using NetDevloperTask.Repositories.interfaces;
using NetDevloperTask.Repositories;
using NetDevloperTask.Services.interfaces;
using NetDevloperTask.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure the DbContext with SQL Server (or other providers)
builder.Services.AddDbContext<BusinessCardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BusinessCardDb")));

// Add your services for dependency injection
builder.Services.AddScoped<IBusinessCardRepository, BusinessCardRepository>();
builder.Services.AddScoped<IBusinessCardService, BusinessCardService>();

// Add controllers
builder.Services.AddControllers();

// Add Swagger (optional for API documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
