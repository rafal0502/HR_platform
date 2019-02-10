using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.EntityFramework;
using WebApplication.Models;


namespace WebApplication.Controllers
{

    [Route("[controller]/[action]")]
    public class JobOfferController : Controller
    {

        private readonly DataContext context;



        public JobOfferController(DataContext dataContext)
        {
            context = dataContext;
        }

        public IActionResult Index()
        {
            return View(context.JobOffers.ToList());
        }

        [HttpGet]
        public IActionResult Index([FromQuery(Name = "search")] string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
                return View(context.JobOffers.Include("Company").ToList());

            List<JobOffer> searchResult = context.
                JobOffers.Include(offer => offer.Company).Where(o => o.JobTitle.Contains(searchString)).ToList();
            return View(searchResult);
        }

        public IActionResult Details(int id)
        {
            var offer = context.JobOffers.Include("Company").Include("JobApplications").FirstOrDefault(o => o.Id == id);
            return View(offer);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            var offer = context.JobOffers.Include("Company").FirstOrDefault(j => j.Id == id);
            if (offer == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid) return View();
            var offer = context.JobOffers.Include("Company").FirstOrDefault(j => j.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            offer.Description = model.Description;
            await context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.Id });
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            var forRemoving = context.JobOffers.Where(j => j.Id == id).ToList();

            foreach (var offer in forRemoving)
            {
                context.JobOffers.Remove(offer);
            }

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Create()
        {
            List<Company> companies = context.Companies.ToList();
            var model = new JobOfferCreateView
            {
                Companies = companies
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = context.Companies.ToList();
                return View(model);
            }
            JobOffer jo = new JobOffer
            {
                CompanyId = model.CompanyId,
                Company = context.Companies.FirstOrDefault(c => c.Id == model.CompanyId),
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            };
            context.JobOffers.Add(jo);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}