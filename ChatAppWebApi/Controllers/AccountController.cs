using Microsoft.AspNetCore.Mvc;

namespace ChatAppWebApi.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
