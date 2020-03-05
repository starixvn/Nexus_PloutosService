using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Newtonsoft.Json;
using Ploutos.Common.Const;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Ploutos.Common.Utils
{
#pragma warning disable S4142
    public class Util : IUtil
    {
        public const string RootElementName = "root";

        public string BuildLogMessage(Exception exception)
        {
            return new LogMessage(exception.Message)
                        .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                        .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                        .ToString();
        }

        public T ConvertJsonResponse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public DateTime GetCurrentTimeAtGMTMinus4()
        {
            return DateTime.UtcNow.AddHours(Consts.GmtMinus4);
        }

        public string SerializeList<T>(IEnumerable<T> list)
        {
            if (list == null || !list.Any())
            {
                return string.Empty;
            }

            var xmlSettings = new XmlWriterSettings
            {
                Indent = false,
                OmitXmlDeclaration = true,
                NewLineHandling = NewLineHandling.None
            };

            var xmlNameSpace = new XmlSerializerNamespaces();
            xmlNameSpace.Add(string.Empty, string.Empty);

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings))
            {
                var serializer = new XmlSerializer(list.GetType(), new XmlRootAttribute(RootElementName));
                serializer.Serialize(xmlWriter, list, xmlNameSpace);

                return stringWriter.ToString();
            }
        }

        public List<List<T>> SplitToBatches<T>(List<T> data, int batchSize)
        {
            return data.Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(grp => grp.Select(x => x.Value).ToList())
                    .ToList();
        }
    }
}
