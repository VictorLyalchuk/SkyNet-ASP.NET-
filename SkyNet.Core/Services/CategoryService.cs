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
        public async Task Create(CategoryDTO model)
        {
            await _categoryRepository.Insert(_mapper.Map<Category>(model));
            await _categoryRepository.Save();
        }
        public async Task Delete(int id)
        {
            var model = await Get(id);
            if (model == null) return;
            await _categoryRepository.Delete(id);
            await _categoryRepository.Save();
        }
        public async Task<CategoryDTO?> Get(int id)
        {
            if (id < 0) return null;
            var category = await _categoryRepository.GetByID(id);
            if (category == null) return null;
            return _mapper.Map<CategoryDTO?>(category);
        }
        public async Task<List<CategoryDTO>> GetAll()
        {
            var result = await _categoryRepository.GetAll();
            return _mapper.Map<List<CategoryDTO>>(result);
        }
        public async Task Update(CategoryDTO model)
        {
            await _categoryRepository.Update(_mapper.Map<Category>(model));
            await _categoryRepository.Save();
        }
    }
}