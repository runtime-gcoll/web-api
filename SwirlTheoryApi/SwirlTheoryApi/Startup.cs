using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SwirlTheoryApi.Data;
using WebApi.Models;

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
            // Register the Identity service with DI
            services.AddIdentity();

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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
