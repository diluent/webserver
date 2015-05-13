using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebServerLib
{
    public class Response
    {
        public Response()
        {
            WriteStream = s => { };
            StatusCode = 200;
            Headers = new Dictionary<string, string> { { "Content-Type", "text/html; charset=UTF-8" } };
        }

        public int StatusCode { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public Action<Stream> WriteStream { get; set; }
    }

    public class StringResponse : Response
    {
        public StringResponse(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            WriteStream = s => s.Write(bytes, 0, bytes.Length);
        }
    }

    public class EmptyResponse : Response
    {
        public EmptyResponse(int statusCode = 204)
        {
            StatusCode = statusCode;
        }
    }

    public class JsonResponse : Response
    {
        public JsonResponse(string message)
        {
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json; charset=windows-1251" } };
            var bytes = Encoding.UTF8.GetBytes(message);
            WriteStream = s => s.Write(bytes, 0, bytes.Length);
        }
    }

    public class Error404Response : Response {
        public Error404Response(string message, int statusCode = 404)
        {
            StatusCode = statusCode;
            var bytes = Encoding.UTF8.GetBytes(message);
            WriteStream = s => s.Write(bytes, 0, bytes.Length);
        }
    }

    public class ProxyResponse : Response
    {
        public ProxyResponse(string message, IDictionary<string, string> headers)
        {
            //Headers = headers;
            var bytes = Encoding.UTF8.GetBytes(message);
            WriteStream = s => s.Write(bytes, 0, bytes.Length);
        }
    }
    	
}
