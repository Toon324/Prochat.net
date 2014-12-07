using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Linq;



namespace Prochat.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            message = ParseMessage(message);
            Clients.All.addNewMessageToPage(name, message);
        }

        private string ParseMessage(string message)
        {
            if (message.Contains("youtu.be"))
                message = HandleYoutube(message);
            else if (message.Contains("http"))
                message = HandleLink(message);
            
            return message;
        }

        private string HandleLink(string message)
        {
            var url = message.Substring(message.IndexOf("http"), message.IndexOf(".com")+4);
            var wrapped = "<a target=\"_blank\" href=\"" + url + "\" >" + url + "</a>";

            message = message.Replace(url, wrapped);
            return message;
        }

        private string HandleYoutube(string message)
        {
            var data = message.Substring(message.IndexOf("youtu.be") + 9);

            message = Embed("Youtube Video ", "<embed width=\"420\" height=\"315\" src=\"http://www.youtube.com/v/" + data + "\">");
            //message = "<div id=\"embedType\">Youtube Video: <a id=\"embedToggle\"> Hide </a><br></div><div id=\"embedData\"></div>"; 
            //message = "<div>Youtube Video: <br></div><embed src=\"http://www.youtube.com/v/zjnJk5V9nSM\">"; 
            
            //message = "Test";
            return message;
        }

        private string HandleOther(string message)
        {

            return message;
        }

        private string Embed(string type, string data)
        {
            return "<div id=\"embedType\">" + type + "<a id=\"embedToggle\"> Hide </a><br></div> <div id=\"embedData\">" + data + "</div>";
        }
    }
}