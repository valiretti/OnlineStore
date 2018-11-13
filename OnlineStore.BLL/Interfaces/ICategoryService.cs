using System.Collections.Generic;
using OnlineStore.BLL.DTO;

namespace OnlineStore.BLL.Interfaces
{
    public interface ICategoryService
    {
        string AddCategory(CategoryDto category);
        void EditCategory(CategoryDto category);
        void DeleteCategory(int id);
        CategoryDto GetCategory(int id);
        IEnumerable<CategoryDto> GetCategories();
    }
}
