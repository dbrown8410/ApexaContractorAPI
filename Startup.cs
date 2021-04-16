using System;
using System.Collections.Generic;
using System.Linq;
using ApexaContractorAPI.Entities;
using ApexaContractorAPI.Repository;
using ApexaContractorAPI.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApexaContractorAPI
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
            InjectProjectDependencies(services);
        }

        private void InjectProjectDependencies(IServiceCollection services)
        {
            var wantedTypes = new List<Type>() { typeof(IRepository), typeof(IService) };
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => wantedTypes.Any(t => t.IsAssignableFrom(p)) && !p.IsInterface && !p.IsAbstract);

            var servicesInfo = types.Select(t => new
            {
                ImplementationType = t,
                ServiceType = t.GetInterfaces().First(i => i.Name.Contains(t.Name))
            }).ToList();

            servicesInfo.ForEach(s => services.AddTransient(s.ServiceType, s.ImplementationType));

            services.AddDbContext<ContractorDBContext>(options =>
                options.UseInMemoryDatabase(databaseName: "ApexaDB")
            );

            services.AddCors();

            //auto mapper
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            //Angular webservice port
            app.UseCors(opts =>
                opts.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }        
    }
}
