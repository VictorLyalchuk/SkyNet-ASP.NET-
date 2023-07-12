using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SkyNet.Core.Entities.User;
using SkyNet.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SkyNet.Infrastructure.Initizalizers;
using Microsoft.EntityFrameworkCore;

namespace SkyNet.Infrastructure.Initizalizers
{
    public static class UsersAndRolesInitializer
    {
        public static void Seed(this ModelBuilder builder)
        {
            static async Task SeedUsersAndRoles(AppDbContext context, UserManager<AppUser> userManager)
            {
                if (!context.Roles.Any())
                {
                    await context.Roles.AddRangeAsync(
                        new IdentityRole()
                        {
                            Name = "administrator",
                            NormalizedName = "ADMINISTRATOR",
                        }
                    );
                    await context.SaveChangesAsync();
                }

                if (await userManager.FindByEmailAsync("admin@example.com") == null)
                {
                    var admin = new AppUser()
                    {
                        FirstName = "Victor",
                        LastName = "Connor",
                        UserName = "Victor@example.com",
                        Email = "Victor@example.com",
                        EmailConfirmed = true,
                        PhoneNumber = "+xx(xx)xxx-xx-xx",
                        PhoneNumberConfirmed = true,
                    };

                    var adminResult = await userManager.CreateAsync(admin, "Qwerty-1");
                    if (adminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "administrator");
                    }
                }
            }




            //AppUser admin = new AppUser()
            //{
            //    FirstName = "John",
            //    LastName = "Connor",
            //    UserName = "admin@email.com",
            //    Email = "admin@email.com",
            //    EmailConfirmed = true,
            //    PhoneNumber = "+xx(xxx)xxx-xx-xx",
            //    PhoneNumberConfirmed = true,
            //};

            //new IdentityRole()
            //{
            //    Name = "Administrator",
            //    NormalizedName = "ADMINISTRATOR"
            //};








            //    var pwd = "P@$$w0rd";
            //    var passwordHasher = new PasswordHasher<IdentityUser>();

            //    var adminRole = new IdentityRole()
            //    {
            //        Name = "administrator",
            //        NormalizedName = "ADMINISTRATOR",
            //    };

            //    List<IdentityRole> roles = new List<IdentityRole>() {
            //    adminRole,
            //    };

            //    builder.Entity<IdentityRole>().HasData(roles);

            //    var adminUser = new AppUser
            //    {
            //        FirstName = "Victor",
            //        LastName = "Connor",
            //        UserName = "Victor@example.com",
            //        Email = "Victor@example.com",
            //        EmailConfirmed = true,
            //        PhoneNumber = "+xx(xx)xxx-xx-xx",
            //        PhoneNumberConfirmed = true,
            //    };
            //    adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
            //    adminUser.NormalizedEmail = adminUser.Email.ToUpper();
            //    adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, pwd);

            //    List<IdentityUser> users = new List<IdentityUser>() {
            //        adminUser,
            //    };

            //    builder.Entity<IdentityUser>().HasData(users);

            //    List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

            //    userRoles.Add(new IdentityUserRole<string>
            //    {
            //        UserId = users[0].Id,
            //        RoleId = roles.First(q => q.Name == "Admin").Id
            //    });

            //    builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
            //}
        }


    }
}
