using System.Collections.Generic;

namespace ConsoleWebServer.DataModels
{
    public class Item
    {
        public string User;
        public string Message;
    }

    public class GuestBook
    {
        public List<Item> UserMessages = new List<Item>();
    }
}
