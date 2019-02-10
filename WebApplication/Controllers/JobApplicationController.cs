using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApplication.EntityFramework;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly DataContext context;
        public JobApplicationController(DataContext dataContext)
        {
            context = dataContext;
        }



        public async Task<ActionResult> Apply(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            var model = new JobApplicationCreateView
            {
                OfferId = (int)id,
                JobOffer = context.JobOffers.FirstOrDefault(o => o.Id == id)
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Apply(JobApplicationCreateView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            JobApplication ja = new JobApplication
            {
                OfferId = model.OfferId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                Birthday = model.Birthday,
                ContactAgreement = model.ContactAgreement
            };

            context.JobApplications.Add(ja);
            JobOffer jo = context.JobOffers.FirstOrDefault(m => m.Id == model.OfferId);
            jo.JobApplications.Add(ja);
            await context.SaveChangesAsync();

            return RedirectToAction("Details", "JobOffer", new { id = model.OfferId });
        }

        public IActionResult Details(int id)
        {
            var application = context.JobApplications.FirstOrDefault(o => o.Id == id);
            return View(application);
        }
    }
}
