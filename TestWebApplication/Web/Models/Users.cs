namespace Web.Models
{
    using System.Collections.Generic;

    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string DateOfBirth { get; set; }
        public string ReturnUrl { get; set; }

        public IEnumerable<Users> GetUsers()
        {
            return new List<Users>() {
                new Users {
                    Id = 101,
                    UserName = "anet",
                    Name = "Anet",
                    EmailId = "anet@test.com",
                    Password = "anet123",
                    Role="Admin",
                    DateOfBirth  = "01/01/2012"
                }
            };
        }
    }
}