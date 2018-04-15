using System;
using Microsoft.AspNetCore.Mvc;
using Ooui;
using Ooui.AspNetCore;

namespace FuGetGallery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index ()
        {
            var inputBox = new Input {
                Placeholder = "Enter text to transmit"
            };
            var ui = inputBox;
            return new ElementResult (ui, title: "Ooui Chat");
        }

        public ActionResult Error ()
        {
            return this.Content("Beep boop something broke");
        }
    }
}
