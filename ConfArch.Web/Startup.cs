using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ConfArch.Data;
using ConfArch.Data.Repositories;

using Kentico.Admin;
using Microsoft.AspNetCore.Authentication;

namespace ConfArch.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(o => o.Filters.Add(new AuthorizeFilter()));
            services.AddScoped<IConferenceRepository, ConferenceRepository>();
            services.AddScoped<IProposalRepository, ProposalRepository>();
            services.AddScoped<IAttendeeRepository, AttendeeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIdentityManager, IdentityManager>();

            services.AddDbContext<ConfArchDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    assembly => assembly.MigrationsAssembly(typeof(ConfArchDbContext).Assembly.FullName)));

            services.AddKenticoAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    // Kentico hooks
                    options.Events.OnSigningIn = IdentityManager.AdministrationAutoSignIn;
                    options.Events.OnSigningOut = IdentityManager.AdministrationAutoSignOut;
                })
                .AddCookie(ExternalAuthenticationDefaults.AuthenticationScheme)
                .AddGoogle(options =>
                {
                    options.SignInScheme = ExternalAuthenticationDefaults.AuthenticationScheme;
                    options.ClientId = Configuration["Google:ClientId"];
                    options.ClientSecret = Configuration["Google:ClientSecret"];
                })
                .AddAzureAD(options => Configuration.Bind("AzureAd", options))
                .AddKenticoAdminExternalAuthentication("/Account/LoginWithAzureAD?returnUrl=/admin", "Sign-in with AzureAD");


            services.AddKenticoSpaFiles();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseKenticoRewriter();
            app.UseKenticoSpaAdmin(env);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Conference}/{action=Index}/{id?}");
            });
        }
    }
}
