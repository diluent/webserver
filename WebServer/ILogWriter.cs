using System;
using Microsoft.SqlServer.Server;

namespace WebServerLib
{
    public interface ILogWriter {
        void Info(string text);
        void Error(string text);
        void Error(Exception exception);
        void Error(string text, Exception exception);
    }
}
