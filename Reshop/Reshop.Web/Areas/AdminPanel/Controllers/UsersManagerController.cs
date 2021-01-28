using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reshop.Application.Services;
using Reshop.Domain.Models.User.Identity;
using Reshop.Domain.ViewModels.User;
using Reshop.Domain.ViewModels.User.Role;

namespace Reshop.Web.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class UsersManagerController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UsersManagerController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Index

        // Take Users Data
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await GetAllUsers());
        }

        #endregion

        #region Edit User

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId = null)
        {


            // Show 404 Error When User Id is Empty
            if (string.IsNullOrEmpty(userId)) return NotFound();

            // Find User Id
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var userInformation = new AddOrEditUserViewModel()
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
            };
            return View(userInformation);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(AddOrEditUserViewModel model)
        {
            // Show Error When User Id And UserName Are Empty
            if (string.IsNullOrEmpty(model.Id))
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditProduct", model) });

            // Find User Id
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditProduct", model) });

            // Change User Data

            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            if (model.Password != null)
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

            // Update Data
            var result = await _userManager.UpdateAsync(user);



            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            // Show Errors       
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        #endregion

        #region Add User To Role

        [HttpGet]
        public async Task<IActionResult> AddUserToRole(string userId)
        {
            // Show Error When User Id Is Empty
            if (string.IsNullOrEmpty(userId)) return NotFound();

            // Find User Id
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var validRoles = await _roleManager.Roles
                .Select(c => c.Name)
                .Where(c => !userRoles.Contains(c))
                .ToListAsync();


            var model = new AddOrRemoveUserFromRoleViewModel()
            {
                UserId = userId,
                RolesName = validRoles
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToRole(AddOrRemoveUserFromRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddUserToRole", model) });

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            try
            {
                await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "User/_BoxManageUsers", await GetAllUsers()) });
            }
            catch
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddUserToRole", model) });
            }
        }

        #endregion

        #region Remove User From Role

        [HttpGet]
        public async Task<IActionResult> RemoveUserFromRole(string userId)
        {
            // Show 404 Error When User Id is Empty
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            // Find User Id
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();


            var model = new AddOrRemoveUserFromRoleViewModel()
            {
                UserId = userId,
                RolesName = await _userManager.GetRolesAsync(user)
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserFromRole(AddOrRemoveUserFromRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "RemoveUserFromRole", model) });

            // Find User Id By model.UserId
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            try
            {
                await _userManager.RemoveFromRolesAsync(user, model.SelectedRoles);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "User/_BoxManageUsers", await GetAllUsers()) });
            }
            catch
            {
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "RemoveUserFromRole", model) });
            }
        }

        #endregion

        #region Delete User



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            // Show 404 Error When User Id Is Empty
            if (string.IsNullOrEmpty(userId)) return NotFound();

            // Find User Id
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // Delete User And Update Database
            await _userManager.DeleteAsync(user);


            return RedirectToAction(nameof(Index));

        }

        #endregion


        private async Task<IEnumerable<ManageUsersViewModel>> GetAllUsers()
        {
            return await _userManager.Users
                .Select(c => new ManageUsersViewModel()
                {
                    Id = c.Id.ToString(),
                    FullName = c.FullName,
                    UserName = c.UserName,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    Email = c.Email
                }).ToListAsync();
        }
    }
}