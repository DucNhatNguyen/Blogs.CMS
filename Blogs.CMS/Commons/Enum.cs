using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Blogs.CMS.Commons
{
    public enum NotificationEnumeration
    {
        Success,
        Error,
        Warning
    }

    public enum BlogStatus
    {
        [Display(Name = "Nháp")]
        Draff = 1,
        [Display(Name = "Công khai")]
        Public = 2,
        [Display(Name = "Ẩn")]
        Hidden = 3
    }
}
