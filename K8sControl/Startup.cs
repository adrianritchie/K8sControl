using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;

namespace K8sControl
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider => {
                var config = KubernetesClientConfiguration.InClusterConfig();
                return new Kubernetes(config);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Kubernetes client)
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
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapGet("/pods", async context =>
                {
                    await context.Response.WriteAsJsonAsync(client.ListNamespacedPod("default"));
                });
                endpoints.MapGet("/delete/{pod}", async context =>
                {
                    var pod = context.Request.RouteValues["pod"].ToString();
                    await client.DeleteNamespacedPodAsync(pod, "default");
                    await context.Response.WriteAsync($"Deleted {pod}");
                });
            });
        }
    }
}
