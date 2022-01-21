using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FormgenAssistant.DataTypes
{
    public class CodeLineSettings
    {
        public int Order { get; set; }
        public CodeType Type { get; set; }
        public string Variable { get; set; }
        public enum CodeType
        {
            INIT,
            PROMPT,
            POST
        }
        public CodeLineSettings(XmlAttributeCollection attributes)
        {
            if (int.TryParse(attributes[0].Value, out int parsedInt))
                Order = parsedInt;

            Type = GetCodeType(attributes[1].Value);

            Variable = attributes[2].Value;
        }
        public CodeLineSettings(CodeLineSettings settings, string newName, int newIndex)
        {
            Order = newIndex;
            Type = settings.Type;
            Variable = newName;
        }
        public static CodeType GetCodeType(string type) => type switch
        {
            "INIT" => CodeType.INIT,
            "PROMPT" => CodeType.PROMPT,
            "POST" => CodeType.POST,
            _ => CodeType.PROMPT,
        };
        public static string GetCodeType(CodeType type) => type switch
        {
            CodeType.INIT => "INIT",
            CodeType.PROMPT => "PROMPT",
            CodeType.POST => "POST",
            _ => "PROMPT",
        };

        internal void GenerateXml(XmlWriter xml)
        {
            xml.WriteAttributeString("order", Order.ToString());
            xml.WriteAttributeString("type", GetCodeType(Type));
            xml.WriteAttributeString("destVariable", Variable);
        }
    }
}
