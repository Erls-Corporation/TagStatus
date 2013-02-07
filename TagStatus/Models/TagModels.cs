using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TagStatus.Models
{
    public class TagModel
    {
        [Display(Name = "Enter Tag Number")]
        [Required(ErrorMessage = "The Tag Number field is required.")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}