using DapperMvc.Models;
using DapperMvc.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

public class CompanyController : Controller
{
     readonly ICompanyRepository _companyRepository;

    public CompanyController(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    // GET: Company
    public async Task<IActionResult> Index()
    {
        var companies = await _companyRepository.GetCompanies();
        return View(companies);
    }

    // GET: Company/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = await _companyRepository.GetCompany(id);
        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    // GET: Company/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Company/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CompanyName,CompanyAddress,Country,GlassdoorRating")] Company company)
    {
        if (ModelState.IsValid)
        {
            await _companyRepository.CreateCompany(company);
            return RedirectToAction("Index");
        }
        return View(company);
    }

    // GET: Company/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = await _companyRepository.GetCompany(id);
        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    // POST: Company/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,CompanyAddress,Country,GlassdoorRating")] Company company)
    {
        if (id != company.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            await _companyRepository.UpdateCompany(id, company);
            return RedirectToAction("Index");
        }

        return View(company);
    }

    // GET: Company/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var company = await _companyRepository.GetCompany(id);
        if (company == null)
        {
            return NotFound();
        }

        return View(company);
    }

    // POST: Company/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _companyRepository.DeleteCompany(id);
        return RedirectToAction("Index");
    }
}
