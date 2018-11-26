using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }
        [Required]
        public IdentityUser User { get; set; }
        [Url]
        public string ImageUrl { get; set; }
        public Currency Currency { get; set; }
        public DisplayLanguage DisplayLanguage { get; set; }
        public Address Address { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
    }
}