using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using static MyReviewProject.Controllers.ReviewController;

namespace MyReviewProject.Models
{  
    [NotMapped]
    public class CustomReviewDTO
    {
        public int ReviewId { get; set; }

        public DateTime DateCreate { get; set; }

        public string Content { get; set; }
        
        public double Rating { get; set;}

        public byte Recommend { get; set; }

        public byte Exp { get; set; }

        public string Like { get; set; }

        public string Dislike { get; set; }

        public byte[] Image { get; set; }

        //public Image Image { get; set; }

        public string Username { get; set; }

        public string Subjectname { get; set; }
    }

    public class CommentsDTO
    {
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

        public string Comment { get; set; }

        public int ReplyToId { get; set; }

        public int Likes { get; set; }

        public int ReviewId { get; set; }

        public UserDTO Author { get; set; }

        public AnswerDTO Reply { get; set; }

        public bool Liked { get; set; }
    }

    public class UserDTO
    {
        public string UserName { get; set; }

        public string Id { get; set; }
        
    }

    public class AnswerDTO
    {
        public string UserName { get; set; }

        public int ReplyToId { get; set; }
    }


    public class IndexReviewViewModel
    {        
        public IEnumerable<CustomReviewDTO> Reviews { get; set; }

    }

    public class ShowReviewViewModel
    {

        public int ReviewId { get; set; }

        public DateTime DateCreate { get; set; }

        public string Content { get; set; }

        public double Rating { get; set; }

        public byte Recommend { get; set; }

        public byte Exp { get; set; }

        public string Like { get; set; }

        public string Dislike { get; set; }

        public byte[] Image { get; set; }

        public string Username { get; set; }

        public string Subjectname { get; set; }

        public List<CommentsDTO> Comments { get; set; }
    }

    //public class CommentsReviewViewModel
    //{
    //    public List<CommentsDTO> Comments { get; set; }
    //}

    public class CreateReviewViewModel
    {
        public Subject ReviewSubject { get; set; }

        public List<Category> Categories { get; set; }

        public List<SubCategory> SubCategories { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Название должно быть не длиннее 50 символов")]
        public string Objectname { get; set; }
        
        public int SubCategoryId { get; set; }
        public int SubjectId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public bool Recomendations { get; set; }

        [Required]
        public int Experience { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пожалуйста, изложите плюсы вкратце.")]
        public string Like { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пожалуйста, изложите минусы вкратце.")]
        public string Dislike { get; set; }

        [Required]
        public string Comment { get; set; }

        [ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]
        public IFormFile Image { get; set; }
    }
}