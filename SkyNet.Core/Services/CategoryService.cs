using AutoMapper;
using SkyNet.Core.DTOs.Category;
using SkyNet.Core.Entities.Site;
using SkyNet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyNet.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category> _categoryRepository;
        public CategoryService(IMapper mapper, IRepository<Category> categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryDTO>> GetAll()
        {
            var result = await _categoryRepository.GetAll();
            return _mapper.Map<List<CategoryDTO>>(result);
        }
    }
}