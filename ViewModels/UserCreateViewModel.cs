using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace Genii_Assessment.ViewModels
{
    public class UserCreateViewModel
    {
        /// <summary>
        /// Email address / username.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        /// <summary>
        /// Initial password for the new user.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Password confirmation.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Role to assign to the user.
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

        public UserCreateViewModel()
        {
            IsActive = true;
            RoleOptions = new List<SelectListItem>();
        }
    }
}