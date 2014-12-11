using System;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Linq;



namespace Prochat.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        private static int messageNumber = 0;
        private string roomName = "default";

        public void Hello()
        {
            Clients.All.hello();
        }

        public void History()
        {
           var history = DataAccess.ChatHistoryDatabaseConnector.GetHistory(roomName);

           string user = "";


           foreach (string s in history)
           {
               //Simple parser that loads a username first then prints the message second, due to the fact the SQL connector currently returns everything as a 1-dimensional list.
               if (user.Equals(""))
                   user = s;
               else
               {
                   HandleMessage(user, s);
                   user = "";
               }
               
           }
            

          /*
            for (int i = 0; i < history.Length; i++)
           {
                  HandleMessage("History", history[i]);
 
           }
           */
           
          // HandleMessage("Output", history);
        }

        public void Send(string name, string message)
        {
            // Save to history
            DataAccess.ChatHistoryDatabaseConnector.AddToHistory(roomName, name, message);

            HandleMessage(name, message);
        }

        private void HandleMessage(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            message = ParseMessage(message);
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


            }
            else if (message.Equals("history"))
                History();
            else if (message.Contains("youtu.be") || message.Contains("youtube.com/watch?"))
                message = HandleYoutube(message);
            else if (message.Contains("twitch.tv"))
                message = HandleTwitch(message);
            else if (message.Contains("Google Hangout"))
                message = HandleHangout(message);
            else if (message.Contains(".png") || message.Contains(".jpg") || message.Contains(".gif"))
                message = HandleImage(message);
            else if (message.Contains("http"))
                message = HandleLink(message);
            
            return message;
        }

        private string HandleLink(string message)
        {
            var url = Regex.Match(message, @"http\S*").ToString();
            var wrapped = "<a target=\"_blank\" href=\"" + url + "\" >" + url + "</a>";

            message = message.Replace(url, wrapped);
            return message;
        }

        private string HandleYoutube(string message)
        {
            string data;

            if (message.Contains("youtu.be"))
                data = message.Substring(message.IndexOf("youtu.be/") + 9);
            else
                data = Regex.Match(message, @"v=\S*").ToString().Replace("v=","");

            //Ensure no values are passed in
            if (data.Contains("?"))
                data = data.Substring(0, data.IndexOf("?"));

            message = Embed("Youtube Video", "<embed width=\"420\" height=\"315\" src=\"http://www.youtube.com/v/" + data + "\">");
   
            return message;
        }

        private string HandleImage(string message)
        {
            var url = Regex.Match(message, @"http\S*").ToString();
            var embedded = "<img src=\"" + url + "\" style=\"width: 400px; height:300px\">";
            message = message.Replace(url, Embed("Image", embedded));

            return message;
        }
       // <iframe src="http://www.twitch.tv/tsm_bjergsen/embed" frameborder="0" scrolling="no" height="378" width="620"></iframe>
        
        private string HandleTwitch(string message)
        {
            var data = Regex.Match(message, @"tv/\S*").ToString().Replace("tv/", "");

            message = Embed("Twitch Stream", "<iframe src=\"http://www.twitch.tv/" + data  + "/embed\" frameborder=\"0\" scrolling=\"no\" height=\"315\" width=\"420\"");
            return message;
        }

        private string HandleHangout(string message)
        {

            return "<iframe width=\"80%\" height=\"100\" scrolling=\"no\" frameborder=\"no\" src=\"https://w.soundcloud.com/player/?visual=true&url=http%3A%2F%2Fapi.soundcloud.com%2Ftracks%2F157576919&show_artwork=true\"></iframe>";
        }

        //template method
        private string HandleOther(string message)
        {

            return message;
        }

        private string Embed(string type, string data)
        {
            return "<div id=\"embedType\">" + type + " " + "<a id=\"embedToggle" + messageNumber + "\"> Hide </a><br></div> <div id=\"embedData" + messageNumber + "\">" + data + "</div>";
        }
    }
}