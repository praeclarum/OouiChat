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
        public List<string> Users => users.Keys.ToList ();

        public event EventHandler<MessageEventArgs> MessageAdded;
        public event EventHandler<UserEventArgs> UserAdded;

        public ChatMessage AddMessage (string username, string message)
        {
            if (string.IsNullOrWhiteSpace (username) || string.IsNullOrWhiteSpace (message))
                throw new ArgumentException ();

            username = username.Trim ();

            var now = DateTime.UtcNow;

            var m = new ChatMessage {
                UserName = username,
                Message = message.Trim (),
                UtcTime = now,
            };

            messages.Enqueue (m);
            MessageAdded?.Invoke (this, new MessageEventArgs (m));

            if (!users.ContainsKey (username)) {
                if (users.TryAdd (username, now)) {
                    UserAdded?.Invoke (this, new UserEventArgs (username));
                }
            }
            users[username] = now;

            return m;
        }
    }

    public class UserEventArgs : EventArgs
    {
        public string User { get; }

        public UserEventArgs (string user)
        {
            this.User = user;
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
