namespace Ploutos.GlobalComJobs.Extensions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using Ploutos.GlobalComJobs.Models;

    public static class XmlHelper
    {
        #pragma warning disable S4142

        private static readonly XmlSerializer serializer =
                new XmlSerializer(typeof(List<UpdateBetTicket>), new XmlRootAttribute("root"));
        public static string Serialize<T>(List<T> list)
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
                serializer.Serialize(xmlWriter, list, xmlNameSpace);
                return stringWriter.ToString();
            }
        }
    }
}
