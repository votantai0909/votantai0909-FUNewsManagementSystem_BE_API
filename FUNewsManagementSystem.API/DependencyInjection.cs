using FUNewsManagementSystem.Repositories.Interfaces;
using FUNewsManagementSystem.Repositories.Repositories;
using FUNewsManagementSystem.Services.Interfaces;
using FUNewsManagementSystem.Services.Services;

namespace FUNewsManagementSystem.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISystemAccountService, SystemAccountService>();
            services.AddScoped<ITagService, TagService>();
            return services;
        }
    }
}
