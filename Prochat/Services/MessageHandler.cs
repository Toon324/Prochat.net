using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            else if (Regex.Match(message, @"/gif").Success)
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

            message = Embed("Youtube Video", "<embed width=\"420\" height=\"315\" src=\"http://www.youtube.com/v/" + data + "\">");

            return message;
        }

        private static string HandleImage(string message)
        {
            var url = Regex.Match(message, @"http\S*").ToString();
            url = url.Replace("<br>", ""); //Ensure no whitespace
            var embedded = "<img src=\"" + url + "\" style=\"width: 300px; height:200px\">";
            message = message.Replace(url, Embed(WrapWithUrl(url, url), embedded));

            return message;
        }

        private static string HandleGifCommand(string message)
        {
            var search = message.Replace("/gif ", "").Replace(" ", "+");


            var reader = WebAccess.Requests.GetJsonReader("http://api.giphy.com/v1/gifs/search?q=" + search + "&api_key=dc6zaTOxFJmzC");


            var response = reader.ReadLine();

            if (String.IsNullOrEmpty(response))
            {
                return Embed(message, "Sorry, no results found :(");
            }

            var matches = Regex.Matches(response, @"embed_url\S*us");

            var random = new Random();
            var num = random.Next(matches.Count);

            var result = matches[num].ToString().Replace("embed_url\":\"", "").Replace("\",\"us", "").Replace("\\", "");

            return Embed(message, result.Equals("") ? "Sorry, no matches found :(" : WrapWithIFrame(result, 300, 200));
        }

        private static string HandleTwitch(string message)
        {
            var data = Regex.Match(message, @"tv/\S*").ToString().Replace("tv/", "");

            message = Embed("Twitch Stream", "<iframe src=\"http://www.twitch.tv/" + data + "/embed\" frameborder=\"0\" scrolling=\"no\" height=\"315\" width=\"420\"");
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
            return Embed("Soundcloud", "<iframe width=\"80%\" height=\"120\" scrolling=\"no\" frameborder=\"no\" src=" + src + "></iframe>");
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
            return "<div id=\"embedType\">" + type + " " + "<a id=\"embedToggle" + Hubs.ChatHub.messageNumber + "\"> Hide </a><br></div> <div id=\"embedData" + Hubs.ChatHub.messageNumber + "\">" + data + "</div>";
        }

        private static string WrapWithIFrame(string url)
        {
            return WrapWithIFrame(url, 420, 315);
        }

        private static string WrapWithIFrame(string url, int width, int height)
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