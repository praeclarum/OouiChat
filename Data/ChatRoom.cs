using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace OouiChat.Data
{
    public class ChatRoom
    {
        readonly ConcurrentQueue<ChatMessage> messages = new ConcurrentQueue<ChatMessage> ();
        readonly ConcurrentDictionary<string, DateTime> users = new ConcurrentDictionary<string, DateTime> ();

        public string Name { get; set; } = "";
        public List<ChatMessage> Messages => messages.ToList ();

        public event EventHandler<MessageEventArgs> MessageAdded;
        public event EventHandler<UserEventArgs> UserAdded;

        public void AddMessage (string userName, string message)
        {
            if (string.IsNullOrWhiteSpace (userName) || string.IsNullOrWhiteSpace (message))
                throw new ArgumentException ();

            userName = userName.Trim ();

            var now = DateTime.UtcNow;

            var m = new ChatMessage {
                UserName = userName,
                Message = message.Trim (),
                UtcTime = now,
            };

            messages.Enqueue (m);
            MessageAdded?.Invoke (this, new MessageEventArgs (m));

            if (!users.ContainsKey (userName)) {
                if (users.TryAdd (userName, now)) {
                    UserAdded?.Invoke (this, new UserEventArgs (userName));
                }
            }
            users[userName] = now;
        }
    }

    public class UserEventArgs : EventArgs
    {
        public string UserName { get; }

        public UserEventArgs (string userName)
        {
            this.UserName = userName;
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
