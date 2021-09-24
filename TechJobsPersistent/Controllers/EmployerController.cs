using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechJobsPersistent.Data;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobsPersistent.Controllers
{
    public class EmployerController : Controller
    {
        private JobDbContext _context;

        public EmployerController(JobDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Employer> employers =_context.Employers.ToList();
            return View();
        }

        public IActionResult Add()
        {
            AddEmployerViewModel addEmployerViewModel = new AddEmployerViewModel();
            return View(addEmployerViewModel);


        }

        public IActionResult
            ProcessAddEmployerForm(AddEmployerViewModel addEmployerViewModel)
        {
            if(ModelState.IsValid)
            {
                Employer employer = new Employer
                {
                    Name = addEmployerViewModel.Name,
                    Location = addEmployerViewModel.Location
                };

                _context.Employers.Add(employer);
                _context.SaveChanges();

                return Redirect("/Employer");
            }

            return View("~/Views/Employer/Add.cshtml", addEmployerViewModel);
        }

        public IActionResult About(int id)
        {
            Employer employer = _context.Employers.Find(id);
            
            return View(employer);
        }
    }
}
