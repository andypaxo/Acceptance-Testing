using System;
using System.Text;
using System.Xml;

namespace AcceptanceTesting
{
    [LoggerName("xml")]
    public class XmlLogger : Logger
    {
        private XmlTextWriter xmlStream;
        private bool inScenario;

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
            var status = result.Status.ToString().ToLower();

            xmlStream.WriteStartElement("result");
            xmlStream.WriteAttributeString("status", status);
            xmlStream.WriteElementString("line", line);
            if (result.Exception != null)
                xmlStream.WriteElementString("exception", result.Exception);
            xmlStream.WriteEndElement();
        }

        public override void WriteScenarioStart(string scenarioName)
        {
            EndScenario();
            xmlStream.WriteStartElement("scenario");
            xmlStream.WriteAttributeString("name", scenarioName);
            inScenario = true;
        }

        private void EndScenario()
        {
            if (inScenario)
                xmlStream.WriteEndElement();
            inScenario = false;
        }

        public override void Dispose()
        {
            if (xmlStream != null)
                Close();
        }

        private void Close()
        {
            xmlStream.WriteEndElement();
            xmlStream.WriteEndDocument();
            xmlStream.Close();
        }
    }
}