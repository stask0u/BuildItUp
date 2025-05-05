using Microsoft.AspNetCore.Mvc;
using BuildItUp.Services.Interfaces;
using BuildItUp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using BuildItUp.Data;

namespace BuildItUpWeb.Controllers
{
    public class CompanyController : Controller
    {

        private readonly ICompanyService _companyService;
        private readonly ApplicationDbContext _context;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        // GET: CompanyController
        public async Task<IActionResult> Index()
        {
            var companies = await _companyService.GetAllAsync();
            return View(companies);
        }

        // GET: CompanyController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: CompanyController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                await _companyService.AddAsync(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: CompanyController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null) return NotFound();
            return View(company);
        }

        // POST: CompanyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Company company)
        {
            if (id != company.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _companyService.UpdateAsync(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }


        // GET: CompanyController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null) return NotFound();
            return View(company);
        }

        // POST: CompanyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _companyService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
