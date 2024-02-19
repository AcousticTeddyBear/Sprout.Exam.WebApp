using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sprout.Exam.Business.Factories.SalaryFactory;
using Sprout.Exam.Business.Factories.SalaryFactory.Services;
using Sprout.Exam.Business.Mapping;
using Sprout.Exam.Business.Services;
using Sprout.Exam.DataAccess;
using Sprout.Exam.DataAccess.Repositories;
using Sprout.Exam.WebApp.Data;
using Sprout.Exam.WebApp.Models;
using System;
using System.Linq;

namespace Sprout.Exam.WebApp.Extensions
{
    public static class StartupExtensions
    {
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
