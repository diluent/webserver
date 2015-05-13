using System;
using WebServerLib;

namespace ConsoleWebServer.Utils
{
    class ConsoleWriter : ILogWriter
    {
        public void Info(string text) {
            Console.WriteLine("{0} INFO: {1}", DateTime.Now.ToString("hh:mm:ss"), text);
        }

        public void Error(string text) {
            Console.WriteLine("{0} ERROR: {1}", DateTime.Now.ToString("hh:mm:ss"), text);
        }

        public void Error(Exception exception) {
            Console.WriteLine("{0} ERROR: {1}", DateTime.Now.ToString("hh:mm:ss"), exception.StackTrace);
        }

        public void Error(string text, Exception exception)
        {
            Console.WriteLine("{0} ERROR: {1}. {2}", DateTime.Now.ToString("hh:mm:ss"), text, exception.StackTrace);
        }
    }
}
