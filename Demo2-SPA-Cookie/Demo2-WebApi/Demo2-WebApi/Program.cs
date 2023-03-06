using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services
  .AddControllers()
  .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services
  .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
    options.Events.OnRedirectToLogin = context =>
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      return Task.CompletedTask;
    };
  });

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Demo2 Web API",
    Version = "v1"
  });
  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
  c.IncludeXmlComments(xmlPath);
});




var app = builder.Build();
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.RoutePrefix = "docs";
  c.DocumentTitle = "Demo2 Web Api";
  c.SwaggerEndpoint($"../swagger/v1/swagger.json", "Demo2 WebAPI");
});

app.UseCors(builder =>
{
  builder.WithOrigins("http://localhost:9000").AllowCredentials()
         .AllowAnyMethod()
         .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();