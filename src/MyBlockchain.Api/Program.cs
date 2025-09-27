using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlockchain.Api.Handlers;
using MyBlockchain.Api.Validators;
using MyBlockchain.Application.AutoMappers;
using MyBlockchain.Application.Interfaces;
using MyBlockchain.Application.Models;
using MyBlockchain.Application.Services;
using MyBlockchain.Infrastructure.Data;
using MyBlockchain.Infrastructure.UnitOfWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Web;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyBlockchain.Api
{
    public class Program
    {   
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // NLog: Setup NLog for Dependency injection
            NLog.LogManager.Setup().LoadConfigurationFromAppSettings();
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {   
                logger.Debug("init main");

                // Add services to the container.
                builder.Services.AddControllers();

                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.Host.UseNLog();  // NLog: setup NLog for Dependency injection

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .EnableSensitiveDataLogging().LogTo(message => Debug.WriteLine(message), LogLevel.Information));

                builder.Services.AddHttpClient<IEthBlockService, EthBlockService>();

                // Register Unit of Work and Services (Dependency Injection)
                builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
                builder.Services.AddScoped<IProductService, ProductService>();
                builder.Services.AddScoped<IEthBlockService, EthBlockService>();
                builder.Services.AddScoped<ProductHandler>();

                // Automapper to map the DTOs and Entities
                builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

                // Read the configuration from appsettings.json
                builder.Services.AddSingleton<BlockCypherEndPoints>();
                builder.Services.Configure<BlockCypherEndPoints>(builder.Configuration.GetSection("BlockCypherEndPoints"));

                // Register Healthcheck
                builder.Services.AddHealthChecks(); // Register health checks

                // CORS policy
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("front_end_cors_policy_port_3000", policy =>
                    {
                        // localhost:3000 port is for front end application if this API needs to be consumed.
                        policy.WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });

                builder.Services.AddFluentValidationAutoValidation();
                builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();



                // Customizing the validation error response
                builder.Services.Configure<ApiBehaviorOptions>(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(e => e.Value?.Errors.Count > 0)
                            .Select(e => new
                            {
                                Field = e.Key,
                                Errors = e.Value?.Errors.Select(x => x.ErrorMessage)
                            });

                        return new BadRequestObjectResult(new { Errors = errors });
                    };
                });

                // JSON searialization options
                builder.Services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    });

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseCors("front_end_cors_policy_port_3000");

                app.UseAuthorization();

                app.MapControllers();

                // Healthcheck endpoint
                app.MapHealthChecks("/health");

                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }

        }
    }

}

public partial class Program { }
