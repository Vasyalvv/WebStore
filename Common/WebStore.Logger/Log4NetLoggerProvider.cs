using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string _ConfigurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new();

        public Log4NetLoggerProvider(string ConfigurationFile) => _ConfigurationFile = ConfigurationFile;
        public ILogger CreateLogger(string Category) =>
            _Loggers.GetOrAdd(Category, category =>
             {      
                 var xml = new XmlDocument();

                 string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                 StringBuilder stringBuilder = new StringBuilder();
                 using (StreamReader sr = new StreamReader(_ConfigurationFile))
                 {
                     stringBuilder.Append( sr.ReadToEnd());
                 }
                 if (stringBuilder.ToString().StartsWith(_byteOrderMarkUtf8)) { stringBuilder.Remove(0, _byteOrderMarkUtf8.Length); }

                 xml.LoadXml(stringBuilder.ToString());
                 //xml.Load(_ConfigurationFile);
                 return new Log4NetLogger(category, xml["log4net"]);
             });

        public void Dispose() => _Loggers.Clear();
    }
}
