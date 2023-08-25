using Microsoft.AspNetCore.Mvc;
using SkyNet.Core.DTOs.Category;
using SkyNet.Core.Interfaces;

namespace SkyNet.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> LoadCategories()
        {
            var result = await _categoryService.GetAll();
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategories(int id)
        {
            await _categoryService.Delete(id);
            return RedirectToAction(nameof(LoadCategories));
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryDTO model)
        {
            await _categoryService.Create(model);
            return RedirectToAction(nameof(LoadCategories));
        }
        public IActionResult EditCategory(int id)
        {
            var result = _categoryService.Get(id);
            if (result == null)
            {
                return NotFound(); 
            }
            return View(result.Result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(CategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _categoryService.Update(model);
            return RedirectToAction(nameof(LoadCategories));
        }
    }
}
