using App.Business.DTOs.Products;
using App.Business.Services.ExternalServices.Interfaces;
using App.Business.Services.Interfaces;
using App.Core.Entities;
using App.DAL.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Business.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileManagerService _fileManagerService;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IFileManagerService fileManagerService,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _fileManagerService = fileManagerService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync(
                p => p.IsActive && !p.IsDeleted,
                p => p.Category,
                p => p.Translations
            );
            return products.Select(p => MapToDTO(p));
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(
                p => p.Id == id,
                p => p.Category,
                p => p.Translations
            );
            
            if (product == null)
            {
                throw new Exception("Məhsul tapılmadı");
            }

            return MapToDTO(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetFeaturedProductsAsync()
        {
            var products = await _productRepository.GetFeaturedProductsAsync();
            return products.Select(p => MapToDTO(p));
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategorySlugAsync(string slug)
        {
            var products = await _productRepository.GetByCategorySlugAsync(slug);
            return products.Select(p => MapToDTO(p));
        }

        public async Task<IEnumerable<ProductDTO>> GetRelatedProductsAsync(int productId, int limit = 8)
        {
            var product = await _productRepository.GetByIdAsync(p => p.Id == productId);
            
            if (product == null)
            {
                throw new Exception("Məhsul tapılmadı");
            }

            var relatedProducts = await _productRepository.GetAllAsync(
                p => p.CategoryId == product.CategoryId && 
                     p.Id != productId && 
                     p.IsActive && 
                     !p.IsDeleted,
                p => p.Category,
                p => p.Translations
            );

            return relatedProducts
                .OrderByDescending(p => p.IsFeatured)
                
                .ThenByDescending(p => p.CreatedOn)
                .Take(limit)
                .Select(p => MapToDTO(p));
        }

        public async Task<IEnumerable<ProductDTO>> SearchProductsAsync(string query)
        {
            var products = await _productRepository.SearchProductsAsync(query);
            return products.Select(p => MapToDTO(p));
        }

        public async Task<IEnumerable<ProductDTO>> GetDealsAsync()
        {
            var products = await _productRepository.GetDealsAsync();
            return products.Select(p => MapToDTO(p));
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO createProductDto)
        {
            var category = await _categoryRepository.GetByIdAsync(c => c.Id == createProductDto.CategoryId);
            if (category == null)
            {
                throw new Exception("Kateqoriya tapılmadı");
            }

            var imageUrls = new List<string>();
            if (createProductDto.Images != null && createProductDto.Images.Any())
            {
                foreach (var image in createProductDto.Images)
                {
                    var imageUrl = await _fileManagerService.UploadFileAsync(image);
                    imageUrls.Add(imageUrl);
                }
            }

            var product = new Product
            {
                Name = createProductDto.Name,
                Brand = createProductDto.Brand,
                Price = createProductDto.Price,
                OriginalPrice = createProductDto.OriginalPrice,
                Volume = createProductDto.Volume,
                Type = createProductDto.Type,
                Description = createProductDto.Description,
                Images = JsonConvert.SerializeObject(imageUrls),
                CategoryId = createProductDto.CategoryId,
                 IsActive = createProductDto.IsActive,
                IsFeatured = createProductDto.IsFeatured,
                Translations = new List<ProductTranslation>()
            };
            
            // Add English translation if provided
            if (!string.IsNullOrWhiteSpace(createProductDto.NameEn))
            {
                product.Translations.Add(new ProductTranslation
                {
                    LanguageCode = "en",
                    Name = createProductDto.NameEn,
                    Type = createProductDto.TypeEn,
                    Description = createProductDto.DescriptionEn
                });
            }
            
            // Add Russian translation if provided
            if (!string.IsNullOrWhiteSpace(createProductDto.NameRu))
            {
                product.Translations.Add(new ProductTranslation
                {
                    LanguageCode = "ru",
                    Name = createProductDto.NameRu,
                    Type = createProductDto.TypeRu,
                    Description = createProductDto.DescriptionRu
                });
            }

            var createdProduct = await _productRepository.AddAsync(product);
            await _categoryRepository.UpdateProductCountAsync(createProductDto.CategoryId);

            return MapToDTO(createdProduct);
        }

        public async Task<ProductDTO> UpdateProductAsync(int id, CreateProductDTO updateProductDto)
        {
            var product = await _productRepository.GetByIdAsync(
                p => p.Id == id,
                p => p.Translations
            );
            
            if (product == null)
            {
                throw new Exception("Məhsul tapılmadı");
            }

            var oldCategoryId = product.CategoryId;

            product.Name = updateProductDto.Name;
            product.Brand = updateProductDto.Brand;
            product.Price = updateProductDto.Price;
            product.OriginalPrice = updateProductDto.OriginalPrice;
            product.Volume = updateProductDto.Volume;
            product.Type = updateProductDto.Type;
            product.Description = updateProductDto.Description;
            
            if (updateProductDto.Images != null && updateProductDto.Images.Any())
            {
                var imageUrls = new List<string>();
                foreach (var image in updateProductDto.Images)
                {
                    var imageUrl = await _fileManagerService.UploadFileAsync(image);
                    imageUrls.Add(imageUrl);
                }
                product.Images = JsonConvert.SerializeObject(imageUrls);
            }

            product.CategoryId = updateProductDto.CategoryId;
            product.IsActive = updateProductDto.IsActive;
            product.IsFeatured = updateProductDto.IsFeatured;
            
            // Update translations
            product.Translations ??= new List<ProductTranslation>();
            
            // Update or add English translation
            var enTranslation = product.Translations.FirstOrDefault(t => t.LanguageCode == "en");
            if (!string.IsNullOrWhiteSpace(updateProductDto.NameEn))
            {
                if (enTranslation != null)
                {
                    enTranslation.Name = updateProductDto.NameEn;
                    enTranslation.Type = updateProductDto.TypeEn;
                    enTranslation.Description = updateProductDto.DescriptionEn;
                }
                else
                {
                    product.Translations.Add(new ProductTranslation
                    {
                        ProductId = product.Id,
                        LanguageCode = "en",
                        Name = updateProductDto.NameEn,
                        Type = updateProductDto.TypeEn,
                        Description = updateProductDto.DescriptionEn
                    });
                }
            }
            else if (enTranslation != null)
            {
                product.Translations.Remove(enTranslation);
            }
            
            // Update or add Russian translation
            var ruTranslation = product.Translations.FirstOrDefault(t => t.LanguageCode == "ru");
            if (!string.IsNullOrWhiteSpace(updateProductDto.NameRu))
            {
                if (ruTranslation != null)
                {
                    ruTranslation.Name = updateProductDto.NameRu;
                    ruTranslation.Type = updateProductDto.TypeRu;
                    ruTranslation.Description = updateProductDto.DescriptionRu;
                }
                else
                {
                    product.Translations.Add(new ProductTranslation
                    {
                        ProductId = product.Id,
                        LanguageCode = "ru",
                        Name = updateProductDto.NameRu,
                        Type = updateProductDto.TypeRu,
                        Description = updateProductDto.DescriptionRu
                    });
                }
            }
            else if (ruTranslation != null)
            {
                product.Translations.Remove(ruTranslation);
            }

            var updatedProduct = await _productRepository.UpdateAsync(product);

            if (oldCategoryId != updateProductDto.CategoryId)
            {
                await _categoryRepository.UpdateProductCountAsync(oldCategoryId);
                await _categoryRepository.UpdateProductCountAsync(updateProductDto.CategoryId);
            }
            else
            {
                await _categoryRepository.UpdateProductCountAsync(updateProductDto.CategoryId);
            }

            // Reload product with all relations for proper DTO mapping
            var productWithRelations = await _productRepository.GetByIdAsync(
                p => p.Id == id,
                p => p.Category,
                p => p.Translations
            );

            return MapToDTO(productWithRelations);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(p => p.Id == id);
            
            if (product == null)
            {
                throw new Exception("Məhsul tapılmadı");
            }

            var categoryId = product.CategoryId;
            await _productRepository.DeleteAsync(product);
            await _categoryRepository.UpdateProductCountAsync(categoryId);
        }

        private ProductDTO MapToDTO(Product product)
        {
            var dto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                Volume = product.Volume,
                Type = product.Type,
                Description = product.Description,
                Images = JsonConvert.DeserializeObject<List<string>>(product.Images) ?? new List<string>(),
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                IsActive = product.IsActive,
                IsFeatured = product.IsFeatured,
                CreatedOn = product.CreatedOn,
                UpdatedOn = product.UpdatedOn,
                Translations = new Dictionary<string, ProductTranslationDTO>()
            };
            
            // Add default Azerbaijani
            dto.Translations["az"] = new ProductTranslationDTO
            {
                LanguageCode = "az",
                Name = product.Name,
                Description = product.Description,
                Type = product.Type
            };
            
            // Add other translations
            if (product.Translations != null)
            {
                foreach (var translation in product.Translations)
                {
                    dto.Translations[translation.LanguageCode] = new ProductTranslationDTO
                    {
                        LanguageCode = translation.LanguageCode,
                        Name = translation.Name,
                        Description = translation.Description,
                        Type = translation.Type
                    };
                }
            }
            
            return dto;
        }
    }
}
