//using Application.Interfaces;
//using AutoMapper;
//using DTOs;
//using Infrastructure.Interfaces;
//using Models;

//namespace Application.Services
//{
//    public class CategoryService : ICategoryService
//    {
//        private readonly ICategoryRepository _categoryRepository;
//        private readonly IMapper _mapper;

//        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
//        {
//            _categoryRepository = categoryRepository;
//            _mapper = mapper;
//        }

//        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
//        {
//            var categories = await _categoryRepository.GetAllAsync();
//            return _mapper.Map<List<CategoryDTO>>(categories);
//        }

//        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
//        {
//            var category = await _categoryRepository.GetByIdAsync(id);
//            return _mapper.Map<CategoryDTO>(category);
//        }

//        public async Task<CategoryDTO> CreateCategoryAsync(CategoryCreateDTO categoryDto)
//        {
//            var category = _mapper.Map<Category>(categoryDto);
//            category.CreatedAt = DateTime.UtcNow;
//            category.IsDeleted = false;

//            var createdCategory = await _categoryRepository.CreateAsync(category);
//            return _mapper.Map<CategoryDTO>(createdCategory);
//        }

//        public async Task<CategoryDTO> UpdateCategoryAsync(CategoryUpdateDTO categoryDto)
//        {
//            var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
//            if (existingCategory == null || existingCategory.IsDeleted)
//                throw new ArgumentException("Category not found");

//            existingCategory.Name = categoryDto.Name;
//            existingCategory.UpdateAt = DateTime.UtcNow;

//            var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);
//            return _mapper.Map<CategoryDTO>(updatedCategory);
//        }

//        public async Task DeleteCategoryAsync(int id)
//        {
//            if (await _categoryRepository.HasProductsAsync(id))
//                throw new InvalidOperationException("Cannot delete category with associated products");

//            await _categoryRepository.DeleteAsync(id);
//        }

//        public async Task<bool> CategoryExistsAsync(string name, int? excludeId = null)
//        {
//            return await _categoryRepository.ExistsAsync(name, excludeId);
//        }

//        public async Task<bool> CategoryHasProductsAsync(int id)
//        {
//            return await _categoryRepository.HasProductsAsync(id);
//        }

//        public async Task<List<string>> GetCategoryNamesAsync()
//        {
//            return await _categoryRepository.GetCategoryNamesAsync();
//        }
//    }
//}