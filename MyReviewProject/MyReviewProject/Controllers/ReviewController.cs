using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyReviewProject.Controllers;
using MyReviewProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

namespace MyReviewProject.Controllers
{
    public class ReviewController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Review(int Id)
        {
            if (Id != null && Id != 0)
            {
                var query = from r in Db.Reviews
                            where r.ReviewId == Id
                            join u in Db.Users on r.AuthorId equals u.Id into lj
                            from u in lj.DefaultIfEmpty()
                            join s in Db.Subjects on r.SubjectId equals s.SubjectId
                            select new ShowReviewViewModel
                            {
                                ReviewId = r.ReviewId,
                                Content = r.Content,
                                Dislike = r.Dislike,
                                Exp = r.Exp,
                                Like = r.Like,
                                Rating = r.Rating,
                                Image = r.Image,
                                Recommend = r.Recommend,
                                Username = u.UserName,
                                Subjectname = s.Name
                            };
                var review = await query.FirstAsync();
                review.Comments = await GetComments(Id);
                return View(review);
            }
            return RedirectToAction("Index", "Home");

        }

        public async Task<List<CommentsDTO>> GetComments(int ReviewId)
        {
            if (ReviewId > -1)
            {
                ApplicationUser user = null;
                var userId = "";
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    userId = user.Id;
                }

                var query = from c in Db.Comments
                            join u in Db.Users on c.AuthorId equals u.Id
                            join c2 in Db.Comments on c.ReplyToId equals c2.CommentId into replyCom
                            from c2 in replyCom.DefaultIfEmpty()
                            join u2 in Db.Users on c2.AuthorId equals u2.Id into replyUser
                            from u2 in replyUser.DefaultIfEmpty()
                            where c.ReviewId == ReviewId
                            orderby c.CreateTime descending
                            select new CommentsDTO
                            {
                                Id = c.CommentId,
                                Comment = c.Content,
                                Author = new UserDTO
                                {
                                    Id = u.Id,
                                    UserName = u.UserName
                                },
                                CreateTime = c.CreateTime,
                                Likes = c.Likes,
                                Reply = c2 != null ? new AnswerDTO
                                {
                                    ReplyToId = c2.CommentId,
                                    UserName = u2.UserName
                                } : null,
                                Liked = userId != null && !String.IsNullOrEmpty(userId) ? c.UserLiked != null && !String.IsNullOrEmpty(c.UserLiked) ?  c.UserLiked.Contains(userId) ? true : false : false : false
                            };

                var commentList = await query.ToListAsync();
                return commentList;
            }
            return new List<CommentsDTO>();
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(string comment, int id, int ReviewId)
        {
            int success = 0;
            if (comment != "" && ReviewId > 0)
            {
                ApplicationUser user = null;
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                }
                var model = new Comment()
                {
                    Content = comment,
                    CreateTime = DateTime.Now,
                    EditTime = DefaultDatetime,
                    ReplyToId = id,
                    AuthorId = user.Id ?? "",
                    Likes = 0,
                    ReviewId = ReviewId
                };
                Db.Comments.Add(model);                
                
                success = await Db.SaveChangesAsync();                
                
                return Json(new { success });
            }

            return Json(new { success });
        }

        [HttpPost]        
        public async Task<IActionResult> PostLike(int id)
        {
            var success = 0;
            if (id != -1)
            {
                ApplicationUser user = null;
                var userId = "";
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    userId = user.Id + ",";
                }
                if (userId == "")
                {
                    return RedirectToAction("Login", "Account");
                }

                var query = from c in Db.Comments
                            where c.CommentId == id
                            select c;
                var comment = await query.FirstOrDefaultAsync();
                if (comment != null)
                {
                    comment.Likes++;
                    
                    if (!String.IsNullOrEmpty(comment.UserLiked))
                    {
                        if (!comment.UserLiked.Contains(user.Id))
                            comment.UserLiked += userId;
                    }
                    else
                    {
                        comment.UserLiked = userId;
                    }
                        
                    Db.Entry(comment).State = EntityState.Modified;
                }
            }
            
            success = await Db.SaveChangesAsync();

            return Json(new { success });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveLike(int id)
        {
            var success = 0;
            if (id != -1)
            {
                ApplicationUser user = null;
                var userId = "";
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    userId = user.Id + ",";
                }
                if (userId == "")
                {
                    return Json(new { });
                }

                var query = from c in Db.Comments
                            where c.CommentId == id
                            select c;
                var comment = await query.FirstOrDefaultAsync();
                if (comment != null)
                {
                    comment.Likes--;
                    if (!String.IsNullOrEmpty(comment.UserLiked))
                    {
                        if (comment.UserLiked.Contains(userId))
                        {
                            comment.UserLiked = comment.UserLiked.Remove(comment.UserLiked.IndexOf(userId), userId.Length);
                        }                            
                    }
                    Db.Entry(comment).State = EntityState.Modified;
                }
            }
            success = await Db.SaveChangesAsync();
            return Json(new { success });
        }


        [HttpGet]
        public IActionResult Create(CreateReviewViewModel review)
        {
            review.Categories = Db.Categories.ToList();
            ViewBag.subs = new List<SubCategory>(Db.SubCategories.Where(s => s.CategoryId == 1));
            return View(review);
        }

        public JsonResult GetSubCategories(string catname)
        {
            var catId = Db.Categories.Where(c => c.Name.ToLower() == catname.ToLower())
                                    .Select(c => c.Id).FirstOrDefault();
            var subs = Db.SubCategories.Where(s => s.CategoryId == catId);

            string responce = "";
            foreach (var sub in subs)
            {
                responce += String.Format("<li><label><input name=\"subCategory\" type=\"radio\" id=\"{0}\" value=\"{1}\" />{1}</label></li>", sub.SubCategoryId, sub.Name);
            }

            return Json(new { result = responce});
        }

        [HttpPost]
        public JsonResult CreateSubject( string subcatname, string subjname)
        {
            var checkexist = Db.Subjects.Where(sub => sub.Name.ToLower() == subjname.ToLower()).FirstOrDefault();
            if (checkexist == null)
            {
                var subCatId = Db.SubCategories.Where(s => s.Name == subcatname)
                                       .Select(s => s.SubCategoryId).FirstOrDefault();
                if (subCatId != 0)
                {
                    Db.Subjects.Add(new Subject { AverageRating = 0, Name = subjname, SubCategoryId = subCatId });
                    Db.SaveChanges();
                }
            }
            return Json(new { });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewViewModel content,  IFormFile uploadImage)
        {
            if (ModelState.IsValid)
            {
                Review review = new Review
                {
                    SubjectId = Db.Subjects.Where(s => s.Name.ToLower() == content.Objectname.ToLower()).Select(s => s.SubjectId).FirstOrDefault(),
                    Rating = content.Rating,
                    Recommend = Convert.ToByte(content.Recomendations ? 1 : 0),
                    Exp = Convert.ToByte(content.Experience),
                    Like = content.Like,
                    Dislike = content.Dislike,
                    Content = content.Comment
            };

                Subject subject = Db.Subjects.Where(su => su.SubjectId == review.SubjectId).FirstOrDefault();
                if (subject.AverageRating == 0)
                {
                    subject.AverageRating = (double)content.Rating;
                }
                else
                {
                    double[] rating = Db.Reviews.Where(r => r.SubjectId == review.SubjectId).Select(r => r.Rating).ToArray();
                    subject.AverageRating = (rating.Sum() + content.Rating) / rating.Count()+1;
                }

                ApplicationUser user = null;
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    review.AuthorId = user.Id;
                }

                if (uploadImage != null)
                {
                    byte[] imageData = null;                    
                    using (var reader = new StreamReader(uploadImage.OpenReadStream()))
                    {
                        var fileContent = reader.ReadToEnd();
                        var parsedContentDisposition = ContentDispositionHeaderValue.Parse(uploadImage.ContentDisposition);
                        var fileName = parsedContentDisposition.FileName;
                    }
                    review.Image = imageData;
                }

                review.DateCreate = DateTime.Now;

                Db.Reviews.Add(review);
                await Db.SaveChangesAsync();
                
                
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public  IActionResult AutocompleteSearch(string term)
        {

            var models = Db.Subjects.Where(a => a.Name.Contains(term))
                            .Select(a => new { value = a.Name, id = a.SubjectId, subcat = a.SubCategoryId })
                            .Distinct();

            return Json(Convert.ToString(models.GetType().GetProperty("value")));
        }

        public IActionResult CheckExistSubject(string term)
        {
            var subjectId = Db.Subjects.Where(s => s.Name.ToLower() == term.ToLower()).Select(s => s.SubjectId).FirstOrDefault();

            if (subjectId > 0)
                return Json(new { correct = true });
            //return ViewBag.NoName = "";            
            else
                return Json(new { correct = false });
            //return ViewBag.NoName = "Похоже никто ещё не делал обзор на это.";

        }        

    }
}