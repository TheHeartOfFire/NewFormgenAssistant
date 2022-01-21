using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FormgenAssistant.DataTypes
{
    public class CodeLine
    {
        public CodeLineSettings Settings { get; set; }
        public string Expression { get; set; }
        public PromptData PromptData { get; set; }
        public CodeLine(XmlNode node)
        {
            Settings = new CodeLineSettings(node.Attributes);

            if (Settings.Type != CodeLineSettings.CodeType.PROMPT) 
                Expression = node.FirstChild.InnerText;
            else
                PromptData = new PromptData(node.FirstChild);

        }
        public CodeLine(CodeLine codeLine, string newName, int newIndex)
        {
            Settings = new CodeLineSettings(codeLine.Settings, newName, newIndex);
            Expression = codeLine.Expression;
            PromptData = codeLine.PromptData;
        }

        public void GenerateXml(XmlWriter xml)
        {
            xml.WriteStartElement("codeLines");
            Settings.GenerateXml(xml);

            if (Settings.Type != CodeLineSettings.CodeType.PROMPT)
            {
                xml.WriteStartElement("expression");
                xml.WriteString(Expression);
                xml.WriteEndElement();
            }
            else
                this.PromptData.GenerateXml(xml);

            xml.WriteEndElement();
        }
    }
}
