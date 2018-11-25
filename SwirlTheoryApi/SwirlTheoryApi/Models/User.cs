using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// User data for our application's user accounts
    /// </summary>
    public class User : IdentityUser
    {
        /*
        [Key]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public override string Email { get; set; }
        [Required]
        public string Password { get; set; }
        */
    }
}