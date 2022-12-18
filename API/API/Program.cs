using API.Data;
using API.Data.Interfaces;
using API.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
        options.UseInMemoryDatabase(databaseName: "EmployeeSimpleDb"));

//If you want to change Db Provider for SqlServer use:
//options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeServiceConnectionString"))

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Uncomment using statement when you want to seed Employees table with example data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    DbInitialDataSeed.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
