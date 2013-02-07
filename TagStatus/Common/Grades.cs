using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

//namespace TagStatus.Common
//{
    public class Grades
    {
        public static SelectList GradeSelectList
        {
            get 
            { 
                return new SelectList(GradeDictionary, "Value", "Key"); 
            }

        }

        public static readonly IDictionary<string, string>
                GradeDictionary = new Dictionary<string, string>
                { 
                  {"Choose...",""}
                , { "First Class", "A" }
                , { "Second Class", "B" }
                , { "Third Class", "c" }
                }; 
    }
//}

