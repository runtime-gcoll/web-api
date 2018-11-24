using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class PaymentDetails
    {
        [Key]
        public int PaymentDetailsId { get; set; }
        [Required]
        [CreditCard]
        public int CardNumber { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public int SecurityCode { get; set; }
    }
}