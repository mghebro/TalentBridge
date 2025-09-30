using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using TalentBridge.Modules.Auth;
using TalentBridge.Enums.Auth;
using TalentBridge.Data;
using System.Text;

namespace TalentBridge.Core.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabaseServices(configuration);
        services.AddModuleServices();
        services.AddBasicServices();
        services.AddOtherServices(configuration);
        services.AddAuthServices(configuration);
        services.AddAuthorizationPolicies();
        services.AddSignalR();
        return services;
    }

    [Obsolete]
    private static IServiceCollection AddBasicServices(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            })
            .AddFluentValidation(config =>
            {
                // Automatically register all validators from this assembly
                config.RegisterValidatorsFromAssemblyContaining<Program>();
            });

        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter JWT token like: Bearer {your_token}"
            });

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddDatabaseServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    private static IServiceCollection AddModuleServices(this IServiceCollection services)
    {
        services.AddAuthModule();
        return services;
    }

    private static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ConsultantOnly", policy => policy.RequireRole(ROLES.CONSULTANT.ToString()));
            options.AddPolicy("AdminOnly", policy => policy.RequireRole(ROLES.ADMIN.ToString()));
            options.AddPolicy("HandCraftManOnly", policy => policy.RequireRole(ROLES.HANDCRAFTMAN.ToString()));
            options.AddPolicy("UserOnly", policy => policy.RequireRole(ROLES.USER.ToString()));
        });
        return services;
    }

    private static IServiceCollection AddOtherServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(Program).Assembly);

    
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(configuration.GetConnectionString("Frontend")!)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],     
                    ValidAudience = configuration["JWT:Audience"],  
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JWT:Key"])  
                    )
                };
            });

        return services;
    }

 
}
