using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace Genii_Assessment.ViewModels
{
    /// <summary>
    /// View model used to edit an existing user.
    /// </summary>
    public class UserEditViewModel
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// User email address.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// Assigned role.
        /// </summary>
        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; }

        /// <summary>
        /// Indicates whether the account is active.
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Available role options.
        /// </summary>
        public IEnumerable<SelectListItem> RoleOptions { get; set; }
    }
}