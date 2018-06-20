using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Web;

namespace MyReviewProject.Models
{
    public class ValidateFileAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                var file = value as IFormFile;
                if (file == null)
                {
                    return true;
                }

                if (file.Length > 5 * 1024 * 1024)
                {
                    return false;
                }

                try
                {
                    if (file.ContentType.Equals("Png") || file.ContentType.Equals("Jpeg") || file.ContentType.Equals("MemoryBmp"))
                        return true;

                }
                catch { }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}