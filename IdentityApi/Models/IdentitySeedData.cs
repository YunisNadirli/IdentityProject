using IdentityApi.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApi.Models
{
    public static class IdentitySeedData
    {
        private const string adminUsername = "admin";
        private const string adminPassword = "12345678";

        public static async void IdentityTestUser(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();

            if (context.Database.GetAppliedMigrations().Any())  context.Database.Migrate();

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var user = await userManager.FindByNameAsync(adminUsername);
            if(user == null)
            {
                user = new AppUser()
                {
                    UserName = adminUsername,
                    FullName = "Admin",
                    Email = "admin@gmail.com"
                };

                await userManager.CreateAsync(user);
                
            }
            await userManager.AddToRoleAsync(user, "admin");
        }
    }
}
