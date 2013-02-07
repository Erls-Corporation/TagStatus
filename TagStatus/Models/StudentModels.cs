using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TagStatus.Models
{
    public class StudentModels
    {
        [Display(Name = "Student Name here")]
        [Required()]
        public string StudentName { get; set; }
        public string Grade { get; set; }
        public string State { get; set; }
    }
}