using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebServerLib
{
    public class Request
    {
        public string HttpMethod { get; set; }
        public IDictionary<string, IEnumerable<string>> Headers { get; set; }
        public Stream InputStream { get; set; }
        public string RawUrl { get; set; }
        public string Path { get; set; }
        public string Query { get; set; }
        public IDictionary<string, string> Params { get; set; }
        public string Method { get; set; }
        public IDictionary<string, string> PostParams { get; set; }
        public int ContentLength
        {
            get { return int.Parse(Headers["Content-Length"].First()); }
        }
    }
}
