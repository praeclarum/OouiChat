using System;

namespace OouiChat.Data
{
    public class ChatMessage
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime UtcTime { get; set; }
    }
}

