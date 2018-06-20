using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyReviewProject.Startup;

namespace MyReviewProject.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpException(this IApplicationBuilder application)
        {
            return application.UseMiddleware<HttpExceptionMiddleware>();
        }
    }
}
