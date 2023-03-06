using Demo3_WebApi.Util;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services
  .AddControllers()
  .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);


var tokenSection = builder.Configuration.GetSection("TokenConfiguration");
builder.Services.Configure<TokenConfig>(tokenSection);
TokenConfig tokenConfig = tokenSection.Get<TokenConfig>();

builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(opt =>
  {
    opt.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,

      IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(tokenConfig.Secret)),
      ValidIssuer = tokenConfig.Issuer,
      ValidAudience = tokenConfig.Audience,
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
    Title = "Demo2 Web API",
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
  c.DocumentTitle = "Demo3 Web Api";
  c.SwaggerEndpoint($"../swagger/v1/swagger.json", "Demo3 WebAPI");
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