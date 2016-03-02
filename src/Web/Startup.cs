using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Microsoft.AspNet.Mvc.Controllers;
using SimpleInjector.Integration.AspNet;
using Microsoft.AspNet.Mvc.ViewComponents;
using Infrastructure.Data;
using Domain;
using Infrastructure;
using Domain.Events;
using Lifecycle.AspNet;
using PhilosophicalMonkey;

namespace Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private Container container = new Container();
        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInstance<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.AddInstance<IViewComponentInvokerFactory>(new SimpleInjectorViewComponentInvokerFactory(container));
            var assembliesToInspect = GetAssemblies();
            services.AddLifecycleCommands(assembliesToInspect);
            services.AddMvc();
            services.AddCaching();
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromSeconds(3600);
            });
        }

        private static System.Reflection.Assembly[] GetAssemblies()
        {
            return Reflect.OnTypes.GetAssemblies(new Type[] { typeof(Startup) }).ToArray();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Department/Error");
            }

            container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();
            app.UseSimpleInjectorAspNetRequestScoping(container);

            InitializeContainer(app);

            container.RegisterAspNetControllers(app);
            container.RegisterAspNetViewComponents(app);

            container.Verify();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseLifecycleCommands();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Department}/{action=Index}/{id?}");
            });
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application services.
            container.Register<IEventStore>(() => new InMemoryEventStore(), Lifestyle.Singleton);
            container.Register<IDepartmentQueryRepository, InMemoryCachedDepartmentRepository>(Lifestyle.Scoped);
            //container.Register<ApplicationQueryService>(Lifestyle.Scoped);
            container.Register<IApplicationCommandService, ApplicationCommandService>(Lifestyle.Scoped);

            // Cross-wire ASP.NET services
            container.CrossWire<ILoggerFactory>(app);
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
