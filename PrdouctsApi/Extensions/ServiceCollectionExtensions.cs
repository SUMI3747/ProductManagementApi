using PrdouctsApi.Helpers;
using PrdouctsApi.Models.DTOs;
using PrdouctsApi.Models.Entities;
using PrdouctsApi.ProductServices;
using ProductInventoryManagerAPI.ProductServices;

namespace ProductInventoryManagerAPI.Extensions
{
    /// <summary>
    /// Extension Method IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductIntialApiServices(this IServiceCollection services)
        {
            services.AddTransient<IProductValidationHelper, ProductValidationHelper>();
            services.AddTransient<IServiceResponseHelper, ServiceResponseHelper>();
            services.AddScoped<IStockUpdateService, StockUpdateService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddTransient<ProductRequestResponse>();
            services.AddTransient<Products>();

            return services;
        }
    }
}
