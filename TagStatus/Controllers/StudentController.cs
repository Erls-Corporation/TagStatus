using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TagStatus.Models;
using TagStatus.ViewModels;

namespace TagStatus.Controllers
{
    public class StudentController : Controller
    {
        //public string StudentName { get; set; }
        //public int Grade { get; set; }

        //
        // GET: /Student/

        public ActionResult Index()
        {
            StudentModels stdm = new StudentModels();
            StudentViewModel stdvm = new StudentViewModel(stdm);
            return View(stdvm);
        }

        //[AllowAnonymous]
        //public ActionResult StudentInfo()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        [HttpPost]
        public ActionResult StudentInfo(StudentModels model)
        {
            if (ModelState.IsValid)
            {
                model.StudentName = model.State;
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult StudentInfo()
        {
            StudentViewModel model = new StudentViewModel();

                //model.StudentName = model.State;

            return View(model);
        }

    }
}
