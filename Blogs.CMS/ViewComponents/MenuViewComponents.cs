using Blogs.CMS.Models;
using Blogs.DataAccess.EntityFramework.Entities;
using Blogs.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.CMS.ViewComponents
{
    [ViewComponentAttribute]
    public class MenuViewComponents : ViewComponent
    {
        private readonly BlogsDbContext _dbContext;

        public MenuViewComponents(BlogsDbContextFactory dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Function> menusource = _dbContext.Functions.ToList();
            List<FunctionViewModel> model = CreateVM(null, menusource);
            return View(model);
        }

        private List<FunctionViewModel> CreateVM(string? parentid, List<Function> source)
        {
            var query = from men in source
                        where men.Parentid == parentid
                        where men.Isdeleted == false
                        select new FunctionViewModel()
                        {
                            MenuId = men.Id,
                            Text = men.Text,
                            Link = men.Link,
                            Target = men.Target,
                            CssClass = men.Cssclass!,
                            IsDelete = men.Isdeleted,
                            SLug = men.Slug,
                            Children = CreateVM(men.Id, source)
                        };
            return query.ToList();
        }
    }
}
