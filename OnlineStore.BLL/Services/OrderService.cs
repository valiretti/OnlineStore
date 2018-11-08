using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.BLL.Services
{
    public class OrderService : IOrderService
    {
        private IIdentityUnitOfWork DataBase { get; set; }

        public OrderService(IIdentityUnitOfWork dataBase)
        {
            DataBase = dataBase;
        }

        #region Methods for products
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

        private IEnumerable<ProductDto> GetCertainBrandProducts(int companyId)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Product, ProductDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Product>, List<ProductDto>>(DataBase.Products.GetAll()
                .Where(t => t.CompanyId == companyId));
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
        #endregion

        #region Methods for categories
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
        #endregion

        #region Methods for companies
        public string AddCompany(CompanyDto companyDto)
        {
            var comp = DataBase.Companies.GetAll().FirstOrDefault(c => c.Name == companyDto.Name);
            if (comp != null)
            {
                return "Company already exists";
            }

            Company company = new Company
            {
                Name = companyDto.Name
            };

            DataBase.Companies.Create(company);
            DataBase.Save();

            return "OK";
        }

        public void EditCompany(CompanyDto companyDto)
        {
            var company = DataBase.Companies.Get(companyDto.Id);
            company.Name = companyDto.Name;

            DataBase.Companies.Update(company);
            DataBase.Save();
        }

        public IEnumerable<CompanyDto> GetCompanies()
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Company, CompanyDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Company>, List<CompanyDto>>(DataBase.Companies.GetAll());
        }

        public CompanyDto GetCompany(int id)
        {
            var company = DataBase.Companies.Get(id);
            if (company != null)
            {
                return new CompanyDto { Id = company.Id, Name = company.Name };
            }

            return null;
        }

        public void DeleteCompany(int id)
        {
            var company = DataBase.Companies.Get(id);
            if (company != null)
            {
                var productDtos = GetCertainBrandProducts(company.Id);
                if (productDtos != null)
                {
                    DeleteProducts(productDtos);
                    DataBase.Companies.Delete(company.Id);
                    DataBase.Save();
                }
            }
        }

        public IEnumerable<CompanyDto> GetCertainCategoryCompanies(int categoryId)
        {
            var category = DataBase.Categories.Get(categoryId);
            if (category == null) return null;

            var companies = DataBase.Companies
                .Find(c => c.Products.Any(p => p.CategoryId == categoryId));
            if (companies == null) return null;

            var mapper = new MapperConfiguration(c => c.CreateMap<Company, CompanyDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Company>, List<CompanyDto>>(companies);


        }


        #endregion
     
        #region Methods for orders
        public void MakeOrder(OrderDto orderDto, IEnumerable<LineItemDto> lineItemDtos, string userId)
        {
            if (orderDto != null && lineItemDtos != null)
            {
                Order order = new Order
                {
                    Address = orderDto.Address,
                    Date = DateTime.Now,
                    Email = orderDto.Email,
                    PhoneNumber = orderDto.PhoneNumber,
                    State = State.Оrdered,
                    Name = orderDto.Name,
                    ClientProfileId = userId
                };
                DataBase.Orders.Create(order);

                foreach (var lineItemDto in lineItemDtos)
                {
                    LineItem lineItem = new LineItem()
                    {
                        Count = lineItemDto.Count,
                        ProductId = lineItemDto.ProductId,
                        Order = order
                    };
                    DataBase.LineItems.Create(lineItem);
                }

                DataBase.Save();
            }
        }

        public IEnumerable<OrderDto> GetOrders()
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Order, OrderDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Order>, List<OrderDto>>(DataBase.Orders.GetAll());
        }

        public IEnumerable<OrderDto> GetOrdersForUser(string userId)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Order, OrderDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Order>, List<OrderDto>>(DataBase.Orders.GetAll().Where(t => t.ClientProfileId == userId));
        }

        public OrderDto GetOrder(int id)
        {

            var order = DataBase.Orders.Get(id);
            if (order != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<Order, OrderDto>()).CreateMapper();
                return mapper.Map<Order, OrderDto>(order);
            }
            return null;
        }

        public void EditOrder(OrderDto orderDto)
        {
            var order = new Order()
            {
                Id = orderDto.Id,
                Name = orderDto.Name,
                Address = orderDto.Address,
                Date = DateTime.Now,
                Email = orderDto.Email,
                PhoneNumber = orderDto.PhoneNumber,
                ClientProfileId = orderDto.ClientProfileId,
                State = orderDto.State == "Canceled" ? State.Canceled : orderDto.State == "Сonfirmed" ? State.Сonfirmed : State.Оrdered
            };
            DataBase.Orders.Update(order);
            DataBase.Save();
        }

        public void DeleteOrder(int id)
        {
            var order = DataBase.Orders.Get(id);
            if (order != null)
            {
                IEnumerable<LineItemDto> lineItemDtos = GetLineItemDtos(order.Id);
                if (lineItemDtos != null)
                {
                    DeleteLineItems(lineItemDtos);
                    DataBase.Orders.Delete(order.Id);
                    DataBase.Save();
                }
            }
        }

        private void DeleteLineItems(IEnumerable<LineItemDto> lineItemDtos)
        {
            foreach (var lineItemDto in lineItemDtos)
            {
                if (lineItemDto != null)
                {
                    DataBase.LineItems.Delete(lineItemDto.Id);
                }
            }
        }


        public IEnumerable<LineItemDto> GetLineItemDtos(int orderId)
        {
            var order = DataBase.Orders.Get(orderId);
            if (order != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<LineItem, LineItemDto>()
                    .ForMember(x => x.OrderDto, s => s.MapFrom(t => t.Order)).
                    ForMember(x => x.ProductDto, s => s.MapFrom(t => t.Product))).CreateMapper();

                return mapper.Map<IEnumerable<LineItem>, List<LineItemDto>>(DataBase.LineItems.GetAll().Where(t => t.OrderId == order.Id));
            }
            return null;
        }

        public LineItemDto GetLineItemDto(int id)
        {
            var lineItem = DataBase.LineItems.Get(id);
            if (lineItem != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<LineItem, LineItemDto>()
                    .ForMember(x => x.OrderDto, s => s.MapFrom(t => t.Order)).
                    ForMember(x => x.ProductDto, s => s.MapFrom(t => t.Product))).CreateMapper();

                return mapper.Map<LineItem, LineItemDto>(DataBase.LineItems.Get(id));
            }
            return null;
        }

        public void DeleteLineItem(int id)
        {
            var lineItem = DataBase.LineItems.Get(id);
            if (lineItem != null)
            {
                DataBase.LineItems.Delete(lineItem.Id);
                DataBase.Save();
            }
        }
        #endregion

        public UserDto GetUserData(string id)
        {
            var user = DataBase.ClientManager.GetClientProfile(id);
            if (user != null)
            {
                var userDto = new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                };
                return userDto;
            }

            return null;
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
