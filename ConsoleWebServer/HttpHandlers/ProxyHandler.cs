using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebServerLib;

namespace ConsoleWebServer.HttpHandlers
{
    internal class ProxyHandler
    {
        public static Action<RequestContext> ProxyUrl = context => {
            var url = context.GetParam("url").ToString();

            using (var wb = new WebClient())
            {
                var answer = wb.DownloadString(url);

                var headers = wb.ResponseHeaders
                    .Cast<string>()
                    .Where(x => x == "Content-Encoding")
                    .ToDictionary(hname => hname, hname => wb.ResponseHeaders[hname]);
                    //.Where(x => x.Key == "Content-Encoding" || x.Key == "Content-Type");


                context.Respond(answer, headers);
                //Console.WriteLine("h=" + wb.ResponseHeaders.Count);
                //foreach(var h in wb.ResponseHeaders)
                    
            }
        };
    }
}
