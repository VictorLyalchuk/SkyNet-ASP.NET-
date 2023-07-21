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
    public class UsersAndRolesInitializer
    {
        public static async Task SeedUserAndRole(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                UserManager<AppUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                if (userManager.FindByEmailAsync("JohnConnor@ukr.net").Result == null)
                {
                    context.Roles.AddRangeAsync(
                        new IdentityRole()
                        {
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        }
                    );
                    AppUser admin = new AppUser()
                    {
                        FirstName = "John",
                        LastName = "Connor",
                        UserName = "JohnConnor@ukr.net",
                        Email = "JohnConnor@ukr.net",
                        EmailConfirmed = true,
                        PhoneNumber = "+xx(xxx)xxx-xx-xx",
                        PhoneNumberConfirmed = true,
                    };

                    await context.SaveChangesAsync();

                    IdentityResult adminResult = userManager.CreateAsync(admin, "Qwerty-1").Result;
                    if (adminResult.Succeeded)
                    {
                        userManager.AddToRoleAsync(admin, "Administrator").Wait();
                    }
                }
            }
        }


        //public static void Seed(this ModelBuilder builder)
        //{
        //    static async Task SeedUsersAndRoles(AppDbContext context, UserManager<AppUser> userManager)
        //    {
        //        if (!context.Roles.Any())
        //        {
        //            await context.Roles.AddRangeAsync(
        //                new IdentityRole()
        //                {
        //                    Name = "administrator",
        //                    NormalizedName = "ADMINISTRATOR",
        //                }
        //            );
        //            await context.SaveChangesAsync();
        //        }

        //        if (await userManager.FindByEmailAsync("admin@example.com") == null)
        //        {
        //            var admin = new AppUser()
        //            {
        //                FirstName = "Victor",
        //                LastName = "Connor",
        //                UserName = "Victor@example.com",
        //                Email = "Victor@example.com",
        //                EmailConfirmed = true,
        //                PhoneNumber = "+xx(xx)xxx-xx-xx",
        //                PhoneNumberConfirmed = true,
        //            };

        //            var adminResult = await userManager.CreateAsync(admin, "Qwerty-1");
        //            if (adminResult.Succeeded)
        //            {
        //                await userManager.AddToRoleAsync(admin, "administrator");
        //            }
        //        }
        //    }
        //}
    }
}
