using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace WebServerLib
{
        public class RequestContext
        {
            private readonly HttpListenerResponse _listenerResponse;

            public RequestContext(HttpListenerRequest request, HttpListenerResponse response)
            {
                _listenerResponse = response;
                Request = MapRequest(request);
            }

            private static Request MapRequest(HttpListenerRequest request)
            {
                var mapRequest = new Request
                {
                    Headers = request.Headers.ToDictionary(),
                    HttpMethod = request.HttpMethod,
                    InputStream = request.InputStream,
                    RawUrl = request.RawUrl,
                    Params = GetUrlParams(request.Url.Query),
                    PostParams = GetRequestPostData(request),
                    Query = request.Url.Query,
                    Path = request.Url.AbsolutePath,
                    Method = request.HttpMethod
                };
                
                return mapRequest;
            }

            private static IDictionary<string, string> GetUrlParams(string query)
            {
                try {
                    if (!string.IsNullOrWhiteSpace(query))
                        return ParseParams(query.Substring(1, query.Length - 1));
                }
                catch {}
                return new Dictionary<string, string>();
            }

            private static IDictionary<string, string> GetRequestPostData(HttpListenerRequest request)
            {
                if (!request.HttpMethod.ToLower().Equals("post") || !request.HasEntityBody) {
                    return null;
                }
                try {
                    using (var body = request.InputStream) {
                        using (var reader = new System.IO.StreamReader(body, request.ContentEncoding)) {
                            var data = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(data))
                                return ParseParams(data);
                        }
                    }
                }
                catch{}
                return new Dictionary<string, string>();
            }

            private static IDictionary<string, string> ParseParams(string data) {
                return data.Split(new[] {"&"}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Split('='))
                    .ToDictionary(sp => sp[0].ToLower(), sp => sp[1].ToLower());
            }

            public object GetParam(string key) {
                return Request.Params[key];
            }

            public object PostParam(string key)
            {
                return Request.PostParams[key];
            }

            public Request Request { get; private set; }

            public void Respond(Response response)
            {
                foreach (var header in response.Headers.Where(r => r.Key != "Content-Type"))
                {
                    _listenerResponse.AddHeader(header.Key, header.Value);
                }

                _listenerResponse.ContentType = response.Headers["Content-Type"];
                _listenerResponse.StatusCode = response.StatusCode;

                using (var output = _listenerResponse.OutputStream)
                {
                    response.WriteStream(output);
                }
            }

            public void RespondEmpty()
            {
                Respond(new EmptyResponse());
            }

            public void Respond(string response)
            {
                Respond(new StringResponse(response));
            }

            public void Respond(string response, IDictionary<string, string> headers)
            {
                Respond(new ProxyResponse(response, headers));
            }

            public void RespondHtmlTemplate(string name) {
                var answer = TemplateLoader.Load(name);
                Respond(answer);
            }

            public void RespondJson(object data)
            {
               var answer = new JavaScriptSerializer().Serialize(new { data });
               Respond(new JsonResponse(answer));
            }

            public void RespondNotFound(string response) {
                Respond(new Error404Response(response, 404));
            }
        }
}
