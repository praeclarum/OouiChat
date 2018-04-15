using System;
using MvvmHelpers;
using OouiChat.Data;

namespace OouiChat.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        string username = "Frank";
        public string Username {
            get => username;
            set => SetProperty (ref username, value);
        }
        string newMessage = "Hello world";
        public string NewMessage {
            get => newMessage;
            set => SetProperty (ref newMessage, value);
        }
        string roomName = "General";
        public string RoomName {
            get => roomName;
            set => SetProperty (ref roomName, value);
        }
        string newRoomName = "Other";
        public string NewRoomName {
            get => newRoomName;
            set => SetProperty (ref newRoomName, value);
        }

        public ObservableRangeCollection<ChatRoom> Rooms { get; } = new ObservableRangeCollection<ChatRoom> ();
        public ObservableRangeCollection<string> Users { get; } = new ObservableRangeCollection<string> ();
        public ObservableRangeCollection<ChatMessage> Messages { get; } = new ObservableRangeCollection<ChatMessage> ();

        public MainPageViewModel ()
        {
            for (var i = 0; i < 100; i++) {
                Rooms.Add (new ChatRoom { Name = "Room " + i });
                Users.Add ("Room " + i);
                Messages.Add (new ChatMessage { Message = "Words words words " + i, UserName = "User " + i, UtcTime = DateTime.UtcNow });
            }
        }
    }
}
