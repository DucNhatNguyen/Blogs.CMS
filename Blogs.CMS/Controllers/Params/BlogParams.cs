using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Blogs.CMS.Controllers.Params
{
    public class AddBlogParams
    {
        [Required]
        [Display(Name = "Tiêu đề")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Slug")]
        public string? Slug { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public int? Status { get; set; } = 1;

        [Display(Name = "Mô tả ngắn")]
        public string? SortDesc { get; set; }

        [Display(Name = "Nội dung")]
        public string? Content { get; set; }

        [Display(Name = "Tác giả")]
        public string? Author { get; set; }

        [Display(Name = "Hình Thumbnail")]
        public string? Thumbnail { get; set; }

        [Display(Name = "Đặt Blog nổi bật")]
        public bool? IsHotBlog { get; set; } = false;

        [Required]
        [Display(Name = "Chuyên mục")]
        public string? CateId { get; set; }

        [NotMapped]
        [Display(Name = "Chọn hình thumbnail")]
        public IFormFile ImageFile { get; set; }

        public string? FileGoogleDriveId { get; set; }

        public List<string>? Tag { get; set; }
    }

    public class EditBlogParams
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Tiêu đề")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Slug")]
        public string? Slug { get; set; }

        [Required]
        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }

        [Display(Name = "Mô tả ngắn")]
        public string? SortDesc { get; set; }

        [Display(Name = "Nội dung")]
        public string? Content { get; set; }

        [Display(Name = "Tác giả")]
        public string? Author { get; set; }

        public string? Thumbnail { get; set; }

        [Display(Name = "Đặt Blog nổi bật")]
        public bool? IsHotBlog { get; set; } = false;

        [Required]
        [Display(Name = "Chuyên mục")]
        public string? CateId { get; set; }

        [NotMapped]
        [Display(Name = "Chọn hình thumbnail")]
        public IFormFile? ImageFile { get; set; }

        public string? FileGoogleDriveId { get; set; }

        public List<string>? Tag { get; set; }
    }

    public class DeleteBlogParams
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }
    }
}
