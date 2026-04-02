using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Genii_Assessment.Models;
using Genii_Assessment.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Genii_Assessment.Controllers
{
    [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.Manager)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private ApplicationUserManager _userManager;
        private RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UsersController()
        {
            _dbContext = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets the ASP.NET Identity user manager.
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Gets the role manager.
        /// </summary>
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbContext));
            }
            private set
            {
                _roleManager = value;
            }
        }

        /// <summary>
        /// Displays a list of users.
        /// </summary>
        /// <returns>User list view.</returns>
        public async Task<ActionResult> Index()
        {
            var users = await _dbContext.Users
                .OrderBy(u => u.Email)
                .ToListAsync();

            var userIds = users.Select(u => u.Id).ToList();
            var userRoles = await _dbContext.Set<IdentityUserRole>().ToListAsync();
            var roles = await _dbContext.Roles.ToListAsync();

            var model = users.Select(user =>
            {
                var userRole = userRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                var roleName = userRole == null
                    ? string.Empty
                    : roles.FirstOrDefault(r => r.Id == userRole.RoleId)?.Name;

                return new UserListItemViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    RoleName = roleName
                };
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// Displays the create user form.
        /// </summary>
        /// <returns>Create user view.</returns>
        public ActionResult Create()
        {
            var model = new UserCreateViewModel
            {
                RoleOptions = GetRoleSelectListItems()
            };

            return View(model);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="model">Create user model.</param>
        /// <returns>Redirect to user list on success.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserCreateViewModel model)
        {
            model.RoleOptions = GetRoleSelectListItems();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await UserManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "A user with this email address already exists.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                IsActive = model.IsActive
            };

            var createResult = await UserManager.CreateAsync(user, model.Password);

            if (!createResult.Succeeded)
            {
                AddIdentityErrors(createResult);
                return View(model);
            }

            var roleExists = await RoleManager.RoleExistsAsync(model.SelectedRole);
            if (!roleExists)
            {
                ModelState.AddModelError("SelectedRole", "The selected role does not exist.");
                return View(model);
            }

            var roleResult = await UserManager.AddToRoleAsync(user.Id, model.SelectedRole);

            if (!roleResult.Succeeded)
            {
                AddIdentityErrors(roleResult);
                return View(model);
            }

            TempData["SuccessMessage"] = "User created successfully.";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Displays the edit form for a user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Edit view.</returns>
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var currentRoles = await UserManager.GetRolesAsync(user.Id);
            var currentRole = currentRoles.FirstOrDefault() ?? string.Empty;

            var model = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email,
                IsActive = user.IsActive,
                SelectedRole = currentRole,
                RoleOptions = GetRoleSelectListItems()
            };

            return View(model);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="model">Edit user model.</param>
        /// <returns>Redirect to user list on success.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserEditViewModel model)
        {
            model.RoleOptions = GetRoleSelectListItems();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var emailInUse = await _dbContext.Users
                .AnyAsync(u => u.Email == model.Email && u.Id != model.Id);

            if (emailInUse)
            {
                ModelState.AddModelError("Email", "A user with this email address already exists.");
                return View(model);
            }

            user.Email = model.Email;
            user.UserName = model.Email;
            user.IsActive = model.IsActive;

            var updateResult = await UserManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                AddIdentityErrors(updateResult);
                return View(model);
            }

            var existingRoles = await UserManager.GetRolesAsync(user.Id);

            if (existingRoles.Any())
            {
                var removeResult = await UserManager.RemoveFromRolesAsync(user.Id, existingRoles.ToArray());

                if (!removeResult.Succeeded)
                {
                    AddIdentityErrors(removeResult);
                    return View(model);
                }
            }

            var addRoleResult = await UserManager.AddToRoleAsync(user.Id, model.SelectedRole);

            if (!addRoleResult.Succeeded)
            {
                AddIdentityErrors(addRoleResult);
                return View(model);
            }

            TempData["SuccessMessage"] = "User updated successfully.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.Manager)]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.Manager)]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.Id == User.Identity.GetUserId())
            {
                TempData["ErrorMessage"] = "You cannot deactivate your own account.";
                return RedirectToAction("Index");
            }

            user.IsActive = false;

            var result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                AddIdentityErrors(result);
                return View(user);
            }

            TempData["SuccessMessage"] = "User deactivated successfully.";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Builds role options for dropdown lists.
        /// </summary>
        /// <returns>Collection of role select list items.</returns>
        private IQueryable<SelectListItem> GetRoleSelectListItems()
        {
            return _dbContext.Roles
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                });
        }

        /// <summary>
        /// Adds identity errors to model state.
        /// </summary>
        /// <param name="result">Identity result.</param>
        private void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        /// <summary>
        /// Releases controller resources.
        /// </summary>
        /// <param name="disposing">Dispose flag.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
                _roleManager?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}