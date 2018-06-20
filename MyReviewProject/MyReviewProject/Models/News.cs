using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyReviewProject.Models
{
    public class News
    {
        public int NewsId { get; set; }

        public int CategoryId { get; set; }

        public string Content { get; set; }

        
    }
}