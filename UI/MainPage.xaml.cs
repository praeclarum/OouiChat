using System;
using System.Collections.Generic;

using Xamarin.Forms;
using OouiChat.ViewModels;
using OouiChat.Data;
using System.Linq;

namespace OouiChat.UI
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel ViewModel => BindingContext as MainPageViewModel;

        ChatRoom currentRoom = null;

        public MainPage ()
        {
            InitializeComponent ();

            var vm = new MainPageViewModel ();
            BindingContext = vm;

            var rooms = ChatRooms.Shared;
            vm.Rooms.AddRange (rooms.Rooms);

            rooms.RoomAdded += Rooms_RoomAdded;

            if (vm.Rooms.Count == 0) {
                rooms.AddChatRoom ("General");
            }

            EnterRoom ("General");
        }

        protected override void OnDisappearing ()
        {
            base.OnDisappearing ();
            ChatRooms.Shared.RoomAdded -= Rooms_RoomAdded;
            if (currentRoom != null) {
                currentRoom.MessageAdded -= CurrentRoom_MessageAdded;
                currentRoom.UserAdded -= CurrentRoom_UserAdded;
                currentRoom = null;
            }
            BindingContext = null;
        }

        void Handle_SendMessage (object sender, System.EventArgs e)
        {
            if (!(BindingContext is MainPageViewModel vm))
                return;

            try {
                var m = currentRoom?.AddMessage (vm.Username, vm.NewMessage);
                vm.NewMessage = "";
                vm.Error = null;
                newMessage.Focus ();
            }
            catch (Exception ex) {
                vm.Error = ex;
            }
        }

        void Handle_RoomSelected (object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            EnterRoom (((ChatRoom)e.SelectedItem).Name);
        }

        void Handle_UserSelected (object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (!(BindingContext is MainPageViewModel vm))
                return;

            vm.NewMessage = $"@{((ChatUser)e.SelectedItem).Username} {vm.NewMessage}";
            newMessage.Focus ();
        }

        void Handle_CreateNewRoom (object sender, System.EventArgs e)
        {
            if (!(BindingContext is MainPageViewModel vm))
                return;

            try {
                if (string.IsNullOrWhiteSpace (vm.NewRoomName))
                    throw new Exception ("No room name specified");

                ChatRooms.Shared.AddChatRoom (vm.NewRoomName);
                EnterRoom (vm.NewRoomName);
                vm.NewRoomName = "";
                vm.Error = null;
            }
            catch (Exception ex) {
                vm.Error = ex;
            }
        }

        void Rooms_RoomAdded (object sender, RoomEventArgs e)
        {
            ViewModel?.Rooms.Add (e.Room);
        }

        void CurrentRoom_MessageAdded (object sender, MessageEventArgs e)
        {
            if (!(BindingContext is MainPageViewModel vm))
                return;

            vm.Messages.Add (e.Message);
            if (vm.Messages.Count > 64) {
                vm.Messages.RemoveRange (vm.Messages.Take (32));
            }
            messageList.ScrollTo (null, ScrollToPosition.End, false);
        }

        void CurrentRoom_UserAdded (object sender, UserEventArgs e)
        {
            ViewModel?.Users.Add (e.User);
        }

        void EnterRoom (string nextRoomName)
        {
            var vm = ViewModel;
            if (vm == null)
                return;

            //
            // Find the requested room
            //
            if (vm.RoomName == nextRoomName)
                return;

            var room = vm.Rooms.FirstOrDefault (x => x.Name == nextRoomName);
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
            vm.RoomName = room.Name;
            vm.Users.Clear ();
            vm.Messages.Clear ();
            vm.Users.AddRange (currentRoom.Users);
            vm.Messages.AddRange (currentRoom.Messages);

            messageList.ScrollTo (null, ScrollToPosition.End, false);
        }
    }
}
