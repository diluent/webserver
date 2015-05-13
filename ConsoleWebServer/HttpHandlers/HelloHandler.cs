using System;
using WebServerLib;

namespace ConsoleWebServer.HttpHandlers
{
    internal class HelloHandler {
        public static Action<RequestContext> HelloWorld = context => {
            const string message = "Hello World!";
            context.Respond(message);
        };
    }
}
