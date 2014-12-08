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

        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string name, string message)
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

                Send("Prochat", "Other links will simply be linked in the message");
                Send("Prochat", "http://www.google.com");
               
                
            }
            else if (message.Contains("youtu.be") || message.Contains("youtube.com/watch?"))
                message = HandleYoutube(message);
            else if (message.Contains(".png") || message.Contains(".jpg") || message.Contains(".gif"))
                message = HandleImage(message);
            else if (message.Contains("http"))
                message = HandleLink(message);
            
            return message;
        }

        private string HandleLink(string message)
        {
            var url = message.Substring(message.IndexOf("http:"), message.IndexOf(".com")+4);
            var wrapped = "<a target=\"_blank\" href=\"" + url + "\" >" + url + "</a>";

            message = message.Replace(url, wrapped);
            return message;
        }

        private string HandleYoutube(string message)
        {
            string data;

            //Ensure no passed in arguments
            if (message.Contains("?"))
                message = message.Substring(0, message.IndexOf("?"));

            if (message.Contains("youtu.be"))
                data = message.Substring(message.IndexOf("youtu.be") + 9);
            else
                data = message.Substring(message.IndexOf("v=") + 2);

            message = Embed("Youtube Video ", "<embed width=\"420\" height=\"315\" src=\"http://www.youtube.com/v/" + data + "\">");
   
            return message;
        }

        private string HandleImage(string message)
        {
            var url = Regex.Match(message, @"http://.+\..../.+\....").ToString();
            var embedded = "<img src=\"" + url + "\" style=\"width: 400px; height:300px\">";
            message = message.Replace(url, Embed("Image ", embedded));

            return message;
        }

        
        //template method
        private string HandleOther(string message)
        {

            return message;
        }

        private string Embed(string type, string data)
        {
            return "<div id=\"embedType\">" + type + "<a id=\"embedToggle" + messageNumber + "\"> Hide </a><br></div> <div id=\"embedData" + messageNumber + "\">" + data + "</div>";
        }
    }
}