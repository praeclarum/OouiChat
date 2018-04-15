using System;
using MvvmHelpers;
using OouiChat.Data;
using System.Linq;

namespace OouiChat.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        string username = "Anonymous";
        public string Username {
            get => username;
            set => SetProperty (ref username, value);
        }
        string newMessage = "";
        public string NewMessage {
            get => newMessage;
            set => SetProperty (ref newMessage, value);
        }
        string roomName = "";
        public string RoomName {
            get => roomName;
            set => SetProperty (ref roomName, value);
        }
        string newRoomName = "";
        public string NewRoomName {
            get => newRoomName;
            set => SetProperty (ref newRoomName, value);
        }

        public ObservableRangeCollection<ChatRoom> Rooms { get; } = new ObservableRangeCollection<ChatRoom> ();
        public ObservableRangeCollection<string> Users { get; } = new ObservableRangeCollection<string> ();
        public ObservableRangeCollection<ChatMessage> Messages { get; } = new ObservableRangeCollection<ChatMessage> ();

        ChatRoom currentRoom;

        public MainPageViewModel ()
        {
            var rooms = ChatRooms.Shared;

            Rooms.AddRange (rooms.Rooms);
            rooms.RoomAdded += Rooms_RoomAdded;
            if (Rooms.Count == 0) {
                rooms.AddChatRoom ("General");
            }

            EnterRoom ("General");
        }

        void Rooms_RoomAdded (object sender, RoomEventArgs e)
        {
            Rooms.Add (e.Room);
        }

        void CurrentRoom_MessageAdded (object sender, MessageEventArgs e)
        {
            Messages.Add (e.Message);
        }

        void CurrentRoom_UserAdded (object sender, UserEventArgs e)
        {
            Users.Add (e.User);
        }

        void EnterRoom (string nextRoomName)
        {
            //
            // Find the requested room
            //
            if (roomName == nextRoomName)
                return;
            
            var room = Rooms.FirstOrDefault (x => x.Name == nextRoomName);
            if (room == null) {
                room = ChatRooms.Shared.GeneralRoom;
            }


            //
            // Set the new current room
            //
            if (room == currentRoom)
                return;

            if (currentRoom != null) {
                currentRoom.MessageAdded -= CurrentRoom_MessageAdded;
                currentRoom.UserAdded -= CurrentRoom_UserAdded;
            }
            currentRoom = room;
            currentRoom.MessageAdded += CurrentRoom_MessageAdded;
            currentRoom.UserAdded += CurrentRoom_UserAdded;

            //
            // Update the display data
            //
            RoomName = room.Name;
            Users.Clear ();
            Messages.Clear ();
            Users.AddRange (currentRoom.Users);
            Messages.AddRange (currentRoom.Messages);
            for (var i = 0; i < 100; i++) {
                Users.Add ("User " + i);
                Messages.Add (new ChatMessage { Message = "Words words words " + i, UserName = "User " + i, UtcTime = DateTime.UtcNow });
            }
        }
    }
}
