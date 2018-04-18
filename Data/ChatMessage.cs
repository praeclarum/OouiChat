using System;

namespace OouiChat.Data
{
    public class ChatMessage
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime UtcTime { get; set; }

        public string Metadata => $"{Username} on {UtcTime:MMM d} at {UtcTime:h:mm}";
    }
}

