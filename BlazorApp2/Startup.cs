using BlazorApp2.Data;
using BlazorApp2.Models;
using BlazorApp2.Services;
using BlazorApp2.Services.Authentication;
using BlazorApp2.Services.TokenValidators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace BlazorApp2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
            Configuration.Bind("Authentication", authenticationConfiguration);
            services.AddSingleton(authenticationConfiguration);

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authenticationConfiguration.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = authenticationConfiguration.Audience,
                    ValidateAudience = true,
                    ClockSkew=TimeSpan.Zero
                };
            });
            services.AddAuthorization();
            services.AddSingleton<TokenValidator>();
            services.AddSingleton<ILocalStorageService, LocalStorageService>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddSingleton<WeatherForecastService>();
            services.AddHttpClient("AuthenticationService", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44395/");
                client.DefaultRequestHeaders.Add("User-Agent", "Blazor");
            });
            //services.AddMvc()
            //    .AddJsonOptions(options => {
            //        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            //    });

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
