using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TagStatus.Models;

namespace TagStatus.ViewModels
{
    public class StudentViewModel
    {
        public StudentModels Student { get; set; }
        public Grades Grades { get; set; }
        public StatesDictionary States { get; set; }

        public StudentViewModel(StudentModels student)
        {
            Student = student;
            Grades = new Grades();
            States = new StatesDictionary();
        }

        public StudentViewModel()
        {
            Student = new StudentModels(); ;
            Grades = new Grades();
            States = new StatesDictionary();
        }
    }
}