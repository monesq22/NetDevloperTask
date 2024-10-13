using NetDevloperTask.Data;
using NetDevloperTask.Repositories.interfaces;
using NetDevloperTask.Repositories;
using NetDevloperTask.Services.interfaces;
using NetDevloperTask.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure the DbContext with SQL Server
builder.Services.AddDbContext<BusinessCardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BusinessCardDb")));

// Add your services for dependency injection
builder.Services.AddScoped<IBusinessCardRepository, BusinessCardRepository>();
builder.Services.AddScoped<IBusinessCardService, BusinessCardService>();

// Configure CORS to allow Angular app from localhost:4200
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // Angular frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add controllers
builder.Services.AddControllers();

// Add Swagger (optional for API documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable CORS middleware
app.UseCors("AllowAngularClient");

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
