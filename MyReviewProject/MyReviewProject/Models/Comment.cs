using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyReviewProject.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public string AuthorId { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime EditTime { get; set; }

        public int ReplyToId { get; set; }

        public int Likes { get; set; }

        public int ReviewId { get; set; }

        public string UserLiked { get; set; }
    }    
}