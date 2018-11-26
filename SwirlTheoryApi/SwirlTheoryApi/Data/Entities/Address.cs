using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        [Required]
        [MaxLength(256)]
        public string AddressLine1 { get; set; }
        [MaxLength(256)]
        public string AddressLine2 { get; set; }
        [Required]
        [MaxLength(16)]
        public string Postcode { get; set; }
    }
}