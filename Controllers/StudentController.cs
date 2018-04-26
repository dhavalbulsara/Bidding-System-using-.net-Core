using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication4.Controllers
{
    public class StudentController : Controller
    {
        // GET: /<controller>/
        
        studentDataAccessLayer objemployee = new studentDataAccessLayer();
        public IActionResult List()
        {
            List<student> lstEmployee = new List<student>();
            lstEmployee = objemployee.GetAllStudent().ToList();

            return View(lstEmployee);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] student stud)
        {
            if (ModelState.IsValid)
            {
                objemployee.AddEmployee(stud);
                return RedirectToAction("List");
            }
            return View(List());
        }
    }
}
