
using ASP.Web.Data;
using ASP.Web.Models;
using ASP.Web.Web.Configuration;
using ElCamino.AspNetCore.Identity.AzureTable.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ASP.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            //if (env.IsDevelopment())
            //{
            //    builder.AddUserSecrets<Startup>();
            //}
            //builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Elcamino Azure Table Identity services.
            services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>((options) =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddAzureTableStores<ApplicationDbContext>(new Func<IdentityConfiguration>(() =>
            {
                IdentityConfiguration idconfig = new IdentityConfiguration();
                idconfig.TablePrefix = Configuration.GetSection("IdentityAzureTable:IdentityConfiguration: TablePrefix").Value;
    idconfig.StorageConnectionString = Configuration.GetSection
    ("IdentityAzureTable:IdentityConfiguration:StorageConnectionString").Value;
                idconfig.LocationMode = Configuration.GetSection("IdentityAzureTable:IdentityConfiguration: LocationMode").Value;
            return idconfig;
            }))
            .AddDefaultTokenProviders()
            .CreateAzureTablesIfNotExists<ApplicationDbContext>();
            // Add framework services.
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddOptions();
            services.Configure<ApplicationSettings>(Configuration.GetSection("Appsettings"));
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
