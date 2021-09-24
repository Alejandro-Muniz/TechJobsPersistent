using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            List<Employer> employers = context.Employers.ToList();
            List<Skill> skills = context.Skills.ToList();

            AddJobViewModel addJobViewModel = new 
                AddJobViewModel(employers, skills);

            return View(addJobViewModel);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel
            addJobViewModel, string[] selectedSkills)
        {
            Console.WriteLine(addJobViewModel.EmployerId);
            Employer theEmployer = context.Employers.Find(addJobViewModel.EmployerId);
            if(ModelState.IsValid)
            {
                Job newJob = new Job
                {
                    Name = addJobViewModel.Name,
                    EmployerId = addJobViewModel.EmployerId,
                    Employer = theEmployer
                };
                foreach(string skill in selectedSkills)
                {
                    JobSkill newJobSkill = new JobSkill
                    {
                        Job = newJob,
                        JobId = newJob.Id,
                        SkillId = Int32.Parse(skill)
                    };

                    context.JobSkills.Add(newJobSkill);
                }

                context.Jobs.Add(newJob);
                context.SaveChanges();

                return Redirect("Index");
            }
            return View(addJobViewModel);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel newAddJobViewModel, string[] selectedSkills)
        {
            Employer theEmployer = _context.Employers.Find(newAddJobViewModel.EmployerId);

            // ...The if statement would be here...

            // We create a new List<Skill>
            List<Skill> skills = _context.Skills.ToList();

            // We then add that list of skills to the new instance of the ViewModel
            newAddJobViewModel.Skills = skills;

            // We then create a new List<Employer>
            List<Employer> employers = _context.Employers.ToList();

            // And we utilize the newly created method within the ViewModel to add the employers to the class
            newAddJobViewModel.createSelectListItems(employers);

            // Passing the ViewModel back to the AddJobView
            return View("~/Views/Home/AddJob.cshtml", newAddJobViewModel);
        }
    }
}
