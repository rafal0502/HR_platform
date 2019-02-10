using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.EntityFramework;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class CompanyController : Controller
    {
        private readonly DataContext context;

        public CompanyController(DataContext dataContext)
        {
            context = dataContext;
        }
   

        public IActionResult Index()
        {
            List<Company> companies = context.Companies.ToList();
            return View(companies);
        }


        [HttpGet]
        public IActionResult Index([FromQuery(Name = "search")] string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
            {
                List<Company> companies = context.Companies.ToList();
                return View(companies);
            }
            List<Company> searchResult = context.Companies.Where(o => o.Name.Contains(searchString)).ToList();
            return View(searchResult);
        }


        public IActionResult Details(int id)
        {
            var offer = context.Companies.FirstOrDefault(o => o.Id == id);
            return View(offer);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }
            var forRemoving = context.Companies.Where(j => j.Id == id).ToList();
            foreach (var company in forRemoving)
            {
                context.Companies.Remove(company);
            }
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Company model)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            Company company = new Company
            {
                Name = model.Name
            };
            context.Companies.Add(company);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            var offer = context.Companies.Where(j => j.Id == id).ToList();
            if (!offer.Any()) return new StatusCodeResult((int)HttpStatusCode.NotFound);
            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Company model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var company = context.Companies.FirstOrDefault(j => j.Id == model.Id);
            company.Name = model.Name;
            await context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.Id });
        }

    }
}