using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Sprout.Exam.Business.Factories.SalaryFactory;
using Sprout.Exam.Business.Factories.SalaryFactory.Services;
using Sprout.Exam.Business.Mapping;
using Sprout.Exam.Business.Services;
using Sprout.Exam.DataAccess;
using Sprout.Exam.DataAccess.Repositories;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sprout.Exam.WebApp.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, Array.Empty<string> ()
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            return app;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<SproutExamDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<DataSeeder>();
            services.AddTransient<BaseSalaryService, RegularSalaryService>();
            services.AddTransient<BaseSalaryService, ContractualSalaryService>();
            services.AddTransient<ISalaryServiceFactory, SalaryServiceFactory>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<IEmployeeTypesService, EmployeeTypesService>();

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new SproutExamMappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        public static IServiceCollection ConfigureAuthenticationAndAuthorization(this IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }

        public static IServiceCollection ConfigureModelValidation(this IServiceCollection services)
        {
            services.Configure((Action<ApiBehaviorOptions>)(options =>
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var response = new ApiResponse
                    {
                        Message = string.Join('\n', actionContext.ModelState
                            .Where(x => x.Value?.Errors?.Any() ?? false)
                            .Select(x => x.Value.Errors[0].ErrorMessage))
                    };

                    return new BadRequestObjectResult(response);
                }));

            return services;
        }

        public static IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            dataSeeder.Seed();

            return app;
        }
    }
}
