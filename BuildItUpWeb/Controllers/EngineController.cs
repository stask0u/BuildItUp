using BuildItUp.Models.Entities;
using BuildItUp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BuildItUpWeb.Controllers
{
    public class EngineController : Controller
    {
        private readonly IEngineService _engineService;
        private readonly ICompanyService _companyService;

        public EngineController(IEngineService engineService, ICompanyService companyService)
        {
            _engineService = engineService;
            _companyService = companyService;
        }

        // GET: Engine
        public async Task<IActionResult> Index()
        {
            var engines = await _engineService.GetAllAsync();
            return View(engines);
        }

        // GET: Engine/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var engine = await _engineService.GetByIdAsync(id);
            if (engine == null)
                return NotFound();

            return View(engine);
        }

        // GET: Engine/Create
        public async Task<IActionResult> Create()
        {
            var companies = await _companyService.GetAllAsync();
            ViewBag.Companies = new SelectList(companies, "Id", "Name");
            return View();
        }

        // POST: Engine/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Engine engine)
        {

            var company = await _companyService.GetByIdAsync(engine.CompanyId);
            if(company == null)
                    {
                ModelState.AddModelError("CompanyId", "Selected company does not exist.");
                var companies = await _companyService.GetAllAsync();
                ViewBag.Companies = new SelectList(companies, "Id", "Name", engine.CompanyId);
                return View(engine);
            }

            engine.Company = company;
            if (!ModelState.IsValid)
                {

                    await _engineService.AddAsync(engine);
                    return RedirectToAction(nameof(Index));
                }

                var allCompanies = await _companyService.GetAllAsync();
                ViewBag.Companies = new SelectList(allCompanies, "Id", "Name", engine.CompanyId);
                return View(engine);
        }

        // GET: Engine/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var engine = await _engineService.GetByIdAsync(id);
            if (engine == null) return NotFound();

            var companies = await _companyService.GetAllAsync();
            ViewBag.Companies = new SelectList(companies, "Id", "Name", engine.CompanyId);

            return View(engine);
        }

        // POST: Engine/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Engine engine)
        {
            if (id != engine.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await _engineService.UpdateAsync(engine);

                var companies = await _companyService.GetAllAsync();
                ViewBag.Companies = new SelectList(companies, "Id", "Name", engine.CompanyId);
                return RedirectToAction(nameof(Index));
            }else return View(engine);
        }

        // GET: Engine/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var engine = await _engineService.GetByIdAsync(id);
            if (engine == null) return NotFound();
            return View(engine);
        }

        // POST: Engine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _engineService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
