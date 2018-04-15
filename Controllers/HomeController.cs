using System;
using Microsoft.AspNetCore.Mvc;

namespace FuGetGallery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index ()
        {
            return Json(5646544654);
        }
    }
}
