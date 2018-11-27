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
        [Url]
        public string ProfileImageUrl { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<PaymentDetails> Cards { get; set; }
    }
}
