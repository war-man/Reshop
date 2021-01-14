using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            // Extract Users Data From Database
            var model = await _userManager.Users
                .Select(c => new ManageUsersViewModel()
                {
                    Id = c.Id.ToString(),
                    FullName = c.FullName,
                    UserName = c.UserName,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    Email = c.Email
                }).ToListAsync();

            return View(model);
        }

        #endregion

        #region Edit User

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
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
            if (string.IsNullOrEmpty(model.Id)) return NotFound();

            // Find User Id
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

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
                return RedirectToAction("index");

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
            // Show 404 Error When Model Is Empty
            if (model == null)
                return NotFound();

            // Find User Id By model.UserId
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();


            // Update Database
            var result = await _userManager.AddToRolesAsync(user, model.SelectedRoles);

            if (result.Succeeded)
                return RedirectToAction("index");

            // Show Errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        #endregion

        #region Remove User From Role

        [HttpGet]
        public async Task<IActionResult> RemoveUserFromRole(string id, string returnUrl = null)
        {
            // Show 404 Error When User Id is Empty
            if (string.IsNullOrEmpty(id))
                return NotFound();

            // Find User Id
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();


            var model = new AddOrRemoveUserFromRoleViewModel()
            {
                UserId = id,
                RolesName = await _userManager.GetRolesAsync(user)
            };

            ViewData["returnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(AddOrRemoveUserFromRoleViewModel model, string returnUrl = null)
        {
            // Show 404 Error When Model Is Empty
            if (model == null) return NotFound();

            // Find User Id By model.UserId
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            ViewData["returnUrl"] = returnUrl;

            // Remove Selected Roles
            var result = await _userManager.RemoveFromRolesAsync(user, model.SelectedRoles);

            if (result.Succeeded)
                return RedirectToAction("Index");

            // Show Errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
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
    }
}