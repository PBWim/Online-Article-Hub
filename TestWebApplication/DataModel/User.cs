namespace DataModel
{
    using System;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser<int>, IBaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime LastModifiedOn { get; set; }
    }
}