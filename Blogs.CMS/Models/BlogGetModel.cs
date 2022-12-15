using Blogs.CMS.Commons;

namespace Blogs.CMS.Models
{
    public class BlogGetModel
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public int? Status { get; set; }

        public string? SortDesc { get; set; }

        public string? Content { get; set; }

        public DateTime? PublicDate { get; set; }

        public int? View { get; set; }

        public int? Author { get; set; }

        public string? Thumbnail { get; set; }

        public bool? IsHotBlog { get; set; }

        public string? StatusName => ((BlogStatus)Status!).GetDisplayName();

        public string? FileGoogleDriveId { get; set; }
    }
}
