using AutoMapper;
using Blogs.CMS.Commons;
using Blogs.CMS.Controllers.Params;
using Blogs.DataAccess.EntityFramework.Entities;
using Blogs.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Blogs.CMS.Services
{
    public class CategoryService : BaseService
    {
        private readonly IMapper _mapper;
        public CategoryService(
            IHttpContextAccessor httpContextAccessor,
            BlogsDbContextFactory dbContextFactory,
            IMapper mapper
            ) : base(httpContextAccessor, dbContextFactory)
        {
            _mapper = mapper;
        }

        public IQueryable<Category> GetCategories() => DbContext.Categories.OrderByDescending(_ => _.Createddate);

        public async Task<Category> AddCategory(AddCategoryParams input)
        {
            Category category = _mapper.Map<Category>(input);
            category.Createdby = "system";
            category.Createddate = DateTime.Now;

            var categoryEntry = DbContext.Add(category);

            await DbContext.SaveChangesAsync();

            return categoryEntry.Entity;
        }

        public IQueryable<Category> GetParentCategories()
        {
            return DbContext.Categories.Where(_ => _.Isparentcate == true).OrderByDescending(x => x.Createddate);
        }

        public async Task<EditCategoryParams> GetCategoryDetail(int id)
        {
            var data = await DbContext.Categories.Where(_ => _.Id == id)
                .FirstAsync();

            EditCategoryParams category = _mapper.Map<EditCategoryParams>(data);

            return await Task.FromResult(category);
        }

        public async Task<Category> EditCategory(EditCategoryParams input)
        {
            Category category = _mapper.Map<Category>(input);

            Category categoryDb = await DbContext.Categories.FirstAsync(x => x.Id == input.Id);

            Helpers.MergeFieldsChanged(category, categoryDb);

            var categoryEntry = DbContext.Update(categoryDb);

            await DbContext.SaveChangesAsync();

            return categoryEntry.Entity;
        }

        public async Task<Category> DeleteCategory(int id)
        {
            Category categoryDb = await DbContext.Categories.FirstAsync(x => x.Id == id);

            var categoryEntry = DbContext.Remove(categoryDb);

            await DbContext.SaveChangesAsync();

            return categoryEntry.Entity;
        }
    }
}
