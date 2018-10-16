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
                return RedirectToAction("GetPhones");
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
            return RedirectToAction("GetPhones");
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

               return View(company);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteCompany(CompanyViewModel company)
        {
            IEnumerable<PhoneDto> phoneDtos = orderService.GetCertainBrandPhones(company.Id);
            if (phoneDtos != null)
            {
                foreach (var phoneDto in phoneDtos)
                {
                    orderService.DeletePhone(phoneDto.Id);
                }
            }
            orderService.DeleteCompany(company.Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetPhonesForCart(string json)
        {
            if (json != null)
            {
                int[] items = JsonConvert.DeserializeObject<int[]>(json);
                if (items != null)
                {
                    PhoneDto[] phoneDtos = orderService.GetPhones(items);
                    var mapper = new MapperConfiguration(c => c.CreateMap<PhoneDto, PhoneViewModel>()).CreateMapper();
                    PhoneViewModel[] phones = mapper.Map<IEnumerable<PhoneDto>, PhoneViewModel[]>(phoneDtos);
                    return Json(phones, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new int[0], JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Cart()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Order()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Order(OrderViewModelForCart order)
        {
            if (ModelState.IsValid)
            {
                if (order.Items != null)
                {
                    List<LineItemViewModel> items = JsonConvert.DeserializeObject<List<LineItemViewModel>>(order.Items);

                    var orderDto = new OrderDto()
                    {
                        Address = order.Address,
                        Email = order.Email,
                        PhoneNumber = order.PhoneNumber,
                    };

                    List<LineItemDto> lineItemDtos = new List<LineItemDto>();
                    foreach (var item in items)
                    {
                        var lineItemDto = new LineItemDto()
                        {
                            Count = item.Count,
                            PhoneId = item.PhoneId,
                        };
                        lineItemDtos.Add(lineItemDto);
                    }

                    orderService.MakeOrder(orderDto, lineItemDtos);
                }
                return View("AcceptOrder");
            }
            return View();
        }

        //[Authorize(Roles = "Manager")]
        public ActionResult GetOrders()
        {
            IEnumerable<OrderDto> orderDtos = orderService.GetOrders();
            var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
            var orders = mapper.Map<IEnumerable<OrderDto>, List<OrderViewModel>>(orderDtos);
            return View(orders);
        }

        public ActionResult GetLineItems(int? id)
        {
            if (id != null)
            {
                IEnumerable<LineItemDto> lineItemDtosDtos = orderService.GetLineItemDtos(id.Value);
                var mapper = new MapperConfiguration(c => c.CreateMap<LineItemDto, LineItemViewModel>()
                    .ForMember(x => x.PhoneViewModel, s => s.MapFrom(t => t.PhoneDto))
                    .ForMember(t => t.OrderViewModel, s => s.MapFrom(x => x.OrderDto)).IgnoreAllPropertiesWithAnInaccessibleSetter()).CreateMapper();
                var lineItems = mapper.Map<IEnumerable<LineItemDto>, List<LineItemViewModel>>(lineItemDtosDtos);
                return View(lineItems);
            }
            return RedirectToAction("GetOrders");
        }

        [HttpGet]
        public ActionResult EditOrder(int? id)
        {
            if (id != null)
            {
                OrderDto orderDto = orderService.GetOrder(id.Value);
                if (orderDto != null)
                {
                    var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
                    var order = mapper.Map<OrderDto, OrderViewModel>(orderDto);

                    SelectList states = new SelectList(new []{"Canceled", "Сonfirmed", "Odered" });
                    ViewBag.States = states;

                    return View(order);
                }
            }
            return RedirectToAction("GetOrders");
        }

        [HttpPost]
        public ActionResult EditOrder(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<OrderViewModel, OrderDto>()).CreateMapper();
                var orderDto = mapper.Map<OrderViewModel, OrderDto>(orderViewModel);
                orderService.EditOrder(orderDto);
                return RedirectToAction("GetOrders");
            }

            SelectList states = new SelectList(new[] { "Canceled", "Сonfirmed", "Odered" });
            ViewBag.States = states;
            return View(orderViewModel);
        }

        [HttpGet]
        public ActionResult DeleteOrder(int? id)
        {
            if (id!=null)
            {
                OrderDto orderDto = orderService.GetOrder(id.Value);
                if (orderDto != null)
                {
                    var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
                    var order = mapper.Map<OrderDto, OrderViewModel>(orderDto);

                    return View(order);
                }
            }
            return RedirectToAction("GetOrders");
        }

        [HttpPost]
        public ActionResult DeleteOrder(OrderViewModel orderViewModel)
        {
            orderService.DeleteOrder(orderViewModel.Id);
            return RedirectToAction("GetOrders");
        }
    }
}