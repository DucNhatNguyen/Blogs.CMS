using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Blogs.CMS.Controllers.Params
{
    public class AddTagParams
    {
        [Required]
        [Display(Name = "Tiêu đề")]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "slug")]
        public string? Slug { get; set; }
    }

    public class EditTagParams
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tiêu đề")]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "slug")]
        public string? Slug { get; set; }
    }

    public class DeleteTagParams
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }
    }
}
