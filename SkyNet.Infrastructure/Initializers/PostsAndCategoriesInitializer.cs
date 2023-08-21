using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using SkyNet.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Infrastructure.Initizalizers
{
    internal static class PostsAndCategoriesInitializer
    {
        public static void SeedCategories(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category[]
            {
                new Category()
                {
                     ID = 1,
                     Name = "Car"
                },
                new Category()
                {
                     ID = 2,
                     Name = "Girl"
                },
                new Category()
                {
                     ID = 3,
                     Name = "Nature"
                },
            });
        }
        public static void SeedPosts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().HasData(new Post[]
            {
                new Post()
                {
                     ID = 1,
                     Text = "Test",
                     Description = "Test",
                     Image = "Car_1",
                     PublishDate = DateTime.Now,
                     CategoryId = 1
                },
                new Post()
                {
                     ID = 2,
                     Text = "Test",
                     Description = "Test",
                     Image = "Car_2",
                     PublishDate = DateTime.Now,
                     CategoryId = 1
                },
                new Post()
                {
                     ID = 3,
                     Text = "Test",
                     Description = "Test",
                     Image = "Girl_1",
                     PublishDate = DateTime.Now,
                     CategoryId = 2
                },
                new Post()
                {
                     ID = 4,
                     Text = "Test",
                     Description = "Test",
                     Image = "Girl_2",
                     PublishDate = DateTime.Now,
                     CategoryId = 2
                },
                new Post()
                {
                     ID = 5,
                     Text = "Test",
                     Description = "Test",
                     Image = "Nature_1",
                     PublishDate = DateTime.Now,
                     CategoryId = 3
                },
                new Post()
                {
                     ID = 6,
                     Text = "Test",
                     Description = "Test",
                     Image = "Nature_2",
                     PublishDate = DateTime.Now,
                     CategoryId = 3
                },
            });
        }
    }
}
