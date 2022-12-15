using AutoMapper;
using Blogs.CMS.Commons;
using Blogs.CMS.Controllers.Params;
using Blogs.CMS.Models;
using Blogs.DataAccess.EntityFramework.Entities;

namespace Blogs.CMS.Configurations
{
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {
            CategoryMapping();
            BlogMapping();
            TagMapping();
        }

        private void CategoryMapping()
        {
            CreateMap<CategoryGetModel, Category>();
            CreateMap<AddCategoryParams, Category>();
            CreateMap<EditCategoryParams, Category>();

            CreateMap<EditCategoryParams, DeleteCategoryParams>();
            CreateMap<Category, EditCategoryParams>();
        }

        private void BlogMapping()
        {
            CreateMap<AddBlogParams, Blog>();
            CreateMap<Blog, EditBlogParams>(); // for get detail

            CreateMap<EditBlogParams, Blog>()
                .ForMember(_ => _.Publicdate, otp =>
                {
                    otp.Condition(src => src.Status == (int)BlogStatus.Public);
                    otp.MapFrom(src => DateTime.Now);
                }); // for update

            CreateMap<EditBlogParams, DeleteBlogParams>();
        }

        private void TagMapping()
        {
            CreateMap<AddTagParams, Tag>();
            CreateMap<EditTagParams, Tag>();
            CreateMap<Tag, EditTagParams>();
            CreateMap<EditTagParams, DeleteTagParams>();
        }
    }
}
