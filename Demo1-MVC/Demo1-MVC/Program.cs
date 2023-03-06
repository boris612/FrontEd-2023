using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services
  .AddControllersWithViews()
  .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services
  .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
  });
    

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
