using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class BasketRow
    {
        public int BasketRowId { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}