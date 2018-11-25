using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [MaxLength(256)]
        public string ProductTitle { get; set; }
        [Required]
        [MaxLength(2048)]
        public string ProductDescription { get; set; }
        [Required]
        public float Cost { get; set; }
    }
}