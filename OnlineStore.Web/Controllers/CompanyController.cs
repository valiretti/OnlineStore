using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.Web.Models;

namespace OnlineStore.Web.Controllers
{
    public class CompanyController : Controller
    {
        IOrderService orderService;

        public CompanyController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public ActionResult AddCompany()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCompany(CompanyViewModel company)
        {
            if (ModelState.IsValid)
            {
                var companyDto = new CompanyDto
                {
                    Name = company.Name
                };

                var result = orderService.AddCompany(companyDto);
                if (result == "OK")
                {
                    return RedirectToAction("GetCompanies");
                }

                ModelState.AddModelError("", result);
            }

            return View(company);
        }

        public ActionResult GetCompanies()
        {
            IEnumerable<CompanyDto> companyDtos = orderService.GetCompanies();
            var mapper = new MapperConfiguration(c => c.CreateMap<CompanyDto, CompanyViewModel>()).CreateMapper();
            var companies = mapper.Map<IEnumerable<CompanyDto>, List<CompanyViewModel>>(companyDtos);
            return View(companies);
        }

        [HttpGet]
        public ActionResult DeleteCompany(int id)
        {
            CompanyDto companyDto = orderService.GetCompany(id);
            if (companyDto != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<CompanyDto, CompanyViewModel>()).CreateMapper();
                var company = mapper.Map<CompanyDto, CompanyViewModel>(companyDto);

                return View(company);
            }

            return RedirectToAction("GetCompanies");
        }

        [HttpPost]
        public ActionResult DeleteCompany(CompanyViewModel company)
        {
            orderService.DeleteCompany(company.Id);
            return RedirectToAction("GetCompanies");
        }

        public ActionResult GetCompaniesForCategory(int categoryId)
        {
            IEnumerable<CompanyDto> companyDtos = orderService.GetCertainCategoryCompanies(categoryId);

            var mapper = new MapperConfiguration(c => c.CreateMap<CompanyDto, CompanyViewModel>()).CreateMapper();
            var companies = mapper.Map<IEnumerable<CompanyDto>, List<CompanyViewModel>>(companyDtos);
            ViewBag.Category = categoryId;
            return PartialView("CompaniesForCategory", companies);
        }

        [HttpGet]
        public ActionResult EditCompany(int id)
        {
            CompanyDto companyDto = orderService.GetCompany(id);
            var mapper = new MapperConfiguration(c => c.CreateMap<CompanyDto, CompanyViewModel>()).CreateMapper();
            var company = mapper.Map<CompanyDto, CompanyViewModel>(companyDto);

            return View(company);
        }

        [HttpPost]
        public ActionResult EditCompany(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<CompanyViewModel, CompanyDto>()).CreateMapper();
                var company = mapper.Map<CompanyViewModel, CompanyDto>(model);

                orderService.EditCompany(company);
                return RedirectToAction("GetCompanies");
            }

            return View(model);
        }
    }
}