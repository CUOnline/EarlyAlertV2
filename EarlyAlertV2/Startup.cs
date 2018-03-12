using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EarlyAlertV2.Models;
using EarlyAlertV2.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using RSS.Providers.Canvas;
using EarlyAlertV2.ViewModels;
using EarlyAlertV2.Repository;
using EarlyAlertV2.Repository.SeedData;
using RSS.Clients.Canvas;
using EarlyAlertV2.BLL;
using EarlyAlertV2.Interfaces.BLL;
using EarlyAlertV2.Interfaces.Repository;

namespace EarlyAlertV2
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
            CanvasOAuth canvasOAuth = new CanvasOAuth();
            Configuration.GetSection(nameof(CanvasOAuth)).Bind(canvasOAuth);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/signout";
            }).AddCanvas(options =>
            {
                options.UserInformationEndpoint = canvasOAuth.ProfileEndpoint;
                options.AuthorizationEndpoint = canvasOAuth.AuthorizationEndpoint;
                options.TokenEndpoint = canvasOAuth.TokenEndpoint;
                options.ClientId = canvasOAuth.ClientId;
                options.ClientSecret = canvasOAuth.ClientSecret;
            });

            services.AddDbContext<EarlyAlertV2Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<EarlyAlertV2Context>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            // BLLs
            services.AddTransient<IReportBLL, ReportBLL>();

            // Repositories
            services.AddTransient<IReportRepository, ReportRepository>();

            CanvasApiAuth canvasApiAuth = new CanvasApiAuth();
            Configuration.GetSection(nameof(CanvasApiAuth)).Bind(canvasApiAuth);

            services.AddTransient<ICanvasClient>(s => new CanvasClient(new Uri(canvasApiAuth.BaseUrl), canvasApiAuth.ApiKey));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            AdminAccount adminAccount = new AdminAccount();
            Configuration.GetSection(nameof(AdminAccount)).Bind(adminAccount);
            Seeder.SeedIt(app.ApplicationServices, adminAccount.UserName, adminAccount.Password);
        }
    }
}
