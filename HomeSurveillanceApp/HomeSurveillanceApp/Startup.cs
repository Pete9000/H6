using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;
using HomeSurveillanceApp.Authentication.AuthModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading.Tasks;
using System;

namespace HomeSurveillanceApp
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

            string mySqlConnectionStr = Configuration.GetConnectionString("RemoteConnection");
            //string mySqlConnectionStr = Configuration.GetConnectionString("RPDockerConnection");
            services.AddDbContextPool<HomeSurveillanceDBContext>(o => o.UseMySql(mySqlConnectionStr, MariaDbServerVersion.AutoDetect(mySqlConnectionStr)));
            services.AddControllersWithViews();

            // Identity
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<HomeSurveillanceDBContext>()
                .AddDefaultTokenProviders();

            //Auth
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //Jwt Token  
            .AddJwtBearer(options =>
             {
                 options.SaveToken = true;
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = new TokenValidationParameters()
                 {

                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidAudience = Configuration["JwtConfig:ValidAudience"],
                     ValidIssuer = Configuration["JwtConfig:ValidIssuer"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtConfig:Secret"])),
                     RequireExpirationTime = true,
                     ValidateLifetime = true,
                 };
                 //options.Events = new JwtBearerEvents
                 //{
                 //    OnMessageReceived = context =>
                 //    {
                 //        context.Token = context.Request.Cookies["JwtToken"];
                 //        return Task.CompletedTask;
                 //    },
                 //};
             });



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

            app.UseStatusCodePages(context => {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    response.Redirect("/Login");
                }

                return Task.CompletedTask;
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "APISubFolder",
                    pattern: "api/{controller=Authenticate}/{action=Login}/{id?}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
