using System;
using System.IO;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Kentico.Admin
{
    public static class StartUpExtensions
    {
        private const string KENTICO_POLICY = "kentico.policy";


        public static AuthenticationBuilder AddKenticoAuthentication(this IServiceCollection services, string defaultScheme, bool authenticateDefaultScheme = true)
        {
            IdentityManager.RegisterScheme(defaultScheme, authenticateDefaultScheme);

            return services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = KENTICO_POLICY;
                sharedOptions.DefaultAuthenticateScheme = KENTICO_POLICY;
                sharedOptions.DefaultChallengeScheme = KENTICO_POLICY;
            })
                .AddPolicyScheme(KENTICO_POLICY, "Kentico authentication policy", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        if (context.Items.ContainsKey("VirtualContext") || context.Request.Path.StartsWithSegments("/api/admin"))
                        {
                            return KenticoConstants.AUTHENTICATION_SCHEME;
                        }
                        else
                        {
                            return defaultScheme;
                        }
                    };
                })

                .AddCookie(KenticoConstants.AUTHENTICATION_SCHEME, options =>
                {
                    options.Cookie.Name = KenticoConstants.AUTHENTICATION_SCHEME;
                });
        }


        public static AuthenticationBuilder AddKenticoAdminExternalAuthentication(this AuthenticationBuilder builder, string endpoint, string displayName)
        {
            IdentityManager.AdminExternalAuthentication.Add(endpoint, displayName);

            return builder;
        }


        public static void AddKenticoSpaFiles(this IServiceCollection services)
        {
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "admin/reactApp/build"; // Or any other folder
            });
        }


        public static IApplicationBuilder UseKenticoRewriter(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                var path = context.Request.Path;

                if (path.StartsWithSegments("/ctx"))
                {
                    context.Items.Add("VirtualContext", true);
                    context.Request.Path = path.Value.Replace("/ctx", "");
                }

                await next();
            });
        }


        public static IApplicationBuilder UseKenticoSpaAdmin(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            return app.MapWhen(context => context.Request.Path.Value.StartsWith("/admin", StringComparison.OrdinalIgnoreCase), (adminApp) =>
            {
                adminApp.UseSpaStaticFiles();

                adminApp.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "admin/reactApp";
                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "admin", "reactApp"))
                    };

                    if (env.IsDevelopment())
                    {
                        spa.UseReactDevelopmentServer(npmScript: "start");
                    }
                });
            });
        }
    }
}
