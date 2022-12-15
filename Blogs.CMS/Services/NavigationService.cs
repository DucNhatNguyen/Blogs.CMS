using Blogs.CMS.Models;
using Blogs.DataAccess.EntityFramework.Entities;
using Blogs.DataAccess.EntityFramework;

namespace Blogs.CMS.Services
{
    public class NavigationService : BaseService
    {
        public NavigationService(
            IHttpContextAccessor httpContextAccessor,
            BlogsDbContextFactory dbContextFactory) : base(httpContextAccessor, dbContextFactory)
        {

        }
        public List<FunctionViewModel> GetMenuSideBar()
        {
            List<Function> menusource = DbContext.Functions.ToList();
            List<FunctionViewModel> model = CreateVM(null, menusource);
            return model;
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
