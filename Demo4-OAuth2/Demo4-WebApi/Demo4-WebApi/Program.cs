using Demo4_WebApi.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services
  .AddControllers()
  .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);


builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(opt =>
  {
    opt.Authority = builder.Configuration["AuthSettings:Authority"];
    opt.Audience = builder.Configuration["AuthSettings:Audience"];
    opt.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
    };
    opt.Events = new JwtBearerEvents
    {
      OnAuthenticationFailed = context =>
      {
        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
        {
          context.Response.Headers.Add("Token-Expired", "true");
        }
        return Task.CompletedTask;
      }
    };
  });

builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Demo4 Web API",
    Version = "v1"
  });
  var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
  c.IncludeXmlComments(xmlPath);

  c.AddBearerTokenScheme();
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
  c.DocumentTitle = "Demo4 Web Api";
  c.SwaggerEndpoint($"../swagger/v1/swagger.json", "Demo4 WebAPI");
});

app.UseCors(builder =>
{
  builder.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader()
         .WithExposedHeaders("Token-Expired");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();