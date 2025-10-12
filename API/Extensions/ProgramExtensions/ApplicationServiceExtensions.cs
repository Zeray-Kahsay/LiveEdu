using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Data;
using API.Interfaces;
using API.Repositories;
using API.Repositories.CourseEnrollment;
using API.Services;
using System.Text.Json.Serialization;
using API.Repositories.CourseCart;
using Stripe;
using API.Interfaces.Accounts;
using API.Repositories.Accounts;
using API.Interfaces.Courses;
using API.Interfaces.Enrollments;
using API.Interfaces.Carts;
using API.Repositories.Carts;
using API.Interfaces.Orders;
using API.Interfaces.Payments;
using API.Repositories.Orders;
using API.Repositories.Payments;

namespace API.Extensions.ProgramExtensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Add application services here
        // strongly typed configuration for Stripe
        services.Configure<StripeSettings>(config.GetSection("StripeSettings"));
        var stripeSettings = config
                                .GetSection("StripeSettings")
                                .Get<StripeSettings>() ?? throw new InvalidOperationException("Stripe settings is missing");
        StripeConfiguration.ApiKey = stripeSettings.SecretKey;

        Console.WriteLine($"Stripe WEBHOOK Key: {stripeSettings.WebhookSecret}");


        services.AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
        services.AddSwaggerGen();

        services.AddScoped<ITokenService, Services.TokenService>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICoursesService, CourseService>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentService, PaymentService>();

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


        // strongly typed configuration for JWT token 
        services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
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

             // Ensure HTTPS when validating tokens in production and keep the validated token available
             options.RequireHttpsMetadata = true;
             options.SaveToken = true;

             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                 ValidateIssuer = !string.IsNullOrEmpty(jwtSettings.Issuer),
                 ValidIssuer = jwtSettings.Issuer,
                 ValidateAudience = !string.IsNullOrEmpty(jwtSettings.Audience),
                 ValidAudience = jwtSettings.Audience,
                 ValidateLifetime = true,
                 // Reduce default clock skew for stricter expiration checks
                 ClockSkew = TimeSpan.Zero
             };

             options.Events = new JwtBearerEvents
             {
                 OnMessageReceived = context =>
                 {
                     var accessToken = context.Request.Query["access_token"].ToString();

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

        services.AddAuthorization();



        return services;
    }
}
