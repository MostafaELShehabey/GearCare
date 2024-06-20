using DomainLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace ServiceLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
           

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RepareOrder_ApplicationUser> RepairOrder_ApplicationUsers { get; set; }  
        public DbSet<RepareOrder> RepareOrders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Photo> photos { get; set; }
        public DbSet<Product_Shoppingcart> product_Shoppingcarts { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<WinchDriver> WinchDrivers { get; set; }
        public DbSet<WinchOrder> WinchOrders { get; set; }
        public DbSet<Winch> Winchs { get; set; }
        public DbSet<Discount> discounts { get; set; }


        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    builder.Entity<ApplicationUser>()
        //        .HasOne(o => o.photo)
        //        .WithOne(p => p.user)
        //        .HasForeignKey<Photo>(c => c.userid);

        //    builder.Entity<Product>()
        //    .HasMany(p => p.Photos)
        //    .WithOne(photo => photo.Product)
        //    .HasForeignKey(photo => photo.photoURL);
        //}

    }

}
