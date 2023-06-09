using Microsoft.AspNetCore.Identity;
using System;

namespace VendingApp.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? FullName { get; set; }

        public void SetContactInformation(string? email,
                                          string? phone,
                                          string? fullName)
        {
            if (email != null) Email = email;
            if (phone != null) PhoneNumber = phone;
            if (fullName != null) FullName = fullName;
        }
    }

    public struct UserRoles
    {
        public const string Admin = "Admin";

        public const string Student = "Student";
    }
}
