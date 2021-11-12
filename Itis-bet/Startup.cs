using BLL;
using DAL;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Claims;
using BLL.MongoSettings;
using BLL.Services;
using Microsoft.Extensions.Options;

namespace Itis_bet
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.ConfigureDataAccess(Configuration);
            services.ConfigureBusinessLogic(Configuration);
            services.ConfigureInfrastructure(Configuration);

            services.Configure<MongoDatabaseSettings>(
                Configuration.GetSection(nameof(MongoDatabaseSettings)));

            services.AddSingleton<IMongoDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDatabaseSettings>>().Value);

            services.AddSingleton<MongoService>();

            services.AddControllersWithViews();

            services.AddAuthorization(opts => {
                opts.AddPolicy("HasAccessToAdminPanel", policy => {
                    policy.RequireClaim(ClaimTypes.Role, "Admin", "Editor");
                });
                opts.AddPolicy("Editor", policy => {
                    policy.RequireClaim(ClaimTypes.Role, "Editor");
                });
                opts.AddPolicy("Admin", policy => {
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                });
            });
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // enables immediate logout, after updating the user stat.
                options.ValidationInterval = TimeSpan.Zero;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/RegLog/Register");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/AccessDenied");
                });

            services.AddSession();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            app.UseSession();
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureChat(provider);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
