using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwirlTheoryApi.Data.Entities
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
        [Url]
        public string ImageUrl { get; set; }
        [Required]
        public float Cost { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}