using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using System.Collections.Generic;
using System.Linq;
using Prochat.DataAccess;


namespace Prochat.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        public static int messageNumber = 0;
        public static List<string> users = new List<string>();
        private bool historyEnabled = true;

        public void Hello()
        {
            Clients.All.hello();
        }

        public void GetHistory(string group, string room)
        {
            if (!historyEnabled)
                return;

            var history = DataAccess.ChatHistoryDatabaseConnector.GetHistory(group, room);

            if (!history.Any())
                return;

            foreach (var message in history)
            {
                var groupId = GroupsDatabaseConnector.GetGroupIdOfRoom(message.RoomId);

                HandleMessage(GroupsDatabaseConnector.GetGroupNameById(groupId), GroupsDatabaseConnector.GetRoomNameById(message.RoomId), message.Username, message.Message);
            }

        }

        public void Join(string name)
        {
            //Ensure no duplicate names on the list
            if (!users.Contains(name))
                 users.Add(name);

            UpdateUserLists();
        }

        public void UpdateUserLists()
        {
            Clients.All.clearUsers();
            foreach (string user in users)
                Clients.All.loadUsers(user);
        }


        public void Leave(string name)
        {
            users.Remove(name);
            UpdateUserLists();
        }

        public void GetConnectedUsers()
        {
            foreach (string user in users)
                Clients.Caller.loadUsers(user);
        }

        public void Send(string group, string room, string name, string message)
        {
            // Save to history
            if (historyEnabled)
                DataAccess.ChatHistoryDatabaseConnector.AddToHistory(group, room, name, message);

            HandleMessage(group, room, name, message);
        }

        private void HandleMessage(string group, string room, string name, string message, bool localOnly = false)
        {
            if (message.Equals(""))
                return; //Don't handle an empty message

            // Call the addNewMessageToPage method to update clients.
            message = ParseMessage(group, room, message);

            if (message.Equals(""))
                return; 

            if (localOnly)
                Clients.Caller.addNewMessageToPage(group, room, name, message, messageNumber);
            else
                Clients.All.addNewMessageToPage(group, room, name, message, messageNumber);
            messageNumber++;
        }

        private string ParseMessage(string group, string room, string message)
        {
            if (message.Equals("/features"))
            {
                LocalSystemMessage("Hello, and welcome to Prochat! Here are the current awesome features you can enjoy:", group, room);
                LocalSystemMessage("You can also paste most Youtube video links and have them automagically embed. If a video isn't working, try using the link found in the \"Share\" tab on the video page. http://youtu.be/SQoA_wjmE9w", group, room);
                LocalSystemMessage("Images such as png, jpg, and gif are all embedded as well. All embeded objects can be hidden and shown using the link found in the message. http://i.imgur.com/p49J3rc.gif", group, room);
                LocalSystemMessage("Other links will simply be linked in the message, like so: http://www.google.com", group, room);
                LocalSystemMessage("Soundcloud links are also embedded: https://soundcloud.com/toon324/they-call-me-waluigi", group,room);
                LocalSystemMessage("As are twitch streams: http://www.twitch.tv/twitchplayspokemon", group, room);
                LocalSystemMessage("We also have several convenience methods for grabbing content. Please type /help for a full list.", group, room);

                return "";
            }
            if (message.StartsWith("/help"))
            {
                Clients.Caller.addNewMessageToPage(@group, room, "Prochat", "<table> <tr><td>/gif {search term}</td><td>Returns a .gif from Giphy based on the search term.</td></tr></table>", 0);
                return "";
            }

            return Services.MessageHandler.HandleMessage(message);
        }

        public void LocalSystemMessage(string msg, string group, string room)
        {
            HandleMessage(group, room, "Prochat", msg, true);
        }

        public void SystemMessage(string msg, string group, string room)
        {
            HandleMessage(group, room, "Prochat", msg);
        }
    }
}