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

        [HttpGet]
        public ActionResult Cart()
        {
            return View();
        }
    }
}