namespace Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AuthUserModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}