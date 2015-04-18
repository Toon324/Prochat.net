using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prochat.Models
{
    public class ChatHistoryModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
    }
}