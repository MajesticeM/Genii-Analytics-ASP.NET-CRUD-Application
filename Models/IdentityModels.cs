using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Genii_Assessment.Models
{
    /// <summary>
    /// Represents an authenticated user within the system.
    /// Extends ASP.NET IdentityUser to include additional application-specific fields.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Indicates whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The date and time the user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Navigation property for invoices created by this user.
        /// </summary>
        public virtual ICollection<Invoice> CreatedInvoices { get; set; }

        /// <summary>
        /// Constructor to initialize default values.
        /// </summary>
        public ApplicationUser()
        {
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            CreatedInvoices = new HashSet<Invoice>();
        }

        /// <summary>
        /// Generates the ClaimsIdentity used for authentication.
        /// </summary>
        /// <param name="manager">User manager instance</param>
        /// <returns>ClaimsIdentity for the authenticated user</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Authentication type must match CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(
                this,
                DefaultAuthenticationTypes.ApplicationCookie);

            // Additional custom claims can be added here if needed

            return userIdentity;
        }
    }

    /// <summary>
    /// Database context for the application.
    /// Extends IdentityDbContext to include application entities.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Default constructor using connection string "DefaultConnection".
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        /// <summary>
        /// Products table.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Invoices table.
        /// </summary>
        public DbSet<Invoice> Invoices { get; set; }

        /// <summary>
        /// Invoice items table.
        /// </summary>
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        /// <summary>
        /// Factory method for creating the context.
        /// </summary>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /// <summary>
        /// Configures entity relationships and database schema rules.
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Always call base method first (required for Identity)
            base.OnModelCreating(modelBuilder);

            // =========================
            // Product Configuration
            // =========================

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Product>()
                .Property(p => p.CostPerItem)
                .HasPrecision(18, 2);

            // =========================
            // Invoice Configuration
            // =========================

            modelBuilder.Entity<Invoice>()
                .Property(i => i.TotalAmount)
                .HasPrecision(18, 2);

            // =========================
            // InvoiceItem Configuration
            // =========================

            modelBuilder.Entity<InvoiceItem>()
                .Property(ii => ii.UnitCost)
                .HasPrecision(18, 2);

            modelBuilder.Entity<InvoiceItem>()
                .Property(ii => ii.LineTotal)
                .HasPrecision(18, 2);

            // =========================
            // Relationships
            // =========================

            // Invoice -> User (Many invoices per user)
            modelBuilder.Entity<Invoice>()
                .HasRequired(i => i.CreatedByUser)
                .WithMany(u => u.CreatedInvoices)
                .HasForeignKey(i => i.CreatedByUserId)
                .WillCascadeOnDelete(false);

            // InvoiceItem -> Invoice (One-to-many)
            modelBuilder.Entity<InvoiceItem>()
                .HasRequired(ii => ii.Invoice)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.InvoiceId)
                .WillCascadeOnDelete(true);

            // InvoiceItem -> Product (Many items per product)
            modelBuilder.Entity<InvoiceItem>()
                .HasRequired(ii => ii.Product)
                .WithMany(p => p.InvoiceItems)
                .HasForeignKey(ii => ii.ProductId)
                .WillCascadeOnDelete(false);
        }
    }
}