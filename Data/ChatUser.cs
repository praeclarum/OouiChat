using System;

namespace OouiChat.Data
{
    public class ChatUser
    {
        public string Username { get; set; } = "";
        public string Title => $"@{Username}";
    }
}
