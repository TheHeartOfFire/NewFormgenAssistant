using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FormgenAssistant.DataTypes
{
    public class FormPage
    {
        public FormPageSettings Settings { get; set; }
        public List<FormField> Fields { get; set; } = new List<FormField>();
        public FormPage(XmlNode Node)
        {
            Settings = new FormPageSettings(Node.Attributes);

            foreach(XmlNode Child in Node.FirstChild)
            {
                Fields.Add(new FormField(Child));
            }
        }

        public void GenerateXml(XmlWriter xml)
        {

            xml.WriteStartElement("pages");
            Settings.GenerateXml(xml);

            xml.WriteStartElement("fields");
            foreach(FormField Field in Fields)
            {
                xml.WriteStartElement("entry");
                Field.GenerateXml(xml);
                xml.WriteEndElement();
            }
                xml.WriteEndElement();
                xml.WriteEndElement();
        }
    }
}
