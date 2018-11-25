using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class OrderStatus
    {
        [Key]
        public int OrderStatusId { get; set; }
        [MaxLength(32)]
        public string Status { get; set; }
    }
}