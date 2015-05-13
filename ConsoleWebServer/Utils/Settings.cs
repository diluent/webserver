using System.Configuration;
using WebServerLib;

namespace ConsoleWebServer.Utils
{
    public interface ISettings
    {
        string AppSetting(string key);
    }

    public class WebConfigSettings : ISettings
    {
        private ILogWriter _logWriter = new ConsoleWriter();

        public ILogWriter LogWriter
        {
            set
            {
                if (value != null)
                    _logWriter = value;
            }
        }

        public string AppSetting(string key)
        {
            var res = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(res))
                _logWriter.Error(string.Format("Не удалось прочитать параметр {0} из app.config", key));
            return res;
        }
    }
}
