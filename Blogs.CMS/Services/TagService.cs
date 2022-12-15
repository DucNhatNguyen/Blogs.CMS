using AutoMapper;
using Blogs.CMS.Commons;
using Blogs.CMS.Controllers.Params;
using Blogs.DataAccess.EntityFramework.Entities;
using Blogs.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Blogs.CMS.Services
{
    public class TagService : BaseService
    {
        private readonly IMapper _mapper;
        public TagService(
            IHttpContextAccessor httpContextAccessor,
            BlogsDbContextFactory dbContextFactory,
            IMapper mapper
            ) : base(httpContextAccessor, dbContextFactory)
        {
            _mapper = mapper;
        }

        public IQueryable<Tag> GetTags()
        {
            return DbContext.Tags.OrderByDescending(_ => _.Title);
        }

        public async Task<EditTagParams> GetTag(int id)
        {
            var tag = await DbContext.Tags.FirstAsync(x => x.Id == id);

            EditTagParams res = _mapper.Map<EditTagParams>(tag);

            return res;
        }

        public async Task<Tag> Add(AddTagParams input)
        {
            Tag tag = _mapper.Map<Tag>(input);

            var tagEntry = DbContext.Add(tag);

            await DbContext.SaveChangesAsync();

            return tagEntry.Entity;
        }

        public async Task<Tag> Edit(EditTagParams input)
        {
            Tag tag = _mapper.Map<Tag>(input);

            Tag tagdb = await DbContext.Tags.FirstAsync(x => x.Id == input.Id);

            Helpers.MergeFieldsChanged(tag, tagdb);

            var tagEntry = DbContext.Update(tagdb);

            await DbContext.SaveChangesAsync();

            return tagEntry.Entity;
        }

        public async Task<Tag> Delete(int id)
        {
            Tag tagDb = await DbContext.Tags.FirstAsync(x => x.Id == id);

            var tagEntry = DbContext.Remove(tagDb);

            await DbContext.SaveChangesAsync();

            return tagEntry.Entity;
        }

        public async Task<List<Blogtag>> GetTagOfBlog(int blogId)
        {
            var data = await DbContext.Blogtags.Where(_ => _.Blogid == blogId).ToListAsync();
            return data;
        }
    }
}
