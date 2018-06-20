using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using MyReviewProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyReviewProject.Controllers
{
    public class RoleController : BaseController
    {        
        public IActionResult Index()
        {
            
            var roles = _roleManager.Roles;
            Dictionary<string, string> usersDic = new Dictionary<string, string>();

            foreach (ApplicationRole role in roles)
            {                
                if (role.Users == null || role.Users.Count() == 0)
                {
                    usersDic.Add(role.Name, "Нет пользователей в этой роли");
                }
                else
                {
                    usersDic.Add(role.Name, string.Join(", ", role.Users.Select(x => _userManager.FindByIdAsync(x.UserId))));
                }
            }

            ViewBag.UserDic = usersDic;

            return View(_roleManager.Roles);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = model.Name,
                    Description = model.Description
                });

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    await _roleManager.DeleteAsync(await _roleManager.FindByNameAsync(model.Name));
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();

                IEnumerable<ApplicationUser> members
                = _userManager.Users.Where(x => memberIDs.Any(y => y == x.Id));

                IEnumerable<ApplicationUser> nonMembers = _userManager.Users.Except(members);

                return View(new EditRoleModel {Role = role, Description = role.Description, Members = members, NonMembers = nonMembers });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleModificationModel model)
        {            
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.IdentityResult result;
                ApplicationRole role = await _roleManager.FindByIdAsync(model.Id);               

                if (role != null)
                { 
                    role.Description = model.Description;
                    role.Name = model.RoleName;

                    foreach (string userId in model.IdsToAdd ?? new string[] { })
                    {
                        result = await _userManager.AddToRoleAsync(await _userManager.FindByIdAsync(userId), model.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }

                    foreach (string userId in model.IdsToDelete ?? new string[] { })
                    {
                        result = await _userManager.RemoveFromRoleAsync(await _userManager.FindByIdAsync(userId),
                        model.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                    return RedirectToAction("Index");
                    
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ApplicationRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }


    }
}