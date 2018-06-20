using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace MyReviewProject.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public bool BrowserRemembered { get; set; }
        public string userLogin { get; set; }
    }
    
    public class EditUserViewModel
    {
        [Display(Name = "Новый логин")]
        public string NewLogin { get; set; }

        public string OldLogin { get; set; }
        
        [Display(Name = "Новый емэйл")]
        [EmailAddress]
        public string NewEmail { get; set; }

        public string OldEmail { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [StringLength(50, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [StringLength(50, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

   /* public class ManageUsers
    {

        public string UserName { get; set; }

        public DateTime DateCreate { get; set; }

        public int CountOfReviews { get; set; }

        public int CountOfComments { get; set; }

        public string UserRole { get; set; }
    }*/

    /*
    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
    
    public class ChangeLoginViewModel
    {
        [Required]
        [Display(Name = "Новый логин")]
        public string NewLogin { get; set; }   
        
        public string OldLogin { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
    */
}