using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FormgenAssistant.DataTypes
{
    public class PromptData
    {
        public PromptDataSettings Settings { get; set; }
        public string Message { get; set; }
        public List<string> Choices { get; set; } = new List<string>();
        public PromptData(XmlNode node)
        {
            Settings = new PromptDataSettings(node.Attributes);

            if (node.FirstChild.HasChildNodes)
                Message = node.FirstChild.FirstChild.InnerText;

            foreach (XmlNode child in node.ChildNodes)
                if(child.Name == "choices")
                    Choices.Add(child.InnerText);
        }

        internal void GenerateXml(XmlWriter xml)
        {
            xml.WriteStartElement("promptData");
            Settings.GenerateXml(xml);

            xml.WriteStartElement("promptMessage");
            xml.WriteString(Message);
            xml.WriteEndElement();

            foreach(var choice in Choices)
            {
                xml.WriteStartElement("choices");
                xml.WriteString(choice);
                xml.WriteEndElement();
            }

            xml.WriteEndElement();

        }
    }
}
