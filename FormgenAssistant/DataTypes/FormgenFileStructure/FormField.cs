using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FormgenAssistant.DataTypes
{
    public class FormField
    {
        public FormFieldSettings Settings { get; set; }
        public string Expression { get; set; }
        public string SampleData { get; set; }
        public FormatOption FormattingOption { get; set; }

        public enum FormatOption
        {
            NONE,
            EMPTYZEROPRINTSNOTHING,
            EMPTYFIELDPRINTS0,
            NUMBERSASWORDS,
            NAIFBLANK
        }
        public FormField(XmlNode node)
        {
            Settings = new FormFieldSettings(node.ChildNodes[1].Attributes);
            Expression = node.ChildNodes[1].ChildNodes[0].InnerText;
            SampleData = node.ChildNodes[1].ChildNodes[1].InnerText;
            FormattingOption = GetFormatOption(node.ChildNodes[1].ChildNodes[2].InnerText);
        }
        public static FormatOption GetFormatOption(string option) => option switch
        {
            "None" => FormatOption.NONE,
            "EmptyZeroPrintsNothing" => FormatOption.EMPTYZEROPRINTSNOTHING,
            "BlankPrintsZero" => FormatOption.EMPTYFIELDPRINTS0,
            "NumberAsWords" => FormatOption.NUMBERSASWORDS,
            "NAIfBlank" => FormatOption.NAIFBLANK,
            _ => FormatOption.NONE,
        };

        public static string GetFormatOption(FormatOption option) => option switch
        {
            FormatOption.NONE => "None",
            FormatOption.EMPTYZEROPRINTSNOTHING => "EmptyZeroPrintsNothing",
            FormatOption.EMPTYFIELDPRINTS0 => "BlankPrintsZero",
            FormatOption.NUMBERSASWORDS => "NumberAsWords",
            FormatOption.NAIFBLANK => "NAIfBlank",
            _ => "None",
        };

        public void GenerateXml(XmlWriter xml)
        {
            xml.WriteStartElement("key");
            xml.WriteString(Settings.ID.ToString());
            xml.WriteEndElement();

            xml.WriteStartElement("value");
            Settings.GenerateXml(xml);

                 xml.WriteStartElement("expression");
                 xml.WriteString(Expression);
                 xml.WriteEndElement();

                 xml.WriteStartElement("sampleData");
                 xml.WriteString(SampleData);
                 xml.WriteEndElement();

                 xml.WriteStartElement("formatOption");
                 xml.WriteString(GetFormatOption(FormattingOption));
                 xml.WriteEndElement();

            xml.WriteEndElement();

        }
    }
}
