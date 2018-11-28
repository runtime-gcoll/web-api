using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwirlTheoryApi.Data.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }
        IEnumerable<OrderRow> OrderRows { get; set; }
    }
}