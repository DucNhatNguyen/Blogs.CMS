using Blogs.CMS.Commons;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.CMS.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {

        }

        public void SetNotification(string message, NotificationEnumeration notifyType, bool autoHideNotification = true)
        {
            TempData["Notification"] = message;
            //this.TempData["NotificationAutoHide"] = (autoHideNotification) ? "true" : "false";

            switch (notifyType)
            {
                case NotificationEnumeration.Success:
                    TempData["NotificationType"] = "success";
                    break;
                case NotificationEnumeration.Error:
                    TempData["NotificationType"] = "error";
                    break;
            }
        }
    }
}
