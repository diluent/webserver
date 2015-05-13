using System.Collections.Generic;
using System.Data;
using ConsoleWebServer.DataModels;

namespace ConsoleWebServer.Repository.SQLite
{
    public class GuestBookSqLiteRepository : IGuestBookRepository
    {
        public class Adapter : ISqliteDataAdapter<Item>
        {
            public Item Get(IDataReader reader)
            {
                return new Item
                {
                    User = reader["user"].ToString(),
                    Message = reader["message"].ToString()
                };
            }

            public IDictionary<string, object> Set(Item model)
            {
                return new Dictionary<string, object>
                {
                    {"user", model.User},
                    {"message", model.Message}
                };
            }
        }

        private readonly ISqLiteRepository<Item> _repository;
        private readonly Adapter _adapter = new Adapter();

        public GuestBookSqLiteRepository(ISqLiteRepository<Item> repository)
        {
            _repository = repository;
        }

        public GuestBook Select()
        {
            const string query = "SELECT User, Message FROM Messages";
            return new GuestBook
            {
                UserMessages = (List<Item>)_repository.Select(query, _adapter)
            };
        }

        public void Insert(Item item)
        {
            const string query = "INSERT INTO Messages (User,Message) VALUES(@user, @message);";
            _repository.Insert(query, item, _adapter);
        }
    }
}
