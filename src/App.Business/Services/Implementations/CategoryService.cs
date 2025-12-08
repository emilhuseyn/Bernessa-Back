using App.Business.DTOs.Categories;
using App.Business.Services.ExternalServices.Interfaces;
using App.Business.Services.Interfaces;
using App.Core.Entities;
using App.DAL.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Business.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFileManagerService _fileManagerService;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IFileManagerService fileManagerService,
            IMapper mapper,
            IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _fileManagerService = fileManagerService;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync(
                c => !c.IsDeleted,
                c => c.Translations
            );
            
            return categories.Select(c => MapToCategoryDTO(c));
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(
                c => c.Id == id,
                c => c.Translations
            );
            
            if (category == null)
            {
                throw new Exception("Kateqoriya tap?lmad?");
            }

            return MapToCategoryDTO(category);
        }

        public async Task<CategoryDTO> GetCategoryBySlugAsync(string slug)
        {
            var category = await _categoryRepository.GetBySlugAsync(slug);
            
            if (category == null)
            {
                throw new Exception("Kateqoriya tap?lmad?");
            }

            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO createCategoryDto)
        {
           

            string imageUrl = null;
            if (createCategoryDto.Image != null)
            {
                imageUrl = await _fileManagerService.UploadFileAsync(createCategoryDto.Image);
            }

            var category = new Category
            {
                Name = createCategoryDto.Name,
                Slug = createCategoryDto.Slug,
                Image = imageUrl,
                ProductCount = 0,
                Translations = new List<CategoryTranslation>()
            };
            
             if (!string.IsNullOrWhiteSpace(createCategoryDto.NameEn))
            {
                category.Translations.Add(new CategoryTranslation
                {
                    LanguageCode = "en",
                    Name = createCategoryDto.NameEn
                });
            }
            
             if (!string.IsNullOrWhiteSpace(createCategoryDto.NameRu))
            {
                category.Translations.Add(new CategoryTranslation
                {
                    LanguageCode = "ru",
                    Name = createCategoryDto.NameRu
                });
            }

            var createdCategory = await _categoryRepository.AddAsync(category);
            return MapToCategoryDTO(createdCategory);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(int id, CreateCategoryDTO updateCategoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(
                c => c.Id == id,
                c => c.Translations
            );
            
            if (category == null)
            {
                throw new Exception("Kateqoriya tap?lmad?");
            }

            

            category.Name = updateCategoryDto.Name;
            category.Slug = updateCategoryDto.Slug;
            
            if (updateCategoryDto.Image != null)
            {
                category.Image = await _fileManagerService.UploadFileAsync(updateCategoryDto.Image);
            }
            
            // Update translations
            category.Translations ??= new List<CategoryTranslation>();
            
            // Update or add English translation
            var enTranslation = category.Translations.FirstOrDefault(t => t.LanguageCode == "en");
            if (!string.IsNullOrWhiteSpace(updateCategoryDto.NameEn))
            {
                if (enTranslation != null)
                {
                    enTranslation.Name = updateCategoryDto.NameEn;
                }
                else
                {
                    category.Translations.Add(new CategoryTranslation
                    {
                        CategoryId = category.Id,
                        LanguageCode = "en",
                        Name = updateCategoryDto.NameEn
                    });
                }
            }
            else if (enTranslation != null)
            {
                category.Translations.Remove(enTranslation);
            }
            
            // Update or add Russian translation
            var ruTranslation = category.Translations.FirstOrDefault(t => t.LanguageCode == "ru");
            if (!string.IsNullOrWhiteSpace(updateCategoryDto.NameRu))
            {
                if (ruTranslation != null)
                {
                    ruTranslation.Name = updateCategoryDto.NameRu;
                }
                else
                {
                    category.Translations.Add(new CategoryTranslation
                    {
                        CategoryId = category.Id,
                        LanguageCode = "ru",
                        Name = updateCategoryDto.NameRu
                    });
                }
            }
            else if (ruTranslation != null)
            {
                category.Translations.Remove(ruTranslation);
            }

            var updatedCategory = await _categoryRepository.UpdateAsync(category);
            return MapToCategoryDTO(updatedCategory);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(c => c.Id == id);
            
            if (category == null)
            {
                throw new Exception("Kateqoriya tap?lmad?");
            }

            if (category.ProductCount > 0)
            {
                // delete associated products
                var products = await _productRepository.GetAllAsync(p => p.CategoryId == category.Id && !p.IsDeleted);
                foreach (var product in products.ToList())
                {
                    await _productRepository.DeleteAsync(product);
                }
                category.ProductCount = 0;
            }

            await _categoryRepository.DeleteAsync(category);
        }
        
        private CategoryDTO MapToCategoryDTO(Category category)
        {
            var dto = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Image = category.Image,
                ProductCount = category.ProductCount,
                Translations = new Dictionary<string, string>()
            };
            
            // Add default Azerbaijani
            dto.Translations["az"] = category.Name;
            
            // Add other translations
            if (category.Translations != null)
            {
                foreach (var translation in category.Translations)
                {
                    dto.Translations[translation.LanguageCode] = translation.Name;
                }
            }
            
            return dto;
        }
    }
}
