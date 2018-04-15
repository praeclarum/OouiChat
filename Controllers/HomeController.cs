using System;
using Microsoft.AspNetCore.Mvc;
using Ooui.AspNetCore;
using Xamarin.Forms;

using OouiChat.UI;

namespace OouiChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index ()
        {
            var page = new MainPage ();
            return new ElementResult (page.GetOouiElement (), title: "Ooui Chat");
        }

        public ActionResult Error ()
        {
            return this.Content("Beep boop something broke");
        }
    }
}
