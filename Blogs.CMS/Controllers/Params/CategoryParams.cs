using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Blogs.CMS.Controllers.Params
{
    public class AddCategoryParams
    {
        [Required]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; } = null!;

        [Display(Name = "Trạng thái")]
        public int Status { get; set; }

        [Display(Name = "Chuyên mục Cha")]
        public int? ParentId { get; set; }

        [Display(Name = "Đặt chuyên mục cha")]
        public bool IsParentCate { get; set; }
    }

    public class EditCategoryParams
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; } = null!;

        [Display(Name = "Trạng thái")]
        public int Status { get; set; }

        [Display(Name = "Chuyên mục Cha")]
        public int? ParentId { get; set; }

        [Display(Name = "Đặt chuyên mục cha")]
        public bool IsParentCate { get; set; }
    }

    public class DeleteCategoryParams
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }
    }
}
