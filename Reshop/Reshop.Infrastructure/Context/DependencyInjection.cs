using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reshop.Application.Interfaces;
using Reshop.Application.Services.Identity;
using Reshop.Domain.Models.User.Identity;
using Reshop.Domain.Services.Interfaces;
using Reshop.Domain.Services.Interfaces.ProductAndCategory;
using Reshop.Domain.Services.Interfaces.User.Cart;
using Reshop.Infrastructure.Services.Repository;
using Reshop.Infrastructure.Services.Repository.ProductAndCategory;
using Reshop.Infrastructure.Services.Repository.User.Cart;

namespace Reshop.Infrastructure.Context
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Db Context

            services.AddDbContext<ReshopDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            #endregion

            #region Identity

            services.AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_@";
                    options.Password.RequiredLength = 1;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<ReshopDbContext>()
                .AddErrorDescriber<PersianIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            #endregion

            #region IoC

            services.AddScoped<IReshopDbContext>(provider => provider.GetService<ReshopDbContext>());

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRe, ProductRe>();
            services.AddScoped<ICartRe, CartRe>();
            services.AddScoped<ICategoryRe, CategoryRe>();

            #endregion

            services.AddAuthentication();
            services.AddMemoryCache();

            return services;
        }
    }
}
