using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace ConsoleWebServer.Repository.Xml
{
    public interface IXmlRepository<T> where T : class, new() {
        T Read();
        void Write(T item);
    }

    public class XmlRepository<T> : IXmlRepository<T> where T : class, new() {
        private readonly string _filePath;
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(T));

        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

        public XmlRepository(string filePath) {
            _filePath = filePath;
        }

        public T Read() {
            _cacheLock.EnterReadLock();

            T item = null;

            try {
                if (File.Exists(_filePath)) {
                    var fs = new FileStream(_filePath, FileMode.Open);
                    item = (T) _serializer.Deserialize(fs);
                    fs.Close();
                }
            }
            finally {
                _cacheLock.ExitReadLock();
            }

            return item ?? new T();
        }

        public void Write(T item) {
            _cacheLock.EnterWriteLock();

            try {
                using (TextWriter writer = new StreamWriter(_filePath)) {
                    _serializer.Serialize(writer, item);
                    writer.Close();
                }
            }
            finally {
                _cacheLock.ExitWriteLock();
            }
        }
    }
}
