using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Genii_Assessment.ViewModels
{
    public class UserListItemViewModel
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Indicates whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Date the account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Role currently assigned to the user.
        /// </summary>
        public string RoleName { get; set; }
    }
}