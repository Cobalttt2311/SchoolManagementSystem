using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Modules.Users.Entities;

namespace SchoolManagementSystem.Data;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
    {
        var userManager = service.GetService<UserManager<ApplicationUser>>();
        var roleManager = service.GetService<RoleManager<IdentityRole>>();
        
        string[] roles = { "Admin", "Teacher", "Student" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Membuat Admin User
        var adminEmail = "admin@school.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, "Admin@123");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}