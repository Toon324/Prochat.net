﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Prochat.Services
{
    public class MessageHandler
    {

        public static string HandleMessage(string message)
        {
            if (message.Contains("/r/"))
                message = HandleReddit(message);
            else if (message.StartsWith("/spotify"))
                message = HandleSpotify(message);
            else if (message.StartsWith("/gif"))
                message = HandleGifCommand(message);
            else if (message.Contains("youtu.be") || message.Contains("youtube.com/watch?"))
                message = HandleYoutube(message);
            else if (message.Contains("twitch.tv"))
                message = HandleTwitch(message);
            else if (message.Contains("soundcloud.com") && !message.Contains("oembed"))
                message = HandleSoundcloud(message);
            else if (message.Contains(".gif") || message.Contains(".gifv") || message.Contains(".png") || message.Contains(".jpg"))
                message = HandleImage(message);
            else if (message.Contains("http"))
                message = HandleLink(message);

            return message;
        }

        private static string HandleLink(string message)
        {
            var url = Regex.Match(message, @"http\S*").ToString();
            var wrapped = "<a target=\"_blank\" href=\"" + url + "\" >" + url + "</a>";

            message = message.Replace(url, wrapped);
            return message;
        }

        private static string HandleSpotify(string message)
        {
            return Embed(message, "<iframe src=\"https://embed.spotify.com/?uri=spotify:track:0ffz9KBdCb7oJkSK0W7bbf\" width=\"300\" height=\"380\" frameborder=\"0\" allowtransparency=\"true\"></iframe>");
        }

        private static string HandleYoutube(string message)
        {
            string data;

            if (message.Contains("youtu.be"))
                data = message.Substring(message.IndexOf("youtu.be/") + 9);
            else
                data = Regex.Match(message, @"v=\S*").ToString().Replace("v=", "");

            //Ensure no values are passed in
            if (data.Contains("?"))
                data = data.Substring(0, data.IndexOf("?"));

            message = Embed(message, "<embed width=\"420\" height=\"315\" src=\"http://www.youtube.com/v/" + data + "\">");

            return message;
        }

        private static string HandleImage(string message)
        {
            var url = Regex.Match(message, @"http\S*").ToString();
            url = url.Replace("<br>", ""); //Ensure no whitespace
            var embedded = "<a href=\"" + url + "\"><img src=\"" + url + "\" style=\"width: 300px; height:200px\"></a>";
            //message = message.Replace(url, Embed(WrapWithUrl(url, url), embedded));
            message = Embed(message, embedded);

            return message;
        }

        private static string HandleGifCommand(string message)
        {
            var search = message.Replace("/gif ", "").Replace(" ", "+");

            var client = new WebClient();

            var response = client.DownloadString("http://api.giphy.com/v1/gifs/search?q=" + search + "&api_key=dc6zaTOxFJmzC");
          
            if (String.IsNullOrEmpty(response))
            {
                return Embed(message, "Sorry, it seems the GIF service is temporarily down." + response + ".");
            }

            var matches = Regex.Matches(response, @"embed_url\S*us");

            if (matches.Count == 0)
                return Embed(message, "Sorry, no results found.");

            var random = new Random();
            var num = random.Next(matches.Count);

            var result = matches[num].ToString().Replace("embed_url\":\"", "").Replace("\",\"us", "").Replace("\\", "");

            return Embed(message, WrapWithIFrame(result, 300, 200));
        }

        private static string HandleTwitch(string message)
        {
            var data = Regex.Match(message, @"tv/\S*").ToString().Replace("tv/", "");

            message = Embed(message, "<iframe src=\"http://www.twitch.tv/" + data + "/embed\" frameborder=\"0\" scrolling=\"no\" height=\"315\" width=\"420\"");
            return message;
        }

        private static string HandleSoundcloud(string message)
        {
            var data = Regex.Match(message, @"http\S*soundcloud.com/\S*").ToString();


            var reader = WebAccess.Requests.GetJsonReader("http://soundcloud.com/oembed?url=" + data);

            
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();


            var s = reader.ReadLine();

            var src = Regex.Match(s, @"https.*").ToString();
            return Embed(message, "<iframe width=\"80%\" height=\"120\" scrolling=\"no\" frameborder=\"no\" src=" + src + "></iframe>");
        }

        private static string HandleCommandIFrame(string message)
        {
            var url = Regex.Match(message, @"http\S*").ToString();

            return Embed(message, WrapWithIFrame(url));
        }

        private static string HandleReddit(string message)
        {
            var sub = Regex.Match(message, @"/r/\S*").ToString();
            var url = "https://www.reddit.com" + sub;
            message = message.Replace(sub, WrapWithUrl(url, sub));
            return message;
        }

        //template method
        private static string HandleOther(string message)
        {

            return message;
        }

        private static string Embed(string type, string data)
        {
            return "<div id=\"embedType\">" + type + " " + "<a id=\"embedToggle" + Hubs.ChatHub.messageNumber + "\"> Hide </a><a id=\"embedRight" +Hubs.ChatHub.messageNumber + "\"> Right Pane </a> <br></div> <div id=\"embedData" + Hubs.ChatHub.messageNumber + "\">" + data + "</div>";
        }

        private static string WrapWithIFrame(string url, int width = 420, int height = 315)
        {
            return "<iframe width=\"" + width + "\" height=\"" + height + "\" scrolling=\"yes\" frameborder=\"no\" src=" + url + "></iframe>";
        }

        private static string WrapWithUrl(string url, string text)
        {
            return "<a target=\"_blank\" href=\"" + url + "\" >" + text + "</a>";
        }

        private static string WrapImage(string url)
        {
            return "<img src=\"" + url + "\" style=\"width: 400px; height:300px\">";
        }
    }
}