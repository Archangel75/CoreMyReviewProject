using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyReviewProject.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public int SubjectId { get; set; }        

        public DateTime DateCreate { get; set; }

        public string Content { get; set; }

        private double _rating;
        public double Rating {
            get { return _rating; }
            set {
                if (value < 1 || value > 5)
                    value = 4;
                _rating = value;
                    
            }
        }

        public byte Recommend { get; set; }

        public byte Exp { get; set; }

        public string Like { get; set; }

        public string Dislike { get; set; }

        public int ImageId { get; set; }

        public byte[] Image { get; set; }
        
        public string AuthorId { get; set; }
        
    }
}