using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }
        [Required]
        [MaxLength(32)]
        public string CurrencyType { get; set; }
    }
}