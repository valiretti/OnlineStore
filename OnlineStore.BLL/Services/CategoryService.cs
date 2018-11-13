using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private IIdentityUnitOfWork DataBase { get; set; }

        public CategoryService(IIdentityUnitOfWork dataBase)
        {
            DataBase = dataBase;
        }

        public string AddCategory(CategoryDto categoryDto)
        {
            var cat = DataBase.Categories.GetAll().FirstOrDefault(c => c.Name == categoryDto.Name);
            if (cat != null)
            {
                return "Category already exists";
            }

            Category category = new Category()
            {
                Name = categoryDto.Name
            };
            DataBase.Categories.Create(category);
            DataBase.Save();

            return "OK";
        }

        public void EditCategory(CategoryDto categoryDto)
        {
            var category = DataBase.Categories.Get(categoryDto.Id);
            category.Name = categoryDto.Name;

            DataBase.Categories.Update(category);
            DataBase.Save();
        }

        public void DeleteCategory(int id)
        {
            var category = DataBase.Categories.Get(id);
            if (category != null)
            {
                var productDtos = GetCertainCategoryProducts(category.Id);
                if (productDtos != null)
                {
                    DeleteProducts(productDtos);
                    DataBase.Categories.Delete(category.Id);
                    DataBase.Save();
                }
            }
        }

        public IEnumerable<ProductDto> GetCertainCategoryProducts(int categoryId)
        {
            var category = DataBase.Categories.Get(categoryId);
            if (category != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<Product, ProductDto>()).CreateMapper();
                return mapper.Map<IEnumerable<Product>, List<ProductDto>>(DataBase.Products.GetAll().Where(t => t.Category.Id == categoryId));
            }
            return null;
        }



        private void DeleteProducts(IEnumerable<ProductDto> productDtos)
        {
            foreach (var productDto in productDtos)
            {
                if (productDto != null)
                {
                    DataBase.LineItems.Delete(productDto.Id);
                }
            }
        }

        public CategoryDto GetCategory(int id)
        {
            var category = DataBase.Categories.Get(id);
            if (category == null) return null;

            return new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Category, CategoryDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Category>, List<CategoryDto>>(DataBase.Categories.GetAll());
        }
    }
}
