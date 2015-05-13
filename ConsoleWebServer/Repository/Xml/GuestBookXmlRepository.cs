using ConsoleWebServer.DataModels;

namespace ConsoleWebServer.Repository.Xml
{
    public class GuestBookXmlRepository : IGuestBookRepository
    {
        private readonly IXmlRepository<GuestBook> _xmlRepository;

        private GuestBook _collection;

        public GuestBookXmlRepository(IXmlRepository<GuestBook> xmlRepository)
        {
            _xmlRepository = xmlRepository;
        }

        public GuestBook Select()
        {
            return _collection ?? _xmlRepository.Read();
        }

        public void Insert(Item item)
        {
            if (_collection == null)
                _collection = _xmlRepository.Read();

            _collection.UserMessages.Add(item);
            _xmlRepository.Write(_collection);
        }
    }
}
