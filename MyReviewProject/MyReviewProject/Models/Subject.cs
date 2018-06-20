using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyReviewProject.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        public string Name { get; set; }

        public int SubCategoryId { get; set; }

        private double _averageRating;
        public double AverageRating {
            get { return _averageRating; }
            set
            {
                if (value < 1 || value > 5)
                    value = 4;
                _averageRating = value;

            }
        }


    }
}