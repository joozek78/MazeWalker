using MazeWalker.Adapters.FundaApi;
using MazeWalker.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MazeWalker.AspNet
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "MazeWalker.AspNet", Version = "v1"});
            });

            services.AddFundaApi();
            AddCoreServices(services);
            services.Configure<AppConfiguration>(Configuration);
        }

        private static void AddCoreServices(IServiceCollection services)
        {
            services.AddTransient<TopPropertiesHandler>();
        }

        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MazeWalker.AspNet v1"));
                app.UseWebAssemblyDebugging();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}