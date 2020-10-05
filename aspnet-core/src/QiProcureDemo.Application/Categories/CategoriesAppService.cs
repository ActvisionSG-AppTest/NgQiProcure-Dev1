

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QiProcureDemo.Categories.Exporting;
using QiProcureDemo.Categories.Dtos;
using QiProcureDemo.Dto;
using Abp.Application.Services.Dto;
using QiProcureDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp;

namespace QiProcureDemo.Categories
{
	[AbpAuthorize(AppPermissions.Pages_Administration_Categories)]
    public class CategoriesAppService : QiProcureDemoAppServiceBase, ICategoriesAppService
    {
		 private readonly IRepository<Category> _categoryRepository;
		 private readonly ICategoriesExcelExporter _categoriesExcelExporter;
		 

		  public CategoriesAppService(IRepository<Category> categoryRepository, ICategoriesExcelExporter categoriesExcelExporter ) 
		  {
			_categoryRepository = categoryRepository;
			_categoriesExcelExporter = categoriesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input)
         {
			
			var filteredCategories = _categoryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.IsApprovedFilter > -1,  e => (input.IsApprovedFilter == 1 && e.IsApproved) || (input.IsApprovedFilter == 0 && !e.IsApproved) )
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) );

			var pagedAndFilteredCategories = filteredCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var categories = from o in pagedAndFilteredCategories
                         select new GetCategoryForViewDto() {
							Category = new CategoryDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                IsApproved = o.IsApproved,
                                IsActive = o.IsActive,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCategories.CountAsync();

            return new PagedResultDto<GetCategoryForViewDto>(
                totalCount,
                await categories.ToListAsync()
            );
         }

        public List<NameValue<string>> GetCategoriesNameValue()
        {
           
            var filteredCategories = _categoryRepository.GetAll()
                        .Where(e => (e.IsApproved && e.IsActive));

            var pagedAndFilteredCategories = filteredCategories
                .OrderBy("Name asc");

            var categories = from o in pagedAndFilteredCategories
                             select new GetCategoryForViewDto()
                             {
                                 Category = new CategoryDto
                                 {
                                     Name = o.Name,
                                     Description = o.Description,
                                     IsApproved = o.IsApproved,
                                     IsActive = o.IsActive,
                                     Id = o.Id
                                 }
                             };


            var categoryList = new List<NameValue<string>>{};

            foreach (var category in categories)
            {
                categoryList.Add(new NameValue { Name = category.Category.Name, Value = category.Category.Id.ToString() });
            }

            return categoryList;
        }

        public async Task<GetCategoryForViewDto> GetCategoryForView(int id)
         {
            var category = await _categoryRepository.GetAsync(id);

            var output = new GetCategoryForViewDto { Category = ObjectMapper.Map<CategoryDto>(category) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Categories_Edit)]
		 public async Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto input)
         {
            var category = await _categoryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCategoryForEditOutput {Category = ObjectMapper.Map<CreateOrEditCategoryDto>(category)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCategoryDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Categories_Create)]
		 protected virtual async Task Create(CreateOrEditCategoryDto input)
         {
            var category = ObjectMapper.Map<Category>(input);

			
			if (AbpSession.TenantId != null)
			{
				category.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _categoryRepository.InsertAsync(category);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Categories_Edit)]
		 protected virtual async Task Update(CreateOrEditCategoryDto input)
         {
            var category = await _categoryRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, category);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Categories_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _categoryRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCategoriesToExcel(GetAllCategoriesForExcelInput input)
         {
			
			var filteredCategories = _categoryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.IsApprovedFilter > -1,  e => (input.IsApprovedFilter == 1 && e.IsApproved) || (input.IsApprovedFilter == 0 && !e.IsApproved) )
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) );

			var query = (from o in filteredCategories
                         select new GetCategoryForViewDto() { 
							Category = new CategoryDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                IsApproved = o.IsApproved,
                                IsActive = o.IsActive,
                                Id = o.Id
							}
						 });


            var categoryListDtos = await query.ToListAsync();

            return _categoriesExcelExporter.ExportToFile(categoryListDtos);
         }


    }
}