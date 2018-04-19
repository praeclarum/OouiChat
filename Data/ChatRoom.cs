using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Xamarin.Forms;

namespace OouiChat.Data
{
    public class ChatRoom
    {
        readonly ConcurrentQueue<ChatMessage> messages = new ConcurrentQueue<ChatMessage> ();
        readonly ConcurrentDictionary<string, ChatUser> users = new ConcurrentDictionary<string, ChatUser> ();

        public string Name { get; set; } = "";
        public List<ChatMessage> Messages => messages.ToList ();
        public List<ChatUser> Users => users.Values.ToList ();
        public string Title => $"#{Name}";

        public event EventHandler<MessageEventArgs> MessageAdded;
        public event EventHandler<UserEventArgs> UserAdded;

        public ChatMessage AddMessage (string username, string message)
        {
            if (string.IsNullOrWhiteSpace (username))
                throw new ArgumentException ("No username specified");
            
            if (string.IsNullOrWhiteSpace (message))
                throw new ArgumentException ("No message specified");

            username = username.Trim ();

            var now = DateTime.UtcNow;

            var m = new ChatMessage {
                Username = username,
                Message = message.Trim (),
                UtcTime = now,
            };

            messages.Enqueue (m);
            while (messages.Count > 32) {
                messages.TryDequeue (out var _);
            }

            MessageAdded?.Invoke (this, new MessageEventArgs (m));

            if (!users.ContainsKey (username)) {
                var u = new ChatUser {
                    Username = username,
                };
                if (users.TryAdd (username, u)) {
                    UserAdded?.Invoke (this, new UserEventArgs (u));
                }
            }

            return m;
        }
    }

    public class UserEventArgs : EventArgs
    {
        public ChatUser User { get; }

        public UserEventArgs (ChatUser user)
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
