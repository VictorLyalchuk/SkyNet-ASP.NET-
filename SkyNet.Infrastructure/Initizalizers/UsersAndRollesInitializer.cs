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

namespace SkyNet.Infrastructure.Initizalizers
{
    public class UsersAndRollesInitializer
    {
        public static async Task SeedUsersAndRole(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                UserManager<AppUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                if (userManager.FindByEmailAsync("admin@email.com").Result == null)
                {
                    AppUser admin = new AppUser()
                    {
                        FirstName = "John",
                        LastName = "Connor",
                        UserName = "admin@email.com",
                        Email = "admin@email.com",
                        EmailConfirmed = true,
                        PhoneNumber = "+xx(xx)xxx-xx-xx",
                        PhoneNumberConfirmed = true,
                    };
                    context.Roles.AddRangeAsync(
                        new IdentityRole()
                        {
                            Name = "administrator",
                            NormalizedName = "ADMINISTRATOR",
                        }
                        );
                    await context.SaveChangesAsync();
                    IdentityResult adminResult = userManager.CreateAsync(admin, "Qwerty-1").Result;
                if(adminResult.Succeeded)
                    {
                        userManager.AddToRoleAsync(admin, "Administrator").Wait();
                    }
                }
            }
        }
    }
}
