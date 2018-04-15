using System;
using Microsoft.AspNetCore.Mvc;
using Ooui;
using Ooui.AspNetCore;
using OouiChat.Data;

namespace FuGetGallery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index ()
        {
            var chatList = new Paragraph ($"{ChatRooms.Shared.Rooms.Count} rooms");
            var inputBox = new Input {
                Placeholder = "Enter text to transmit"
            };
            var ui = new Div (chatList, inputBox);
            return new ElementResult (ui, title: "Ooui Chat");
        }

        public ActionResult Error ()
        {
            return this.Content("Beep boop something broke");
        }
    }
}
