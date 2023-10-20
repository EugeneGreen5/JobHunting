using AppMiddlewarExample.Middleware;
using JobHunting.Data;
using JobHunting.Middleware;
using JobHunting.Repositories;
using JobHunting.Services;
using JobHunting.Services.Password;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "JobHunting API",
                Description = "Минимальное API"
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddControllers();
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddScoped<IPersonService, PersonService>();
        builder.Services.AddScoped<IResumeService, ResumeService>();
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();
        builder.Services.AddScoped<IResumeRepository, ResumeRepository>();

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

        app.MapControllers();
        app.UseMiddleware<LoggerMiddleware>();
        app.UseMiddleware<JsonValidationMiddleware>();
        app.Run();
        
    }

}