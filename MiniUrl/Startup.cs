using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniUrl.Data;
using MiniUrl.Services;
using Unity;

namespace MiniUrl
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private readonly string MyAllowSpecificOrigins = "_keyKey";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller}/{action}");

                routes.MapRoute("SpaDetail", "app/details/{param}", defaults: new { controller = "Home", action = "Spa" });
                routes.MapRoute("SpaHome", "app/", defaults: new { controller = "Home", action = "Spa" });

            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        public void ConfigureContainer(IUnityContainer container)
        {
            // Could be used to register more types
            container.RegisterType<IConversion, Base62Conversion>();
            container.RegisterType<IUrlShortener, UrlShortener>();
            container.RegisterType<IRedisConnector, RedisConnector>();
            container.RegisterType<IStorage, RedisStorage>();
        }
    }
}
