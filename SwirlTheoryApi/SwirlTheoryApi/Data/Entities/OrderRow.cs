using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace SwirlTheoryApi.Models
{
    public class OrderRow
    {
        [Key]
        public int OrderRowId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
