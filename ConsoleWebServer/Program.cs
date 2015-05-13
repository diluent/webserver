using System;
using ConsoleWebServer.HttpHandlers;
using ConsoleWebServer.Utils;
using WebServerLib;

namespace ConsoleWebServer
{
    class Program
    {
        private static void Main(string[] args)
        {
            var writer = new ConsoleWriter();
            var settings = new WebConfigSettings();
            TemplateLoader.Root = settings.AppSetting("templateDirectory");

            try
            {
                var weburl = settings.AppSetting("webServerAddress");
                using (var webServer = new WebServer(weburl, writer))
                {
                    InitRoutes(webServer);
                    webServer.Start();
                    Console.ReadLine();
                    webServer.Stop();
                }
            }
            catch (Exception e)
            {
                writer.Error(e);
            }
            Console.ReadLine();
        }

        private static void InitRoutes(IWebServer ws)
        {
            ws.AddRouteAction(new RouteAction(
                    route: "/hello/",
                    handler: HelloHandler.HelloWorld));

            ws.AddRouteAction(new RouteAction(
                    route: "/proxy/", 
                    handler: ProxyHandler.ProxyUrl));

            ws.AddRouteAction(new RouteAction(
                    route: "/guestbook/", 
                    handler: GuestBookHandler.Index,
                    method: Method.GET));

            ws.AddRouteAction(new RouteAction(
                    route: "/guestbook/jsonlist/",
                    handler: GuestBookHandler.Get, 
                    method: Method.GET));

            ws.AddRouteAction(new RouteAction(
                    route: "/guestbook/", 
                    handler: GuestBookHandler.Add,
                    method: Method.POST));
        }
    }

}
