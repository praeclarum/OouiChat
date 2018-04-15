using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace OouiChat.Data
{
    public class ChatRoom
    {
        readonly ConcurrentQueue<ChatMessage> messages;

        public string Name { get; set; } = "";
        public List<ChatMessage> Messages => messages.ToList ();

        public event EventHandler MessageAdded;
        public event EventHandler UserAdded;

        public void AddMessage (string userName, string message)
        {
            if (string.IsNullOrWhiteSpace (userName) || string.IsNullOrWhiteSpace (message))
                throw new ArgumentException ();

            var now = DateTime.UtcNow;

            var m = new ChatMessage {
                UserName = userName.Trim (),
                Message = message.Trim (),
                UtcTime = now,
            };

            messages.Enqueue (m);

            MessageAdded?.Invoke (this, new MessageEventArgs (m));
        }
    }

    public class MessageEventArgs : EventArgs
    {
        public ChatMessage Message { get; }

        public MessageEventArgs (ChatMessage m)
        {
            this.Message = m;
        }
    }
}
