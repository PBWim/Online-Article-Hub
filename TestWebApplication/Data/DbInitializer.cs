namespace Data
{
    using System;
    using System.Linq;
    using Common;
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
                    FirstName = Constants.SuperUserName,
                    LastName = Constants.SuperUserName,
                    Email = Constants.SuperUserEmail,
                    NormalizedEmail = Constants.SuperUserNormalizedEmail,
                    UserName = Constants.SuperUserEmail,
                    NormalizedUserName = Constants.SuperUserNormalizedEmail,
                    PhoneNumber = Constants.SuperUserPhone,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Role = Constants.AdminRole,
                    LastModifiedBy = 0,
                    LastModifiedOn = DateTime.UtcNow,
                };
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(adminUser, Constants.SuperUserPassword);
                adminUser.PasswordHash = hashed;
                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}