using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestCase.DistanceCalculation.Consumer.Consumer;
using TestCase.DistanceCalculation.Consumer.Settings;

namespace TestCase.DistanceCalculation.Consumer
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
            //Configurations
            AmqpSettings amqpSettings = Configuration.GetSection("AmqpSettings").Get<AmqpSettings>();
            services.AddSingleton(amqpSettings);

            var bus = RabbitHutch.CreateBus($"host={amqpSettings.Host};virtualHost={amqpSettings.VirtualHost};username={amqpSettings.Username};password={amqpSettings.Password};prefetchcount=1");
            services.AddSingleton(bus);

            services.AddScoped<QueueConsumer>();

            ApplicationSettings applicationSettings = Configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();
            services.AddSingleton(applicationSettings);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Working!!!");
                });
            });


            using (app.ApplicationServices.CreateScope())
            {
                app.ApplicationServices.GetService<QueueConsumer>().Consume().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}
