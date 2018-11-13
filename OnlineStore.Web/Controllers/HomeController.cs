using System.Collections.Generic;
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
        IProductService productService;
        ICategoryService categoryService;

        public HomeController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        public ActionResult Index()
        {
            IEnumerable<CategoryDto> categoryDtos = categoryService.GetCategories();
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
                    ProductDto[] productDtos = productService.GetProducts(items);
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