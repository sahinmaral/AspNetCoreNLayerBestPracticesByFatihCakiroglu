using FluentValidation.AspNetCore;
using NLayer.Service.Validations;
using NLayer.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());


builder.Services.AddHttpClient<ProductAPIService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseURL"]);
});
builder.Services.AddHttpClient<CategoryAPIService>(opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseURL"]);
});


var app = builder.Build();


app.UseExceptionHandler("/Home/Error");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
