using ConsoleWebServer.DataModels;
using ConsoleWebServer.Repository.SQLite;
using ConsoleWebServer.Repository.Xml;
using ConsoleWebServer.Utils;

namespace ConsoleWebServer.Repository
{
    public interface IGuestBookRepositoryFactory
    {
        IGuestBookRepository GetRepository();
    }

    class GuestBookRepositoryFactory : IGuestBookRepositoryFactory
    {
        private readonly ISettings _settings;

        public GuestBookRepositoryFactory(ISettings settings)
        {
            _settings = settings;
        }

        public IGuestBookRepository GetRepository()
        {
            switch (_settings.AppSetting("baseType"))
            {
                case "sqlite":
                    return
                        new GuestBookSqLiteRepository(
                            new SqLiteRepository<Item>(_settings.AppSetting("sqliteConnectionString")));
                default:
                    return new GuestBookXmlRepository(new XmlRepository<GuestBook>(_settings.AppSetting("xmlFilePath")));
            }
        }
    }
}
