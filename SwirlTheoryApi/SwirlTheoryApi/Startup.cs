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
using SwirlTheoryApi.Middleware;

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

            // Add role-based authorization policy
            services.AddAuthorization(options => {
                // Policy is applied to routes that only Admin users should be able to access
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", "true"));
            });

            // Configure settings for CORS
            services.AddCors();

            // Set up the DB connection and register it with dependency injection
            // We get the connection string from our config.json file (see Program.cs for config setup)
            services.AddDbContext<ShoppingContext>(options => options.UseSqlServer(_config.GetConnectionString("SwirlConnectionString")));

            // Set up the seeding service which runs once at startup
            services.AddTransient<SwirlSeeder>();

            // Add the Interface as a service people can use, specify the SwirlRepository implementation is to be used
            services.AddScoped<ISwirlRepository, SwirlRepository>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                // This prevents object pairs with self-referencing attributes from crashing the server whenever we ask for them
                // For example, the Order class has a list of OrderRows, each of which has a reference to the Order it's a part of
                // This option tells the JSON serializer not to try and go down these rabbit holes when it's trying to turn our C# data into JSON for transmission
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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

            // Enable the OPTIONS middleware we wrote to handle CORS preflight requests from browsers
            app.UseOptions();

            // Enable Identity-based auth
            app.UseAuthentication();

            // Disable default HTTPS redirection while we're in a development environment
            if (!env.IsDevelopment()) {
                app.UseHttpsRedirection();
            }

            // Allow CORS (needed since the Angular frontend is running on a separate server)
            app.UseCors();

            app.UseMvc();
        }
    }
}
