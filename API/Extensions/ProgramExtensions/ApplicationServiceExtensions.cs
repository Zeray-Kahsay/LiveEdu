
using System.Text;
using API.Data;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions.ProgramExtensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Add application services here
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
        services.AddSwaggerGen();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .WithOrigins(["http://localhost:3000", "https://localhost:3000"]);
            });
        });
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });

        // Jwt token settings strongly typed configuration
        // services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
        var jwtSettings = config
                            .GetSection("JwtSettings")
                            .Get<JwtSettings>() ?? throw new InvalidOperationException("Jwt settings is missing");

        services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
     .AddJwtBearer(options =>
     {
         var tokenKey = Encoding.UTF8.GetBytes(jwtSettings.TokenKey);
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
             ValidateIssuer = false,
             ValidateAudience = false,
             ValidateLifetime = true,
         };

         options.Events = new JwtBearerEvents
         {
             OnMessageReceived = context =>
             {
                 var accessToken = context.Request.Query["access_token"];

                 // If the request is for the SignalR hub, read the access token from the query string
                 var path = context.HttpContext.Request.Path;
                 if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/hubs/message"))
                 {
                     context.Token = accessToken;
                 }

                 return Task.CompletedTask;
             }
         };


     });



        return services;
    }
}
