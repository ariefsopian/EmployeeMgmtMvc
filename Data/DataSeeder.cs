using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMgmtMvc.Data
{
    public static class DataSeeder
    {
        public static async Task MigrateAndSeedAsync(IServiceProvider sp)
        {
            using var scope = sp.CreateScope();
            var services = scope.ServiceProvider;
            var ctx = services.GetRequiredService<AppDbContext>();
            var um  = services.GetRequiredService<UserManager<IdentityUser>>();
            var rm  = services.GetRequiredService<RoleManager<IdentityRole>>();

            await ctx.Database.MigrateAsync();

            var roles = new[] { "Admin", "Staff" };
            foreach (var r in roles)
                if (!await rm.RoleExistsAsync(r))
                    await rm.CreateAsync(new IdentityRole(r));

            var adminEmail = "admin@local.test";
            var admin = await um.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await um.CreateAsync(admin, "Admin123!");
                await um.AddToRoleAsync(admin, "Admin");
            }

            var staffEmail = "staff@local.test";
            var staff = await um.FindByEmailAsync(staffEmail);
            if (staff is null)
            {
                staff = new IdentityUser { UserName = staffEmail, Email = staffEmail, EmailConfirmed = true };
                await um.CreateAsync(staff, "Staff123!");
                await um.AddToRoleAsync(staff, "Staff");
            }
        }
    }
}