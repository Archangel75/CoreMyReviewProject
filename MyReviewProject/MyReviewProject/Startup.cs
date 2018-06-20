using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyReviewProject.Extensions;

namespace MyReviewProject
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
            services.AddMvc(options => options.MaxModelValidationErrors = 50);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseOwin();

            app.UseAuthentication();

            app.UseHttpException();

            app.UseStaticFiles();
            
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        internal class HttpExceptionMiddleware
        {
            private readonly RequestDelegate next;

            public HttpExceptionMiddleware(RequestDelegate next)
            {
                this.next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await this.next.Invoke(context);
                }
                catch (HttpException httpException)
                {
                    context.Response.StatusCode = httpException.StatusCode;
                    var responseFeature = context.Features.Get<IHttpResponseFeature>();
                    responseFeature.ReasonPhrase = httpException.Message;
                }
            }
        }

        public class HttpException : Exception
        {
            private readonly int httpStatusCode;

            public HttpException(int httpStatusCode)
            {
                this.httpStatusCode = httpStatusCode;
            }

            public HttpException(HttpStatusCode httpStatusCode)
            {
                this.httpStatusCode = (int)httpStatusCode;
            }

            public HttpException(int httpStatusCode, string message) : base(message)
            {
                this.httpStatusCode = httpStatusCode;
            }

            public HttpException(HttpStatusCode httpStatusCode, string message) : base(message)
            {
                this.httpStatusCode = (int)httpStatusCode;
            }

            public HttpException(int httpStatusCode, string message, Exception inner) : base(message, inner)
            {
                this.httpStatusCode = httpStatusCode;
            }

            public HttpException(HttpStatusCode httpStatusCode, string message, Exception inner) : base(message, inner)
            {
                this.httpStatusCode = (int)httpStatusCode;
            }

            public int StatusCode { get { return this.httpStatusCode; } }
        }
    }
}
