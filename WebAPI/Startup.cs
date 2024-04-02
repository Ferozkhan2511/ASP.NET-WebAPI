using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using WebAPI.Models;

namespace WebAPI
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
            services.AddControllers();

            services.AddDbContext<DonationDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));


            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
            //options.WithOrigins("http://localhost:3000")
            //   options.WithOrigins("http://74.249.89.180:777")
            // options.WithOrigins(Environment.GetEnvironmentVariable("React_Front_end_Url"))

            {
                var frontEndUrl = Configuration.GetSection("React_Front_end_Url").Value;
                if (!string.IsNullOrEmpty(frontEndUrl))
                {
                    options.WithOrigins(frontEndUrl)
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                }
                else
                {
                    // Log a warning or handle the case where the environment variable is not set.
                    // For example:
                    // Console.WriteLine("Warning: React_Front_end_Url environment variable is not set.");
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
