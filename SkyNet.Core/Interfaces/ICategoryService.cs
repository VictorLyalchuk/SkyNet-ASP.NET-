using SkyNet.Core.DTOs.Category;
using SkyNet.Core.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAll();
        Task<CategoryDTO?> Get(int id);
        Task Create(CategoryDTO model);
        Task Update(CategoryDTO model);
        Task Delete(int id);

    }
}
