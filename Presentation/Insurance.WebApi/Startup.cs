using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insurance.DTO.Model.Client;
using Insurance.DTO.Model.Policy;
using Insurance.Proxy.Contracts;
using Insurance.Proxy.Proxy;
using Insurance.Security.Authentication;
using Insurance.Security.Contracts;
using Insurance.Service.Contracts;
using Insurance.Service.Service;
using Insurance.WebApi.Filters;
using Insurance.WebApi.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Insurance.WebApi
{
    public class Startup
    {
        #region maps

        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<ClientDto, ClientViewModel>();
                config.CreateMap<ClientViewModel, ClientDto>();

                config.CreateMap<PolicyDto, PolicyViewModel>();
                config.CreateMap<PolicyViewModel, PolicyDto>();

            });
        }

        #endregion

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterMaps();

            // for production, store the secret into AWS Secrets Manager, Azure or similar
            string secretKey = Configuration["secret"];

            byte[] symmetricKey = Convert.FromBase64String(secretKey);
            SymmetricSecurityKey secret = new SymmetricSecurityKey(symmetricKey);

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateActor = false,
                ValidateLifetime = true,
                IssuerSigningKey = secret
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                //options.RequireHttpsMetadata = false;
                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = c =>
                    {
                        return Task.FromResult(0);
                    },
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "text/plain";

                        return c.Response.WriteAsync(string.Empty);
                    }

                };
            });
            services.AddAuthorization(options => { options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build(); });

            services.AddMemoryCache();

            services.AddMvc(o =>
            {
                o.Filters.Add(typeof(WebExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHttpClient();

            //IoC
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IPolicyService, PolicyService>();
            services.AddTransient<IHttpProxy<Client>, HttpProxy<Client>>();
            services.AddTransient<IHttpProxy<Policy>, HttpProxy<Policy>>();

            services.AddTransient<IAuthentication, Authentication>();

            services.AddScoped<WebExceptionFilter>();
            services.AddScoped<WebLoggerFilter>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
