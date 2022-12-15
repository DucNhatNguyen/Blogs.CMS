using AutoMapper;
using Blogs.CMS.Commons;
using Blogs.CMS.Controllers.Params;
using Blogs.DataAccess.EntityFramework.Entities;
using Blogs.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Blogs.CMS.Services
{
    public class BlogService : BaseService
    {
        private readonly IMapper _mapper;
        public BlogService(
            IHttpContextAccessor httpContextAccessor,
            BlogsDbContextFactory dbContextFactory,
            IMapper mapper
            ) : base(httpContextAccessor, dbContextFactory)
        {
            _mapper = mapper;
        }

        public IQueryable<Blog> GetBlogs()
        {
            try
            {
                var data = DbContext.Blogs.OrderByDescending(_ => _.Publicdate);
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Blog> AddBlog(AddBlogParams input)
        {
            Blog blog = _mapper.Map<Blog>(input);

            var blogEntry = DbContext.Add(blog);

            await DbContext.SaveChangesAsync();

            return blogEntry.Entity;
        }

        public async Task<EditBlogParams> GetBlog(int id)
        {
            var blog = await DbContext.Blogs.FirstAsync(x => x.Id == id);

            EditBlogParams res = _mapper.Map<EditBlogParams>(blog);

            return res;
        }

        public async Task<Blog> Edit(EditBlogParams input)
        {
            Blog blog = _mapper.Map<Blog>(input);

            Blog blogDb = await DbContext.Blogs.FirstAsync(x => x.Id == input.Id);

            Helpers.MergeFieldsChanged(blog, blogDb);

            var blogEntry = DbContext.Update(blogDb);

            if (input.Tag?.Count > 0)
            {
                blogEntry.Entity.Blogtags = input.Tag.Select(_ => new Blogtag
                {
                    Blogid = input.Id,
                    Tagid = Convert.ToInt32(_)
                }).ToList();
            }

            try
            {
                await DbContext.SaveChangesAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return blogEntry.Entity;
        }

        public async Task<Blog> Delete(int id)
        {
            Blog blogDb = await DbContext.Blogs.FirstAsync(_ => _.Id == id);

            var blogEntry = DbContext.Remove(blogDb);

            await DbContext.SaveChangesAsync();

            return blogEntry.Entity;
        }

        public async Task<Blog> CopyBlog(int id)
        {
            var blogDb = await DbContext.Blogs.FirstAsync(_ => _.Id == id);

            Blog blog = _mapper.Map<Blog>(blogDb);
            blog.Id = 0;
            blog.Title = String.Format("{0}_{1}", blog.Title, "Copy");
            blog.Slug = String.Format("{0}-{1}", blog.Slug, "copy");

            var blogEntry = DbContext.Add(blog);

            await DbContext.SaveChangesAsync();

            return blogEntry.Entity;
        }
    }
}
