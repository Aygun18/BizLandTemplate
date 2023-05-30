using Microsoft.AspNetCore.Mvc;

namespace BizLandTemplate.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
