namespace Data
{
    using System;
    using System.Linq;
    using DataModel;
    using Microsoft.AspNetCore.Identity;

    public class DbInitializer
    {
        public static void InitializeDatabase(ApplicationDbContext context)
        {
            SeedAdminUser(context);
        }

        private static void SeedAdminUser(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var adminUser = new User
                {
                    FirstName = "SuperAdmin",
                    LastName = "SuperAdmin",
                    Email = "SuperAdmin@example.com",
                    NormalizedEmail = "SUPERADMIN@EXAMPLE.COM",
                    UserName = "SuperAdmin@example.com",
                    NormalizedUserName = "SUPERADMIN@EXAMPLE.COM",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Role = "Admin",
                    LastModifiedBy = 0,
                    LastModifiedOn = DateTime.UtcNow,
                };
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(adminUser, "TestingPassword123@");
                adminUser.PasswordHash = hashed;
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}