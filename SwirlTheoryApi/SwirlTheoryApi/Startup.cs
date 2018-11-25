using System;
using System.Collections.Generic;
using System.Linq;
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
using WebApi.Models;

namespace SwirlTheoryApi
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
            // Register DB with dependency injection
            services.AddDbContext<ShoppingContext>(options => options.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Miner\Documents\GatesheadCollegeProject\web-api\SwirlTheoryApi\SwirlTheoryDb.mdf;Integrated Security=True;Connect Timeout=30"));

            // Register Identity services with DI adds
            // Adds cookie-based auth, also adds scoped classes like UserManager, SignInManager, PasswordHashers etc...
            // NOTE: Automatically adds the validated user from a cookie to the HttpContext.User (which can be accessed in controller methods)
            services.AddIdentity<User, IdentityRole>()
                    // Adds UserStore and RoleStore from this context
                    // These are just helper classes to wrap around default DB tables
                    .AddEntityFrameworkStores<ShoppingContext>()
                    // Adds a provider that generated unique keys and hashes for things like
                    // "Forgot Password" links, phone number verification codes etc...
                    .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Change redirect URL for when a user tries to access a route they aren't authorized for
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/noauth";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Setup Identity
            app.UseAuthentication();

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
