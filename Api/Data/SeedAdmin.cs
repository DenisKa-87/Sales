using Microsoft.AspNetCore.Identity;
using Api.Entities;
using Api.Interfaces;
using System.Text.Json;

namespace Api.Data
{
    public class SeedAdmin
    {
        public async Task Seed(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var roles = new List<AppRole>
            {
                new AppRole{Name = "Admin"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            var result = await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin" });
        }
    }
}
