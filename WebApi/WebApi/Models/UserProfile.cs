using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class UserProfile
    {
        public int UserProfileId { get; set; }
        public User User { get; set; }
        public Currency Currency { get; set; }
        public DisplayLanguage DisplayLanguage { get; set; }
        public Address Address { get; set; }
        public bool IsEmailVerified { get; set; }
        public AccountType AccountType { get; set; }
    }
}