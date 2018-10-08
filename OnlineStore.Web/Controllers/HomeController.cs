using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Newtonsoft.Json;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.Web.Models;

namespace OnlineStore.Web.Controllers
{
    public class HomeController : Controller
    {
        IOrderService orderService;

        public HomeController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public ActionResult Index()
        {
            IEnumerable<CompanyDto> companyDtos = orderService.GetCompanies();
            var mapper = new MapperConfiguration(c => c.CreateMap<CompanyDto, CompanyViewModel>()).CreateMapper();
            var companies = mapper.Map<IEnumerable<CompanyDto>, List<CompanyViewModel>>(companyDtos);
            return View(companies);
        }

        public ActionResult GetPhones()
        {
            IEnumerable<PhoneDto> phoneDtos = orderService.GetPhones();
            var mapper = new MapperConfiguration(c => c.CreateMap<PhoneDto, PhoneViewModel>()).CreateMapper();
            var phones = mapper.Map<IEnumerable<PhoneDto>, List<PhoneViewModel>>(phoneDtos);
            return View(phones);
        }

        public ActionResult GetCertainBrendPhones(int id)
        {
            IEnumerable<PhoneDto> phoneDtos = orderService.GetCertainBrandPhones(id);
            var mapper = new MapperConfiguration(c => c.CreateMap<PhoneDto, PhoneViewModel>()).CreateMapper();
            var phones = mapper.Map<IEnumerable<PhoneDto>, List<PhoneViewModel>>(phoneDtos);
            return View("GetPhones", phones);
        }

        [HttpGet]
        //[Authorize(Roles = "Manager")]
        public ActionResult AddPhone()
        {
            SelectList companies = new SelectList(orderService.GetCompanies(), "Id", "Name");
            ViewBag.Companies = companies;
            return View();
        }

        [HttpPost]
        public ActionResult AddPhone(PhoneViewModel phone)
        {
            if (ModelState.IsValid)
            {
                var phoneDto = new PhoneDto
                {
                    Name = phone.Name,
                    PhoneDescription = phone.PhoneDescription,
                    CompanyId = int.Parse(phone.CompanyId),
                    Price = decimal.Parse(phone.Price)
                };
                orderService.AddPhone(phoneDto);
                return RedirectToAction("Index");
            }
            SelectList companies = new SelectList(orderService.GetCompanies(), "Id", "Name");
            ViewBag.Companies = companies;
            return View(phone);
        }

        [HttpGet]
        //[Authorize(Roles = "Manager")]
        public ActionResult AddCompany()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCompany(CompanyViewModel company)
        {
            if (ModelState.IsValid)
            {
                var companyDto = new CompanyDto()
                {
                    Name = company.Name
                };
                orderService.AddCompany(companyDto);
                return RedirectToAction("Index");
            }
            return View(company);
        }



        [HttpGet]
        //[Authorize(Roles = "Manager")]
        public ActionResult DeletePhone(int id)
        {
            PhoneDto phoneDto = orderService.GetPhone(id);
            var mapper = new MapperConfiguration(c => c.CreateMap<PhoneDto, PhoneViewModel>()).CreateMapper();
            var phone = mapper.Map<PhoneDto, PhoneViewModel>(phoneDto);
            return View(phone);
        }

        [HttpPost]
        public ActionResult DeletePhone(PhoneViewModel phone)
        {
            orderService.DeletePhone(phone.Id);
            return RedirectToAction("Index");
        }


        //[Authorize(Roles = "Manager")]
        [HttpGet]
        public ActionResult DeleteCompany(int id)
        {
            CompanyDto companyDto = orderService.GetCompany(id);
            if (companyDto != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<CompanyDto, CompanyViewModel>()).CreateMapper();
                var company = mapper.Map<CompanyDto, CompanyViewModel>(companyDto);

                IEnumerable<PhoneDto> phoneDtos = orderService.GetCertainBrandPhones(companyDto.Id);
                if (phoneDtos != null)
                {
                    foreach (var phoneDto in phoneDtos)
                    {
                        orderService.DeletePhone(phoneDto.Id);
                    }
                }
                return View(company);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteCompany(CompanyViewModel company)
        {
            orderService.DeleteCompany(company.Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetPhonesForCart(string json)
        {
            int[] items = JsonConvert.DeserializeObject<int[]>(json);
            PhoneViewModel[] phones = new PhoneViewModel[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                PhoneDto phoneDto = orderService.GetPhone(items[i]);
                var mapper = new MapperConfiguration(c => c.CreateMap<PhoneDto, PhoneViewModel>()).CreateMapper();
                var phone = mapper.Map<PhoneDto, PhoneViewModel>(phoneDto);
                phones[i] = phone;
            }

            return Json(phones, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Cart()
        {
            return View();
        }
    }
}