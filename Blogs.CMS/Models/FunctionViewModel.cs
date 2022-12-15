namespace Blogs.CMS.Models
{
    public class FunctionViewModel
    {
        public string MenuId { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public string Target { set; get; }
        public string CssClass { set; get; }
        public bool IsDelete { get; set; }
        public List<FunctionViewModel> Children { get; set; }
        public string? SLug { get; set; }
    }
}
