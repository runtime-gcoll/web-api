using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class DisplayLanguage
    {
        [Key]
        public int DisplayLanguageId { get; set; }
        [Required]
        [MaxLength(8)]
        public string Language { get; set; }
    }
}