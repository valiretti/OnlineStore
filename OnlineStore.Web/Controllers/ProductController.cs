using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.Web.Models;

namespace OnlineStore.Web.Controllers
{
    public class ProductController : Controller
    {
        IOrderService orderService;

        public ProductController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

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
    }
}