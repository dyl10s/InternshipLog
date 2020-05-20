using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using InternshipJournals.Data;
using NPoco;
using System.Data.SqlClient;
using InternshipJournals.Data.Database;
using System.Net;
using System.Threading;

namespace InternshipJournals
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
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<Account>();

            services.AddSingleton(
                new Database(
                    Configuration["ConnectionString"],
                    DatabaseType.SqlServer2012,
                    SqlClientFactory.Instance
                )
            );
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            //Startup AntiSleep
            AntiSleep();
        }

        //This is used to stop the program from sleeping on azure
        public async void AntiSleep()
        {
            //The domain of the website
            while (true)
            {
                using (WebClient c = new WebClient())
                {
                    await c.DownloadStringTaskAsync(new Uri("https://internshipjournal.azurewebsites.net"));
                    await Task.Delay(120000);
                }
            }
        }
    }
}
