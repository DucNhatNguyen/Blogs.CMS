namespace Blogs.CMS.Models
{
    public class CategoryGetModel
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public int? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public int? ParentId { get; set; }

        public bool? IsParentCate { get; set; }

        public string StatusName
        {
            get
            {
                return Status == 1 ? "Kích hoạt" : "Ẩn";
            }
        }
    }
}
