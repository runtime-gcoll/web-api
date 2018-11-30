using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SwirlTheoryApi.Data;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config) {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the Identity service with DI and configure Identity settings
            services.AddIdentity<User, IdentityRole>(cfg => {
                cfg.User.RequireUniqueEmail = true;
            })
            // And tell it where to find the User data (in our DB context)
            .AddEntityFrameworkStores<ShoppingContext>();

            // Change the support for JWT-based authentication
            // We don't need to support cookie-based auth since we're just an API
            services.AddAuthentication()
                //.AddCookie()
                // Now we need to configure the details for our JWTs
                .AddJwtBearer(cfg => {
                    cfg.TokenValidationParameters = new TokenValidationParameters() {
                        ValidIssuer = _config["Tokens:Issuer"],
                        ValidAudience = _config["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))
                    };
                });

            // Set up the DB connection and register it with dependency injection
            // We get the connection string from our config.json file (see Program.cs for config setup)
            services.AddDbContext<ShoppingContext>(options => options.UseSqlServer(_config.GetConnectionString("SwirlConnectionString")));

            // Set up the seeding service which runs once at startup
            services.AddTransient<SwirlSeeder>();

            // Add the Interface as a service people can use, specify the SwirlRepository implementation is to be used
            services.AddScoped<ISwirlRepository, SwirlRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

            // Enable Identity-based auth
            app.UseAuthentication();

            // Disable default HTTPS redirection while we're in a development environment
            if (!env.IsDevelopment()) {
                app.UseHttpsRedirection();
            }
            
            app.UseMvc();
        }
    }
}
