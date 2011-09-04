using System;
using System.Text;
using System.Xml;

namespace AcceptanceTesting
{
    [LoggerName("xml")]
    public class XmlLogger : Logger
    {
        private XmlTextWriter xmlStream;

        public XmlLogger()
        {
            var outputStream = Console.OpenStandardOutput();
            xmlStream = new XmlTextWriter(outputStream, new UTF8Encoding(false))
            {
                Formatting = Formatting.Indented,
            };
            xmlStream.WriteStartDocument();
            xmlStream.WriteStartElement("doc");
        }

        public override void WriteResult(string line, StepResult result)
        {
            var status =
                result.Exception != null ? "fail" :
                result == StepResult.Ok ? "pass" :
                result == StepResult.NotFound ? "notfound" :
                result == StepResult.Ignored ? "ignored" :
                "unknown";

            xmlStream.WriteStartElement("result");
            xmlStream.WriteAttributeString("status", status);
            xmlStream.WriteElementString("line", line);
            if (result.Exception != null)
                xmlStream.WriteElementString("exception", result.Exception);
            xmlStream.WriteEndElement();
        }

        public override void Dispose()
        {
            if (xmlStream != null)
            {
                xmlStream.WriteEndElement();
                xmlStream.WriteEndDocument();
                xmlStream.Close();
            }

            Console.ReadLine();
        }
    }
}