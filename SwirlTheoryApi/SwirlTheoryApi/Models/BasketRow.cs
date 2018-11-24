using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class BasketRow
    {
        [Key]
        public int BasketRowId { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Product Product { get; set; }
    }
}