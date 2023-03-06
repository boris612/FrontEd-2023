using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Demo3_WebApi.Util
{
  public static class AddBearerTokenSchemeExtension
  {
    public static void AddBearerTokenScheme(this SwaggerGenOptions opt)
    {
      var jwtSecurityScheme = new OpenApiSecurityScheme
      {
        Description = "Paste only token (without Bearer prefix)",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
          Id = JwtBearerDefaults.AuthenticationScheme,
          Type = ReferenceType.SecurityScheme
        }
      };

      opt.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

      opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          { jwtSecurityScheme, Array.Empty<string>() }
        });
    }
  }
}