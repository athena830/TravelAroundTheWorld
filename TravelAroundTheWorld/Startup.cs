using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TravelAroundTheWorld
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseRouting();
            //
            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            // });
            
            app.UseDefaultFiles();
            //存取SPA網頁資源
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseMvc();

            //判斷是否是要存取網頁，而不是發送API需求
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 &&                        //該資源不存在
                    !System.IO.Path.HasExtension(context.Request.Path.Value) &&  //網址最後沒有帶副檔名
                    !context.Request.Path.Value.StartsWith("/api"))              //網址不是/api開頭
                {
                    context.Request.Path = "/index.html";                      //將網址改成/index.html
                    context.Response.StatusCode = 200;                           //並將HTTP狀態碼修改為200成功
                    await next();
                }
            });
        }
    }
}