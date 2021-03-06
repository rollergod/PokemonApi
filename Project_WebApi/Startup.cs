using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Project_WebApi.Data;
using Project_WebApi.Interfaces;
using Project_WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_WebApi
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
            services.AddTransient<Seed>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 
            services.AddScoped<IPokemonRepository, PokemonRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewerRepository, ReviewerRepository>();
            services.AddDbContext<AppDbContext>(options =>
            {   
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //services.AddTransient<Seed>();
            services.AddControllersWithViews()
                    .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project_WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var scopedFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();

            using(var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<Seed>();
                    service.SeedDataContext();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project_WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
