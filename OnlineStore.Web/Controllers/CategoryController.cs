using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.Web.Models;

namespace OnlineStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        IOrderService orderService;

        public CategoryController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

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
                    return RedirectToAction("Index", "Home");
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

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult DeleteCategory(CategoryViewModel category)
        {
            orderService.DeleteCategory(category.Id);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            CategoryDto categoryDto = orderService.GetCategory(id);
            var mapper = new MapperConfiguration(c => c.CreateMap<CategoryDto, CategoryViewModel>()).CreateMapper();
            var category = mapper.Map<CategoryDto, CategoryViewModel>(categoryDto);

            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<CategoryViewModel, CategoryDto>()).CreateMapper();
                var category = mapper.Map<CategoryViewModel, CategoryDto>(model);

                orderService.EditCategory(category);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}