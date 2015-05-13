using System.Collections.Generic;
using System.Data;

namespace ConsoleWebServer.Repository
{
    public interface ISqliteDataAdapter<T> where T : class
    {
        T Get(IDataReader reader);
        IDictionary<string, object> Set(T model);
    }
}
