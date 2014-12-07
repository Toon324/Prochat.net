﻿using System;
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
            if (message.Contains("youtu.be") || message.Contains("youtube.com/watch?"))
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
            string data;

            //Ensure no passed in arguments
            if (message.Contains("?"))
                message = message.Substring(0, message.IndexOf("?"));

            if (message.Contains("youtu.be"))
                data = message.Substring(message.IndexOf("youtu.be") + 9);
            else
                data = message.Substring(message.IndexOf("?v=") + 3);

            message = Embed("Youtube Video ", "<embed width=\"420\" height=\"315\" src=\"http://www.youtube.com/v/" + data + "\">");
   
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