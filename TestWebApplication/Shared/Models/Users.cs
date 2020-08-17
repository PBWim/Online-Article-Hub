namespace Shared.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserModel
    {
        public int Id { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "The User Name is required")]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        // https://stackoverflow.com/questions/16712043/email-address-validation-using-asp-net-mvc-data-type-attributes
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "The Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required(ErrorMessage = "The Confirm Password is required"), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }

        public string DateOfBirth { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<UserModel> GetUsers()
        {
            return new List<UserModel>() {
                new UserModel {
                    Id = 101,
                    UserName = "anet",
                    FirstName = "Anet",
                    LastName = "Test",
                    EmailId = "anet@test.com",
                    Password = "anet123",
                    Role="Admin",
                    DateOfBirth  = "01/01/2012"
                }
            };
        }
    }
}