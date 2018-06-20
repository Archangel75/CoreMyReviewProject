using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyReviewProject.Models
{
    public class ErrorModel
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string Trace { get; set; }

        public string AddStuff { get; set; }
        
    }
}