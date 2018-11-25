using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class AccountType
    {
        [Key]
        public int AccountTypeId { get; set; }
        [MaxLength(32)]
        public string Type { get; set; }
    }
}