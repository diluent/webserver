using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebServerLib
{
    public class TemplateLoader
    {
        private static string _root = @"..\..\Templates\";
        private static IList<string> _extentions = new List<string>{"html"};

        public static string Root
        {
            get { return _root; }
            set
            {
                if (value != null)
                    _root = value;
            }
        }

        public static IList<string> Extensions
        {
            get { return _extentions; }
            set
            {
                if (value != null)
                    _extentions = value;
            }
        }

        public static string Load(string name) {
            try {
                var filePath =
                    _extentions.Select(ext => Directory.GetFiles(GetDirectory(), string.Format("{0}.{1}", name, ext)))
                        .First()
                        .First();
                if (!string.IsNullOrEmpty(filePath))
                using (var sr = new StreamReader(filePath)) {
                    var text = sr.ReadToEnd();
                    sr.Close();
                    return text;
                }
            }
            catch{}

            return NotFoundMessage(name);
        }

        private static string GetDirectory()
        {
            return _root;
        }

        private static string NotFoundMessage(string name)
        {
            var s = new StringBuilder();
            s.AppendLine("Файл(ы) не найден(ы):<br/>");
            foreach (var e in _extentions)
            {
                s.AppendFormat("{0}{1}.{2}<br/>", GetDirectory(), name, e);
            }
            return s.ToString();
        }
    }
}
