using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.BLL.Services
{
    public class ProductService :IProductService
    {
        private IIdentityUnitOfWork DataBase { get; set; }

        public ProductService(IIdentityUnitOfWork dataBase)
        {
            DataBase = dataBase;
        }

        public void AddProduct(ProductDto productDto)
        {
            Company company = DataBase.Companies.Get(productDto.CompanyId);
            Category category = DataBase.Categories.Get(productDto.CategoryId);
            if (company != null && category != null)
            {
                Product product = new Product
                {
                    Name = productDto.Name,
                    CompanyId = company.Id,
                    CategoryId = category.Id,
                    ProductDescription = productDto.ProductDescription,
                    Price = productDto.Price,
                    ImagePath = productDto.ImagePath

                };
                DataBase.Products.Create(product);
                DataBase.Save();
            }
        }
        public void DeleteProduct(int id)
        {
            var product = DataBase.Products.Get(id);
            if (product != null)
            {
                DataBase.Products.Delete(product.Id);
                DataBase.Save();
            }
        }

        public ProductDto GetProduct(int? id)
        {
            if (id != null)
            {
                var phone = DataBase.Products.Get(id.Value);
                if (phone != null)
                {
                    return new ProductDto { Name = phone.Name, CompanyId = phone.CompanyId, ProductDescription = phone.ProductDescription, Price = phone.Price, Id = phone.Id };
                }

                return null;
            }
            return null;
        }
        public ProductDto[] GetProducts(int[] ids)
        {
            var phones = DataBase.Products.Find(t => ids.Contains(t.Id)).ToList();
            var mapper = new MapperConfiguration(c => c.CreateMap<Product, ProductDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Product>, ProductDto[]>(phones);
        }

        public IEnumerable<ProductDto> GetProducts()
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Product, ProductDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Product>, List<ProductDto>>(DataBase.Products.GetAll());
        }

        public IEnumerable<ProductDto> GetCertainBrandProducts(int? companyId, int categoryId)
        {
            if (companyId != null)
            {
                var company = DataBase.Companies.Get(companyId.Value);
                if (company != null)
                {
                    var mapper = new MapperConfiguration(c => c.CreateMap<Product, ProductDto>()).CreateMapper();
                    return mapper.Map<IEnumerable<Product>, List<ProductDto>>(DataBase.Products.GetAll().Where(t => t.CompanyId == companyId).Where(t => t.CategoryId == categoryId));
                }
            }
            return null;
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

       
        
    }
}
