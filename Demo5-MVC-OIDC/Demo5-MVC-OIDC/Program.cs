using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services
  .AddControllersWithViews()
  .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services
  .AddAuthentication(config =>
  {
    config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
  })
  .AddCookie(options =>
  {
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
  })
  .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
  {
    options.Authority = builder.Configuration["AuthSettings:Authority"];
    options.ClientId = builder.Configuration["AuthSettings:ClientId"];
    options.ClientSecret = builder.Configuration["AuthSettings:ClientSecret"];
    options.ResponseType = "code";
    options.Scope.Add("openid");
    options.Scope.Add("email");
    options.Scope.Add("profile");  
    options.CallbackPath = "/signin-oidc";
    options.TokenValidationParameters = new TokenValidationParameters
    {
      NameClaimType = "name"
    };
    #region Auth0 nema end_session_endpoint i standardni signout ne radi
    //alternativa je ne koristiti AddOpenIdConnect, već paket Auth0.AspNetCore.Authentication
    options.Events.OnRedirectToIdentityProviderForSignOut = ctx =>
    {      
      ctx.Response.Redirect($"{builder.Configuration["AuthSettings:Authority"]}/v2/logout?client_id={builder.Configuration["AuthSettings:ClientId"]}&returnTo={ctx.Properties.RedirectUri}");
      ctx.HandleResponse();

      return Task.FromResult(0);
    };
    #endregion
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
