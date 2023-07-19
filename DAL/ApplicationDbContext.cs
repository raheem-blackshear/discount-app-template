using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DAL.Models.Interfaces;
using DAL.Models.Questions;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public string CurrentUserId { get; set; }

        public DbSet<Category> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductUser> ProductUsers { get; set; }

        public DbSet<StoreStyle> StoreStyles { get; set; }

        public DbSet<StoreUser> StoreUsers { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Style> Styles { get; set; }

        public DbSet<Lookbook> Lookbooks { get; set; }

        public DbSet<UserModel> TBL_User { get; set; }

        public DbSet<ProductImage> ProductImage { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationRole>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

         

            builder.Entity<Category>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Category>().Property(p => p.Description).HasMaxLength(500);
            builder.Entity<Category>().ToTable($"App{nameof(this.ProductCategories)}");

            builder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Product>().HasIndex(p => p.Name);
            builder.Entity<Product>().HasIndex(p => p.IsOnSale);
            builder.Entity<Product>().HasIndex(p => p.IsTrending);
            builder.Entity<Product>().HasIndex(p => p.DealDate);
            builder.Entity<Product>().Property(p => p.Description).HasMaxLength(500);
            builder.Entity<Product>().Property(p => p.Icon).IsUnicode(false).HasMaxLength(256);
            builder.Entity<Product>().ToTable($"App{nameof(this.Products)}");

            

            builder.Entity<UserModel>().Property(p => p.FullName).IsRequired().HasMaxLength(100);
            builder.Entity<UserModel>().HasIndex(c => c.FullName);
            builder.Entity<UserModel>().Property(p => p.IsActive).HasDefaultValue(value: true);
            builder.Entity<UserModel>().ToTable($"{nameof(this.TBL_User)}");


            builder.Entity<ProductUser>()
                 .HasKey(bc => new { bc.UserId, bc.ProductId });
            builder.Entity<ProductUser>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.ProductUsers)
                .HasForeignKey(bc => bc.ProductId);
            builder.Entity<ProductUser>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.ProductUsers)
                .HasForeignKey(bc => bc.UserId);

            builder.Entity<StoreUser>()
                 .HasKey(bc => new { bc.UserId, bc.StoreId });
            builder.Entity<StoreUser>()
                .HasOne(bc => bc.Store)
                .WithMany(b => b.StoreUsers)
                .HasForeignKey(bc => bc.StoreId);
            builder.Entity<StoreUser>()
                .HasOne(bc => bc.User)
                .WithMany(c => c.StoreUsers)
                .HasForeignKey(bc => bc.UserId);

            builder.Entity<StoreStyle>()
                 .HasKey(bc => new { bc.StyleId, bc.StoreId });
            builder.Entity<StoreStyle>()
                .HasOne(bc => bc.Store)
                .WithMany(b => b.StoreStyles)
                .HasForeignKey(bc => bc.StoreId);
            builder.Entity<StoreStyle>()
                .HasOne(bc => bc.Style)
                .WithMany(c => c.StoreStyles)
                .HasForeignKey(bc => bc.StyleId);

            //Questions RelationShips

            builder.Entity<QuestionOnlineShopping>()
                 .HasKey(bc => new { bc.QuestionId, bc.OnlineId });
            builder.Entity<QuestionOnlineShopping>()
                .HasOne(bc => bc.Question)
                .WithMany(b => b.QuestionOnlineShoppings)
                .HasForeignKey(bc => bc.QuestionId);
            builder.Entity<QuestionOnlineShopping>()
                .HasOne(bc => bc.OnlineShopping)
                .WithMany(c => c.QuestionOnlineShoppings)
                .HasForeignKey(bc => bc.OnlineId);


            builder.Entity<QuestionPreferredStyle>()
                 .HasKey(bc => new { bc.QuestionId, bc.PreferredId });
            builder.Entity<QuestionPreferredStyle>()
                .HasOne(bc => bc.Question)
                .WithMany(b => b.QuestionPreferredStyles)
                .HasForeignKey(bc => bc.QuestionId);
            builder.Entity<QuestionPreferredStyle>()
                .HasOne(bc => bc.PreferredStyle)
                .WithMany(c => c.QuestionPreferredStyles)
                .HasForeignKey(bc => bc.PreferredId);

            builder.Entity<QuestionProductType>()
                .HasKey(bc => new { bc.QuestionId, bc.ProductTypeId });
            builder.Entity<QuestionProductType>()
                .HasOne(bc => bc.Question)
                .WithMany(b => b.QuestionProductTypes)
                .HasForeignKey(bc => bc.QuestionId);
            builder.Entity<QuestionProductType>()
                .HasOne(bc => bc.ProductType)
                .WithMany(c => c.QuestionProductTypes)
                .HasForeignKey(bc => bc.ProductTypeId);

            builder.Entity<QuestionShoppingTime>()
                .HasKey(bc => new { bc.QuestionId, bc.ShoppingTimeId });
            builder.Entity<QuestionShoppingTime>()
                .HasOne(bc => bc.Question)
                .WithMany(b => b.QuestionShoppingTimes)
                .HasForeignKey(bc => bc.QuestionId);
            builder.Entity<QuestionShoppingTime>()
                .HasOne(bc => bc.ShoppingTime)
                .WithMany(c => c.QuestionShoppingTimes)
                .HasForeignKey(bc => bc.ShoppingTimeId);

            builder.Entity<QuestionCategory>()
                .HasKey(bc => new { bc.QuestionId, bc.CategoryId });
            builder.Entity<QuestionCategory>()
                .HasOne(bc => bc.Question)
                .WithMany(b => b.QuestionCategories)
                .HasForeignKey(bc => bc.QuestionId);
            builder.Entity<QuestionCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.QuestionCategories)
                .HasForeignKey(bc => bc.CategoryId);

        }
        

        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;
            }
        }
    }
}
