using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reshop.Application.Interfaces;
using Reshop.Domain.Models.ProductAndCategory;
using Reshop.Domain.Models.User.Cart;
using Reshop.Domain.Models.User.Comment;
using Reshop.Domain.Models.User.Identity;

namespace Reshop.Infrastructure.Context
{
    public class ReshopDbContext : IdentityDbContext<User, Role, Guid>, IReshopDbContext
    {

        public ReshopDbContext(DbContextOptions<ReshopDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ProductToCategory> ProductToCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CommentForProduct> CommentsForProduct { get; set; }
        public DbSet<ChildCategory> ChildCategories { get; set; }
        public DbSet<ChildCategoryToCategory> ChildCategoryToCategories { get; set; }
        public DbSet<ProductToChildCategory> ProductToChildCategories { get; set; }
        public DbSet<AnswerToComment> AnswerToComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<ProductToCategory>()
                .HasKey(t => new { t.ProductId, t.CategoryId });

            builder.Entity<ChildCategoryToCategory>()
                .HasKey(t => new { t.CategoryId, t.ChildCategoryId });

            builder.Entity<ProductToChildCategory>()
                .HasKey(t => new { t.ProductId, t.ChildCategoryId });

            builder.Entity<Item>(i =>
            {
                i.Property(w => w.Price).HasColumnType("Money");
            });

            builder.Entity<OrderDetail>(i =>
            {
                i.Property(w => w.Price).HasColumnType("Money");
            });




            base.OnModelCreating(builder);
        }
    }
}
