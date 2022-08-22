using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ParkyAPI
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
            services.AddDbContext<ApplicationDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //add NationalParkRepository so that we can access it from anywhere
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();
            //add automappers class
            services.AddAutoMapper(typeof(NationalPark));
            //version control
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

             services.AddVersionedApiExplorer(options=>options.GroupNameFormat="'v'VVV");
            //add swagger gen
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();


        /*  services
                .AddSwaggerGen(options =>
                  {
                      options.SwaggerDoc("ParkingOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
                      {
                          Title = "Parky API",
                          Version = "1",
                          Description = "Udemy Parky API",
                          Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                          {
                              Email = "nimmi24.1990@gmail.com",
                              Name = "Nermeen Nasim",
                              Url = new Uri("https://www.linkedin.com/in/n-nasim/")


                          },
                          License = new Microsoft.OpenApi.Models.OpenApiLicense()
                          {
                              Name = "MIT Liscence",
                              Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")


                          }

                      });

                     





                  });

            */
            services.AddControllers();
               }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IApiVersionDescriptionProvider provider)
        {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseHttpsRedirection();
            //add swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                        desc.GroupName.ToUpperInvariant());
                    options.RoutePrefix = "";

                  

                }

                //after for eACH LOOP
               
            });





            /*   app.UseSwaggerUI(options =>
               {
                   options.SwaggerEndpoint("/swagger/ParkingOpenAPISpec/swagger.json", "Parky API");

                   options.RoutePrefix = "";

               });*/

            app.UseRouting();
            app.UseSwagger();

            //change swaggerend points
         //   app.UseSwaggerUI
           

            app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }
    }

