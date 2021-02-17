
using FurkanKARAKASChatApp.Service.Services;
using FurkanKARAKASChatApp.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;  

namespace FurkanKARAKASChatApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
         
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddMvc();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddSession(); 
            services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:6379");
            services.AddSignalR();

            //string mongoConnectionString = this.Configuration.GetConnectionString("MongoConnectionString");
            //services.AddTransient(s => new ChatRoomRepository(mongoConnectionString, "ChatApp", "ChatRoomLog"));

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
                app.UseExceptionHandler("/Home/Error"); 
            } 

            app.UseStaticFiles(); 
            app.UseRouting(); 
            app.UseAuthorization(); 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHttpsRedirection(); 
            app.UseCookiePolicy(); 

        }
    }
}
