using Microsoft.Extensions.Logging;
using Troonch.Application.Base.UnitOfWork;
using Troonch.RetailSales.Product.DataAccess.Repositories.Interfaces;
using Troonch.Sales.Domain.Entities;

namespace Troonch.RetailSales.Product.Application.Services;

public class ProductGenderCategoryService
{
    private readonly ILogger<ProductGenderCategoryService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductGenderCategoryRepository _productGenderCategoryRepository;
    public ProductGenderCategoryService(
                                        IUnitOfWork unitOfWork, 
                                        IProductGenderCategoryRepository productGenderCategoryRepository,
                                        ILogger<ProductGenderCategoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _productGenderCategoryRepository = productGenderCategoryRepository;
        _logger = logger;
    }

    public async Task<List<ProductGenderCategoryLookup>> GetProductGenderCategoriesAsync()
    {
        var productGenderCategories = await _productGenderCategoryRepository.GetAllAsync();

        if (productGenderCategories == null) 
        {
            _logger.LogError("ProductGenderCategoryService::GetProductGenderCategoriesAsync productGenderCategories is null");
            throw new ArgumentNullException(nameof(productGenderCategories));
        }

        return productGenderCategories;
    }

    public async Task<List<ProductGenderCategoryLookup>> GetProductGenderCategoriesByCateogryIdAsync(Guid categoryId)
    {
        if (categoryId == Guid.Empty) 
        {
            _logger.LogError("ProductGenderCategoryService::GetProductGenderCategoriesByCateogryIdAsync categoryId is empty");
            throw new ArgumentException(nameof(categoryId));
        }

        return await _productGenderCategoryRepository.GetProductGenderCategoryByCategoryIdAsync(categoryId);
    }

    public async Task<List<ProductGenderCategoryLookup>> GetProductGenderCategoriesByGenderIdAsync(Guid genderId)
    {
        if (genderId == Guid.Empty)
        {
            _logger.LogError("ProductGenderCategoryService::GetProductGenderCategoriesByGenderIdAsync genderId is empty");
            throw new ArgumentException(nameof(genderId));
        }

        return await _productGenderCategoryRepository.GetProductGenderCategoryByGenderIdAsync(genderId);
    }

    public async Task<bool> HandleProductGenderCategoriesByCatgoryId(Guid categoryId,List<ProductGenderCategoryLookup> productGenderCategoryLookups)
    {
        if (categoryId == Guid.Empty)
        {
            _logger.LogError("ProductGenderCategoryService::UpdateProductGenderCategories categoryId is empty");
            throw new ArgumentNullException(nameof(categoryId));
        }

        var isListRemoved =  await RemoveAllByCategoryId(categoryId);

        if (!isListRemoved)
        {
            _logger.LogError($"ProductGenderCategoryService::UpdateProductGenderCategories Clean operation not executed successfully to  category {categoryId}");
            throw new Exception(nameof(isListRemoved));
        }

        if (productGenderCategoryLookups is null)
        {
            _logger.LogError("ProductGenderCategoryService::UpdateProductGenderCategories productGenderCategoryLookups is null");
            throw new ArgumentNullException(nameof(productGenderCategoryLookups));
        }

        if (!productGenderCategoryLookups.Any())
        {
            _logger.LogInformation("ProductGenderCategoryService::UpdateProductGenderCategories the productGenderCategoryLookups is Empty");
            return true;
        }

        await _productGenderCategoryRepository
                .AddRangeAsync(productGenderCategoryLookups);

        var isProductGenderCategoriesUpdated = await _unitOfWork.CommitAsync();

        if (!isProductGenderCategoriesUpdated) 
        {
            _logger.LogError("ProductGenderCategoryService::UpdateProductGenderCategoriesByCatgoryId isProductGenderCategoriesUpdated is false");
            throw new Exception(nameof(isProductGenderCategoriesUpdated));
        }

        return isProductGenderCategoriesUpdated;
    }

    #region Private Methods 
    private async Task<bool> RemoveAllByCategoryId(Guid categoryId)
    {
      

        var listToRemove = await _productGenderCategoryRepository
                                    .GetProductGenderCategoryByCategoryIdAsync(categoryId);


        if(listToRemove is null)
        {
            _logger.LogError($"ProductGenderCategoryService::RemoveAllByCategoryId List to remove not retrived from categoryId - {categoryId}");
            throw new ArgumentNullException(nameof(listToRemove));
        }

        if (!listToRemove.Any())
        {
            return true;
        }

        await _productGenderCategoryRepository
               .BulkDeleteByCategoryIdAsync(categoryId);

        return await _unitOfWork.CommitAsync();
    }
    #endregion
}
