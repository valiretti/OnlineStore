using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
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
            IEnumerable<CategoryDto> categoryDtos = orderService.GetCategories();
            var mapper = new MapperConfiguration(c => c.CreateMap<CategoryDto, CategoryViewModel>()).CreateMapper();
            var categories = mapper.Map<IEnumerable<CategoryDto>, List<CategoryViewModel>>(categoryDtos);

            return View(categories);
        }

        #region Methods for products
        public ActionResult GetProducts()
        {
            IEnumerable<ProductDto> productDtos = orderService.GetProducts();
            var mapper = new MapperConfiguration(c => c.CreateMap<ProductDto, ProductViewModel>()).CreateMapper();
            var products = mapper.Map<IEnumerable<ProductDto>, List<ProductViewModel>>(productDtos);
            return View(products);
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            SelectList companies = new SelectList(orderService.GetCompanies(), "Id", "Name");
            SelectList categories = new SelectList(orderService.GetCategories(), "Id", "Name");
            ViewBag.Categories = categories;
            ViewBag.Companies = companies;
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(ProductViewModel product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                string imagePath = null;
                if (image != null)
                {
                    imagePath = $"{Guid.NewGuid():N}{Path.GetExtension(image.FileName)}";
                    image.SaveAs(Server.MapPath("~/Files/Images/" + imagePath));
                }

                var productDto = new ProductDto
                {
                    Name = product.Name,
                    ProductDescription = product.ProductDescription,
                    CompanyId = int.Parse(product.CompanyId),
                    CategoryId = int.Parse(product.CategoryId),
                    Price = decimal.Parse(product.Price),
                    ImagePath = imagePath
                };

                orderService.AddProduct(productDto);
                return RedirectToAction("GetProducts");
            }

            SelectList companies = new SelectList(orderService.GetCompanies(), "Id", "Name");
            SelectList categories = new SelectList(orderService.GetCategories(), "Id", "Name");
            ViewBag.Categories = categories;
            ViewBag.Companies = companies;
            return View(product);
        }

        public ActionResult GetCertainBrendProducts(int id, int categoryId)
        {
            IEnumerable<ProductDto> productDtos = orderService.GetCertainBrandProducts(id, categoryId);
            var mapper = new MapperConfiguration(c => c.CreateMap<ProductDto, ProductViewModel>()).CreateMapper();
            var products = mapper.Map<IEnumerable<ProductDto>, List<ProductViewModel>>(productDtos);
            return View("GetProducts", products);
        }

        public ActionResult GetCertainCategoryProducts(int id)
        {
            IEnumerable<ProductDto> productDtos = orderService.GetCertainCategoryProducts(id);
            var mapper = new MapperConfiguration(c => c.CreateMap<ProductDto, ProductViewModel>()).CreateMapper();
            var products = mapper.Map<IEnumerable<ProductDto>, List<ProductViewModel>>(productDtos);
            return View("GetProducts", products);
        }

        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            ProductDto productDto = orderService.GetProduct(id);
            var mapper = new MapperConfiguration(c => c.CreateMap<ProductDto, ProductViewModel>()).CreateMapper();
            var product = mapper.Map<ProductDto, ProductViewModel>(productDto);
            return View(product);
        }

        [HttpPost]
        public ActionResult DeleteProduct(ProductViewModel product)
        {
            orderService.DeleteProduct(product.Id);
            return RedirectToAction("GetProducts");
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetProductsForCart(string json)
        {
            if (json != null)
            {
                int[] items = JsonConvert.DeserializeObject<int[]>(json);
                if (items != null)
                {
                    ProductDto[] productDtos = orderService.GetProducts(items);
                    var mapper = new MapperConfiguration(c => c.CreateMap<ProductDto, ProductViewModel>()).CreateMapper();
                    ProductViewModel[] products = mapper.Map<IEnumerable<ProductDto>, ProductViewModel[]>(productDtos);
                    return Json(products, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new int[0], JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Methods for companies
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
            var categories = mapper.Map<IEnumerable<CompanyDto>, List<CompanyViewModel>>(companyDtos);
            return View(categories);
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
        #endregion

        #region Methods for categories
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                var categoryDto = new CategoryDto()
                {
                    Name = category.Name
                };

                var result = orderService.AddCategory(categoryDto);
                if (result == "OK")
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", result);
            }

            return View(category);
        }

        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            CategoryDto categoryDto = orderService.GetCategory(id);
            if (categoryDto != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<CategoryDto, CategoryViewModel>()).CreateMapper();
                var category = mapper.Map<CategoryDto, CategoryViewModel>(categoryDto);

                return View(category);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteCategory(CategoryViewModel category)
        {
            orderService.DeleteCategory(category.Id);
            return RedirectToAction("Index");
        }
        #endregion



        

        [HttpGet]
        public ActionResult Cart()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Order()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                UserDto userDto = orderService.GetUserData(userId);
                if (userDto != null)
                {
                    var orderViewModel = new OrderViewModelForCart()
                    {
                        Name = userDto.Name,
                        Address = userDto.Address,
                        Email = userDto.Email,
                        PhoneNumber = userDto.PhoneNumber
                    };
                    return View(orderViewModel);
                }
            }
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

                    List<LineItemDto> lineItemDtos = new List<LineItemDto>();

                    foreach (var item in items)
                    {
                        var lineItemDto = new LineItemDto()
                        {
                            Count = item.Count,
                            ProductId = item.ProductId,
                        };
                        lineItemDtos.Add(lineItemDto);
                    }

                    var orderDto = new OrderDto
                    {
                        Name = order.Name,
                        Address = order.Address,
                        Email = order.Email,
                        PhoneNumber = order.PhoneNumber,
                    };

                    string userId = User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null;

                    orderService.MakeOrder(orderDto, lineItemDtos, userId);
                }

                return View("AcceptOrder");
            }

            return View();
        }

        public ActionResult GetOrders()
        {
            IEnumerable<OrderDto> orderDtos = orderService.GetOrders();
            var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
            var orders = mapper.Map<IEnumerable<OrderDto>, List<OrderViewModel>>(orderDtos);
            return View(orders);
        }

        public ActionResult GetOrdersForUser()
        {
            var userId = User.Identity.GetUserId();
            IEnumerable<OrderDto> orderDtos = orderService.GetOrdersForUser(userId);
            var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
            var orders = mapper.Map<IEnumerable<OrderDto>, List<OrderViewModel>>(orderDtos);
            return View("GetOrders", orders);
        }


        public ActionResult GetLineItems(int? id)
        {
            if (id != null)
            {
                IEnumerable<LineItemDto> lineItemDtosDtos = orderService.GetLineItemDtos(id.Value);
                var mapper = new MapperConfiguration(c => c.CreateMap<LineItemDto, LineItemViewModel>()
                    .ForMember(x => x.ProductViewModel, s => s.MapFrom(t => t.ProductDto))
                    .ForMember(t => t.OrderViewModel, s => s.MapFrom(x => x.OrderDto))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter()).CreateMapper();
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

                    SelectList states = new SelectList(new[] { "Сonfirmed", "Odered", "Canceled" });
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

            SelectList states = new SelectList(new[] { "Сonfirmed", "Odered", "Canceled" });
            ViewBag.States = states;
            return View(orderViewModel);
        }

        [HttpGet]
        public ActionResult EditOrderForUser(int? id)
        {
            if (id != null)
            {
                OrderDto orderDto = orderService.GetOrder(id.Value);

                if (orderDto != null)
                {
                    if (orderDto.State == "Сonfirmed")
                    {
                        return View("OrderConfirmed");
                    }
                    if (orderDto.State == "Canceled")
                    {
                        return View("OrderCanceled");
                    }

                    var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
                    var order = mapper.Map<OrderDto, OrderViewModel>(orderDto);

                    SelectList states = new SelectList(new[] { "Odered", "Canceled" });
                    ViewBag.States = states;

                    return View("EditOrder", order);
                }
            }
            return RedirectToAction("GetOrdersForUser");
        }

        [HttpPost]
        public ActionResult EditOrderForUser(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<OrderViewModel, OrderDto>()).CreateMapper();
                var orderDto = mapper.Map<OrderViewModel, OrderDto>(orderViewModel);

                orderService.EditOrder(orderDto);
                return RedirectToAction("GetOrdersForUser");
            }

            SelectList states = new SelectList(new[] { "Odered", "Canceled" });
            ViewBag.States = states;
            return View("EditOrder", orderViewModel);
        }

        [HttpGet]
        public ActionResult DeleteOrder(int? id)
        {
            if (id != null)
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

        [HttpGet]
        public ActionResult DeleteOrderForUser(int? id)
        {
            if (id != null)
            {
                OrderDto orderDto = orderService.GetOrder(id.Value);
                if (orderDto != null)
                {
                    if (orderDto.State == "Сonfirmed")
                    {
                        return View("OrderConfirmed");
                    }

                    var mapper = new MapperConfiguration(c => c.CreateMap<OrderDto, OrderViewModel>()).CreateMapper();
                    var order = mapper.Map<OrderDto, OrderViewModel>(orderDto);

                    return View("DeleteOrder", order);
                }
            }
            return RedirectToAction("GetOrdersForUser");
        }

        [HttpPost]
        public ActionResult DeleteOrderForUser(OrderViewModel orderViewModel)
        {
            orderService.DeleteOrder(orderViewModel.Id);
            return RedirectToAction("GetOrdersForUser");
        }

        [HttpGet]
        public ActionResult DeleteLineItem(int? id)
        {
            if (id != null)
            {
                LineItemDto lineItemDto = orderService.GetLineItemDto(id.Value);
                if (lineItemDto != null)
                {
                    var mapper = new MapperConfiguration(c => c.CreateMap<LineItemDto, LineItemViewModel>()
                            .ForMember(x => x.ProductViewModel, s => s.MapFrom(t => t.ProductDto))).CreateMapper();
                    var lineItem = mapper.Map<LineItemDto, LineItemViewModel>(lineItemDto);

                    return View(lineItem);
                }
            }
            return RedirectToAction("GetOrders");
        }

        [HttpPost]
        public ActionResult DeleteLineItem(LineItemViewModel lineItemViewModel)
        {
            orderService.DeleteLineItem(lineItemViewModel.Id);
            return RedirectToAction("GetOrders");
        }
    }
}