using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MyReviewProject.Models;
using MyReviewProject.Services;
using System;
using System.Net;
using System.Web;
using static MyReviewProject.Startup;

namespace MyReviewProject.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {

        }
        
        public BaseController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        protected Microsoft.AspNetCore.Identity.RoleManager<ApplicationRole> _roleManager;
        protected  Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        protected  SignInManager<ApplicationUser> _signInManager;
        protected  IEmailSender _emailSender;
        protected  ILogger _logger;

        protected DateTime DefaultDatetime = new DateTime(1753, 01, 01);

        private ApplicationDbContext _db;
        public ApplicationDbContext Db
        {
            get
            {
                return _db ?? GetNewContext();
            }
            set
            {
                _db = value;
            }        
        }

        private ApplicationDbContext GetNewContext()
        {
            _db = ApplicationDbContext.Create();
            return _db;
        }       

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        protected void OnException(ExceptionContext error)
        {
            error.ExceptionHandled = true;
            int code = 0;
            if (error.Exception is HttpException)
            {
                code = ((HttpException)error.Exception).StatusCode;
            }

            switch (code)
            {
                case 400:
                case 401:
                case 404:
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    error.Result = View("~/Views/Error/NotFound.cshtml");
                    break;
                case 403:
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    error.Result = View("~/Views/Error/Forbidden.cshtml");
                    break;
                case 0:
                case 500:
                case 501:
                case 502:
                case 503:
                case 504:
                case 505:
                    ErrorModel model = new ErrorModel
                    {
                        Code = code,
                        Source = error.Exception.Source ?? error.RouteData.Values["controller"].ToString() + " " + error.RouteData.Values["action"].ToString(),
                        Message = error.Exception.Message,
                        Trace = error.Exception.StackTrace
                    };
                    var innerexception = error.Exception;
                    while (innerexception.InnerException != null)
                    {
                        model.AddStuff = innerexception.InnerException.Message + "\n   " + innerexception.InnerException.Source + " \n " + innerexception.InnerException.StackTrace.Replace(" at ", "\n >> at ") + " \n";
                        innerexception = innerexception.InnerException;
                    }

                    
                    ViewBag.Model = model;
                    error.Result = View("~/Views/Error/ServerError.cshtml");                       
                    break;
                default:
                    error.Result = View("~/Views/Error/NotFound.cshtml");
                    break;
            }
        }        
    }
}


