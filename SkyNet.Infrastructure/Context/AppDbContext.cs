using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyNet.Core.Entities.User;
using SkyNet.Infrastructure.Initizalizers;
using System.Data;
using SkyNet.Core.Entities.Site;

namespace SkyNet.Infrastructure.Context
{
    internal class AppDbContext : IdentityDbContext
    {
        public AppDbContext() : base() { }
        public AppDbContext(DbContextOptions<AppDbContext>options) : base(options) { }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.SeedCategories();
            builder.SeedPosts();

            builder.Entity<Post>().HasOne(c => c._Category).WithMany(p => p._Posts).HasForeignKey(c => c.CategoryId);
        }
    }
}
