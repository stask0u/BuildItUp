using BuildItUp.Models.Entities;
using BuildItUp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CarsController : Controller
{
    private readonly ICarService _carService;
    private readonly IEngineService _engineService;
    private readonly ICompanyService _companyService;

    public CarsController(ICarService carService, IEngineService engineService, ICompanyService companyService)
    {
        _carService = carService;
        _engineService = engineService;
        _companyService = companyService;
    }

    // GET: Cars
    public async Task<IActionResult> Index()
    {
        var cars = await _carService.GetAllAsync();
        return View(cars);
    }

    // GET: Cars/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var car = await _carService.GetByIdAsync(id.Value);
        if (car == null)
        {
            return NotFound();
        }

        return View(car);
    }

    // GET: Cars/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.EngineId = new SelectList(await _engineService.GetAllAsync(), "Id", "EngineModel");
        ViewBag.CompanyId = new SelectList(await _companyService.GetAllAsync(), "Id", "Name");
        return View();
    }

    // POST: Cars/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Model,EngineId,CompanyId")] Car car)
    {
        if (!ModelState.IsValid)
        {
            var engine = await _engineService.GetByIdAsync(car.EngineId);
            if (engine == null)
            {
                ModelState.AddModelError("EngineId", "Selected engine does not exist.");
                ViewBag.EngineId = new SelectList(await _engineService.GetAllAsync(), "Id", "EngineModel", car.EngineId);
                ViewBag.CompanyId = new SelectList(await _companyService.GetAllAsync(), "Id", "Name", car.CompanyId);
                return View(car);
            }

            var company = await _companyService.GetByIdAsync(car.CompanyId);
            if (company == null)
            {
                ModelState.AddModelError("CompanyId", "Selected company does not exist.");
                ViewBag.EngineId = new SelectList(await _engineService.GetAllAsync(), "Id", "EngineModel", car.EngineId);
                ViewBag.CompanyId = new SelectList(await _companyService.GetAllAsync(), "Id", "Name", car.CompanyId);
                return View(car);
            }

            car.Engine = engine;
            car.Company = company;

            await _carService.AddAsync(car);

            return RedirectToAction(nameof(Index));
        }

        ViewBag.EngineId = new SelectList(await _engineService.GetAllAsync(), "Id", "EngineModel", car.EngineId);
        ViewBag.CompanyId = new SelectList(await _companyService.GetAllAsync(), "Id", "Name", car.CompanyId);
        return View(car);
    }

    // GET: Cars/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var car = await _carService.GetByIdAsync(id.Value);
        if (car == null)
        {
            return NotFound();
        }

        ViewBag.EngineId = new SelectList(await _engineService.GetAllAsync(), "Id", "EngineModel", car.EngineId);
        ViewBag.CompanyId = new SelectList(await _companyService.GetAllAsync(), "Id", "Name", car.CompanyId);
        return View(car);
    }

    // POST: Cars/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Model,EngineId,CompanyId")] Car car)
    {
        if (id != car.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            try
            {
                var engine = await _engineService.GetByIdAsync(car.EngineId);
                if (engine == null)
                {
                    ModelState.AddModelError("EngineId", "Selected engine does not exist.");
                    ViewBag.EngineId = new SelectList(await _engineService.GetAllAsync(), "Id", "EngineModel", car.EngineId);
                    ViewBag.CompanyId = new SelectList(await _companyService.GetAllAsync(), "Id", "Name", car.CompanyId);
                    return View(car);
                }

                var company = await _companyService.GetByIdAsync(car.CompanyId);
                if (company == null)
                {
                    ModelState.AddModelError("CompanyId", "Selected company does not exist.");
                    ViewBag.EngineId = new SelectList(await _engineService.GetAllAsync(), "Id", "EngineModel", car.EngineId);
                    ViewBag.CompanyId = new SelectList(await _companyService.GetAllAsync(), "Id", "Name", car.CompanyId);
                    return View(car);
                }

                car.Engine = engine;
                car.Company = company;

                await _carService.UpdateAsync(car);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _carService.CarExistsAsync(car.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.EngineId = new SelectList(await _engineService.GetAllAsync(), "Id", "EngineModel", car.EngineId);
        ViewBag.CompanyId = new SelectList(await _companyService.GetAllAsync(), "Id", "Name", car.CompanyId);
        return View(car);
    }

    // GET: Cars/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var car = await _carService.GetByIdAsync(id.Value);
        if (car == null)
        {
            return NotFound();
        }

        return View(car);
    }

    // POST: Cars/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _carService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
