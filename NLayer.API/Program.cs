using Microsoft.EntityFrameworkCore;

using NLayer.Core;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped(IService, Service);

builder.Services.AddDbContext<AppDbContext>(x =>
{
    // Migration olustururken API katmaninda olusturmak yerine Repository katmaninda
    // olusturmasi gerekir. Bu yuzden Assembly konumunu belirtmek gerekir.
    x.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLServerConnection"),options => {
        string repositoryAssemblyName = Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name;
        options.MigrationsAssembly(repositoryAssemblyName);
    }); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
