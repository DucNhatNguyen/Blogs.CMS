using Blogs.CMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.CMS.Controllers
{
    public class NavigationController : BaseController
    {
        private readonly NavigationService _navigationService;

        public NavigationController(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public IActionResult _MainMenu()
        {
            var menu = _navigationService.GetMenuSideBar();
            return View(menu);
        }
    }
}
