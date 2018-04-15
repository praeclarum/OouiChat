using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace OouiChat.Data
{
    public class ChatRooms
    {
        public static ChatRooms Shared { get; } = new ChatRooms ();

        readonly ConcurrentDictionary<string, ChatRoom> rooms = new ConcurrentDictionary<string, ChatRoom> ();

        public ChatRoom GeneralRoom { get; } = new ChatRoom { Name = "General" };

        public List<ChatRoom> Rooms => rooms.Values.ToList ();

        public event EventHandler<RoomEventArgs> RoomAdded;

        public ChatRooms ()
        {
            rooms.TryAdd (GeneralRoom.Name, GeneralRoom);
        }

        public ChatRoom FindChatRoom (string name)
        {
            rooms.TryGetValue (name, out var room);
            return room;
        }

        public ChatRoom AddChatRoom (string name)
        {
            if (string.IsNullOrWhiteSpace (name))
                throw new ArgumentException ();

            var room = new ChatRoom {
                Name = name,
            };
            if (rooms.TryAdd (name, room)) {
                RoomAdded?.Invoke (this, new RoomEventArgs (room));
            }
            return room;
        }
    }

    public class RoomEventArgs : EventArgs
    {
        public ChatRoom Room { get; }

        public RoomEventArgs (ChatRoom room)
        {
            this.Room = room;
        }
    }
}
