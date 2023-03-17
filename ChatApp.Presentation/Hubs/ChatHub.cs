using System.Collections;
using Microsoft.AspNetCore.SignalR;
using System.Reflection;

namespace ChatApp.Presentation.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly List<string> _chatRooms = new List<string>();

        public override async Task OnConnectedAsync()
        {
            // Join the default chat room
            await Groups.AddToGroupAsync(Context.ConnectionId, "default");
            await base.OnConnectedAsync();
        }

        public async Task JoinRoom(string roomName)
        {
            // Leave the current room
            await LeaveRoom();

            // Join the new room
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

            // Broadcast the join message to all clients in the new room
            await Clients.Group(roomName).SendAsync("ReceiveMessage", "system", $"{Context.User.Identity.Name} has joined the chat room '{roomName}'");

            // Store the active chat room
            _chatRooms.Add(roomName);
        }

        public async Task LeaveRoom()
        {
            // Get the list of groups the user is currently in
            //var groups = await Groups.GetGroupsAsync(Context.ConnectionId, CancellationToken.None);
            var groupNames = GroupNames();

            // Remove the user from all groups except the default group
            foreach (var group in groupNames.Where(group => group != "default"))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);

                // Broadcast the leave message to all clients in the room
                await Clients.Group(group).SendAsync("ReceiveMessage", "system", $"{Context.User.Identity.Name} has left the chat room '{group}'");
            }

            // Clear the list of active chat rooms
            _chatRooms.Clear();
        }

        public async Task SendMessage(string user, string message)
        {
            // Get the list of groups the user is currently in
            //var groups = await Groups.GetGroupsAsync(Context.ConnectionId);
            var groupNames = GroupNames();

            // Broadcast the message to all clients in the current room
            foreach (var group in groupNames)
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
        }
        public async Task GetRooms()
        {
            await Clients.Caller.SendAsync("GetRooms", _chatRooms);
        }

        private List<string> GroupNames()
        {
            var groupManager = Groups;

            var lifetimeManager = groupManager.GetType().GetRuntimeFields()
                .Single(fi => fi.Name == "_lifetimeManager")
                .GetValue(groupManager);

            var groupsObject = lifetimeManager?.GetType().GetRuntimeFields()
                .Single(fi => fi.Name == "_groups")
                .GetValue(lifetimeManager);

            var groupsDictionary = groupsObject?.GetType().GetRuntimeFields()
                .Single(fi => fi.Name == "_groups")
                .GetValue(groupsObject) as IDictionary;

            var groupNames = groupsDictionary?.Keys.Cast<string>().ToList();
            return groupNames;
        }
    }
}
