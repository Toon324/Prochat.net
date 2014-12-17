using System;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Collections.Generic;



namespace Prochat.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        public static int messageNumber = 0;
        public static List<string> users = new List<string>();
        private string roomName = "default";
        private bool historyEnabled = false;

        public void Hello()
        {
            Clients.All.hello();
        }

        public void History()
        {
            if (!historyEnabled)
                return;

           var history = DataAccess.ChatHistoryDatabaseConnector.GetHistory(roomName);

           string user = "";

           foreach (string s in history)
           {
               //Simple parser that loads a username first then prints the message second, due to the fact the SQL connector currently returns everything as a 1-dimensional list.
               if (user.Equals(""))
                   user = s;
               else
               {
                   HandleMessage(user, s, true);
                   user = "";
               }
               
           }

        }

        public void Join(string name)
        {
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

        public void Send(string name, string message)
        {
            // Save to history
            if (historyEnabled)
                DataAccess.ChatHistoryDatabaseConnector.AddToHistory(roomName, name, message);

            HandleMessage(name, message);
        }

        private void HandleMessage(string name, string message)
        {
            HandleMessage(name, message, false);
        }

        private void HandleMessage(string name, string message, bool localOnly)
        {
            if (message.Equals(""))
                return; //Don't handle an empty message

            // Call the addNewMessageToPage method to update clients.
            message = ParseMessage(message);
            if (localOnly)
                Clients.Caller.addNewMessageToPage(name, message, messageNumber);
            else
                Clients.All.addNewMessageToPage(name, message, messageNumber);
            messageNumber++;
        }

        private string ParseMessage(string message)
        {
            if (message.Equals("Prochat, show me your features"))
            {
                Send("Prochat", "Hello, and welcome to Prochat! Here are the current awesome features you can enjoy:");

                Send("Prochat", "You can also paste most Youtube video links and have them automagically embed. If a video isn't working, try using the link found in the \"Share\" tab on the video page.");
                Send("Prochat", "http://youtu.be/SQoA_wjmE9w");

                Send("Prochat", "Images such as png, jpg, and gif are all embedded as well. All embeded objects can be hidden and shown using the link found in the message.");
                Send("Prochat", "http://i.imgur.com/p49J3rc.gif");

                Send("Prochat", "Other links will simply be linked in the message, like so: http://www.google.com");

                return "";
            }
            else
                return Services.MessageHandler.HandleMessage(message);
        }

        public void Debug(string msg)
        {
            Clients.All.addNewMessageToPage("Prochat Debugger", msg);
        }
    }
}