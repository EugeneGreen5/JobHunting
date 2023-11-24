using AppMiddlewarExample.Middleware;
using JobHunting.Controllers;
using JobHunting.Data;
using JobHunting.Helpers;
using JobHunting.Middleware;
using JobHunting.Repositories;
using JobHunting.Services;
using JobHunting.Services.Password;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Reflection;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddLogging(log =>
        {
            log.ClearProviders();
            log.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);// удаляет все зарегистрированные провайдеры логирования
        }) ;

        LogManager.LoadConfiguration("NLog.config");
        builder.Host.UseNLog();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "JobHunting API",
                Description = "API"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = JWTOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = JWTOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = JWTOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                });

        builder.Services.AddAuthorization(opts =>
        {
/*            opts.AddPolicy("AdminUser", options =>
            {
                options.RequireRole("")
            });*/
        });

        builder.Services.AddControllers();
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddScoped<PersonController>();
        builder.Services.AddScoped<IPersonService, PersonService>();
        builder.Services.AddScoped<IResumeService, ResumeService>();
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();
        builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
        builder.Services.AddScoped<ICreditHistoryRepository, CreditHistoryRepository>();

        builder.Services.AddTransient<IPassword, SHA256EncryptionPassword>();
        builder.Services.AddTransient<IPassword, SHA512EncryptionPassword>();
        builder.Services.AddTransient<IPassword, MD5EncryptionPassword>();


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;

            });
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;

        });

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseMiddleware<LoggerMiddleware>();
        app.UseMiddleware<JsonValidationMiddleware>();
        app.Run();
        
    }

}