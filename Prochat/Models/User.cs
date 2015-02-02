using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prochat
{
    public class User
    {
        public string UserName { get; set; }
        public string Group { get; set; }
        public string Room { get; set; }
        public List<string> RoomsList { get; set; }
    }
}