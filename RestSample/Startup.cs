using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSample.App.Clients;
using RestSample.App.Clients.Interfaces;
using RestSample.App.Services;
using RestSample.App.Services.Products;
using RestSample.Core.Mocky;

namespace RestSample.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                // Return pretty JSON
                options.JsonSerializerOptions.WriteIndented = true;
            }); ;

            // Adds HTTP Client
            services.AddHttpClient();

            services.Configure<MockyOptions>(Configuration.GetSection("Mocky"));

            // Register instances of Mocky HTTP Client for generic objects
            // Registering Mocky client as Singleton as HttpClient can be reused and response can be cashed
            services.AddSingleton(typeof(IMockyHttpClient<>), typeof(MockyHttpClient<>));

            services.AddTransient<IProductService, ProductService>();

            // Add simple memory caching
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
