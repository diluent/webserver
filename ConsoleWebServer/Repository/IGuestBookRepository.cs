using ConsoleWebServer.DataModels;

namespace ConsoleWebServer.Repository
{
    public interface IGuestBookRepository
    {
        GuestBook Select();
        void Insert(Item item);
    }
}
