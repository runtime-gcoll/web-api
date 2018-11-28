using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwirlTheoryApi.Data.Entities
{
    public enum OrderStatus {
        Ordered,
        Shipped,
        Delivered
    }

    /*
    public class OrderStatus
    {
        [Key]
        public int OrderStatusId { get; set; }
        [MaxLength(32)]
        public EOrderStatus Status { get; set; }
    }
    */
}