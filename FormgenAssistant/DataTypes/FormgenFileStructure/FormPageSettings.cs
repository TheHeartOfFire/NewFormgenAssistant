using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FormgenAssistant.DataTypes
{
    public class FormPageSettings
    {
        public int PageNumber { get; set; }
        public int DefaultFontSize { get; set; }
        public int LeftPrinterMargin { get; set; }
        public int RightPrinterMargin { get; set; }
        public int TopPrinterMargin { get; set; }
        public int BottomPrinterMargin { get; set; }
        public FormPageSettings(XmlAttributeCollection attributes)
        {
            if (int.TryParse(attributes[0].Value, out int parsedInt))
                PageNumber = parsedInt;

            if (int.TryParse(attributes[1].Value, out parsedInt))
                DefaultFontSize = parsedInt;

            if (int.TryParse(attributes[2].Value, out parsedInt))
                LeftPrinterMargin = parsedInt;

            if (int.TryParse(attributes[3].Value, out parsedInt))
                RightPrinterMargin = parsedInt;

            if (int.TryParse(attributes[4].Value, out parsedInt))
                TopPrinterMargin = parsedInt;

            if (int.TryParse(attributes[5].Value, out parsedInt))
                BottomPrinterMargin = parsedInt;
        }

        public void GenerateXml(XmlWriter xml)
        {
            xml.WriteAttributeString("pageNumber", PageNumber.ToString());
            xml.WriteAttributeString("defaultPoints", DefaultFontSize.ToString());
            xml.WriteAttributeString("leftPrinterMargin", LeftPrinterMargin.ToString());
            xml.WriteAttributeString("rightPrinterMargin", RightPrinterMargin.ToString());
            xml.WriteAttributeString("topPrinterMargin", TopPrinterMargin.ToString());
            xml.WriteAttributeString("bottomPrinterMargin", BottomPrinterMargin.ToString());

        }

    }
}
