using Excursion.Models;
using Microsoft.AspNetCore.Identity;

namespace Excursion.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();


                if (!context.Tours.Any())
                {
                    context.Tours.AddRange(new List<Tour>()
                    {
                        
                        new Tour()
                        {
                            Name = "Snake café",
                            Description = "Enjoy coffee and sweets while spending time with reptile friends. Tokyo Snake Centre offers an unique opportunity. Meet the snakes, dine with them and if you are brave enough -even hold them!",
                            Price=55,
                            Image= "snakecafe.jpg",
                            Minor = 0,
                            Address = new Address()
                            {
                                Street = "254-1030, Kosuiji",
                                City = "Yahaba-cho Shiwa-gun",
                                Country = "Japan"
                            },
                        },
                        new Tour()
                        {
                            Name = "Anime & Gaming Adventure Walking Tour",
                            Description = "Anime and gaming fan? Explore this part of Japanese culture by visiting most popular Akihabara attractions, such as maid cafe and retro gaming store.",
                            Price=78,
                            Image= "walkingtour.jpg",
                            Minor = 0,
                            Address = new Address()
                            {
                                Street = "473-1051, Nishiwakecho",
                                City = "Tokyo",
                                Country = "Japan"
                            },
                        },
                        new Tour()
                        {
                            Name = "Maison de Julietta",
                            Description = "This excursion offers visitors an unique opportunity to get to know Lolita fashion, by transforming yourself into one of the Lolitas! Makeup, wigs, dresses and pictures - all included in this distinct experience!",
                            Price=60,
                            Image= "fashion.jpg",
                            Minor = 0,
                            Address = new Address()
                            {
                                Street = "248-1297, Yokocho",
                                City = "Kawagoe",
                                Country = "Japan"
                            },
                        }


                    });

                    context.SaveChanges();
                }


            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "turgut@gvcn.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "turgutgvcn",
                        Email = adminUserEmail,
                        EmailConfirmed = true,

                    };
                    await userManager.CreateAsync(newAdminUser, "netcoremvc7.0");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "pigeon@pigeon.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "pigeon",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                    };
                    await userManager.CreateAsync(newAppUser, "CanPigeonsCountUpTo9?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }

    }
}
