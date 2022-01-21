using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FormgenAssistant.DataTypes
{
    public class DotFormgen
    {
        public DotFormgenSettings Settings { get; set; }
        public List<FormPage> Pages { get; set; } = new List<FormPage>();
        public string Title { get; set; }
        public bool TradePrompt { get; set; }
        public Format FormType { get; set; }
        public bool SalesPersonPrompt { get; set; }
        public string Username { get; set; }
        public string BillingName { get; set; }
        public List<CodeLine> CodeLines { get; set; } = new List<CodeLine>();
        public FormCategory Category { get; set; }

        public List<string> States { get; set; } = new List<string>();
        
        public enum Format
        {
            IMPACT,
            IMPACTLABELROLL,
            IMPACTLABELSHEET,
            LASERLABELSHEET,
            LEGACYIMPACT,
            LEGACYLASER,
            PDF
        }

        public enum FormCategory
        {
            AFTERMARKET,
            BUYERSGUIDE,
            COMMISSION,
            CREDITLIFEAH,
            CUSTOM,
            DEALRECAP,
            ENVELOPEDEALJACKET,
            EXTENDEDWARRANTIES,
            GAP,
            INSURANCE,
            LABEL,
            LEASE,
            MAINTENANCE,
            MEMBERAPPLICATION,
            NOTICETOCOSIGNER,
            NOTICETOCUSTOMER,
            OTHER,
            PURCHASEORDERINVOICE,
            REBATEINCENTIVE,
            RETAIL,
            STATESPECIFICDMV,
            WEOWEYOUOWEDUEBILL
        }
        public DotFormgen(XmlElement document)
        {
            Settings = new DotFormgenSettings(document.Attributes);
            foreach(XmlNode node in document.ChildNodes)
            {
                switch (node.Name)
                {
                    case "pages": Pages.Add(new FormPage(node));
                        break;
                    case "title": Title = node.InnerText;
                        break;
                    case "tradePrompt":
                        if (bool.TryParse(node.InnerText, out bool parsedBool))
                            TradePrompt = parsedBool;
                        break;
                    case "formPrintType": FormType = GetFormat(node.InnerText);
                        break;
                    case "salespersonPrompt":
                        if (bool.TryParse(node.InnerText, out parsedBool))
                            SalesPersonPrompt = parsedBool;
                        break;
                    case "username": Username = node.InnerText;
                        break;
                    case "billingName": BillingName = node.InnerText;
                        break;
                    case "codeLines": CodeLines.Add(new CodeLine(node));
                        break;
                    case "formCategory": Category = GetCategory(node.InnerText);
                        break;
                    case "validStates":
                        States.Add(node.InnerText);
                        break;
                }
            }
        }

        public CodeLine GetPrompt(int index)
        {
            var prompts = new List<CodeLine>();

            prompts = CodeLines.Where(x => x.Settings.Type == CodeLineSettings.CodeType.PROMPT).ToList();

            return prompts[index];
        }

        public FormField GetField(int Index)
        {
            var Fields = new List<FormField>();
            foreach(var page in Pages)
                foreach(var field in page.Fields)
                    Fields.Add(field);

            return Fields[Index];
        }

        public int PromptCount()
        {
            return CodeLines.Where(x => x.Settings.Type == CodeLineSettings.CodeType.PROMPT).Count();
        }

        public CodeLine ClonePrompt(CodeLine Prompt, string newName, int NewIndex)
        {
            var cl = new CodeLine(Prompt, newName, NewIndex);
            CodeLines.Add(cl);
            return cl;
        }
        public static Format GetFormat(string format) => format switch
        {
            "Pdf" => Format.PDF,
            "LegacyImpact" => Format.LEGACYIMPACT,
            _ => Format.PDF,
        }; 
        
        public static string GetFormat(Format format) => format switch
        {
            Format.PDF => "Pdf",
            Format.LEGACYIMPACT => "LegacyImpact",
            _ => "Pdf",
        };

        public static FormCategory GetCategory(string category) => category switch
        {
            "Aftermarket" => FormCategory.AFTERMARKET,
            "BuyersGuide" => FormCategory.BUYERSGUIDE,
            "Commission" => FormCategory.COMMISSION,
            "CreditLifeAH" => FormCategory.CREDITLIFEAH,
            "Custom" => FormCategory.CUSTOM,
            "DealRecap" => FormCategory.DEALRECAP,
            "EnvelopeDealJacket" => FormCategory.ENVELOPEDEALJACKET,
            "ExtendedWarranties" => FormCategory.EXTENDEDWARRANTIES,
            "Gap" => FormCategory.GAP,
            "Insurance" => FormCategory.INSURANCE,
            "Label" => FormCategory.LABEL,
            "Lease" => FormCategory.LEASE,
            "Maintenance" => FormCategory.MAINTENANCE,
            "MemberApplication" => FormCategory.MEMBERAPPLICATION,
            "NoticeToCoSigner" => FormCategory.NOTICETOCOSIGNER,
            "NoticeToCustomer" => FormCategory.NOTICETOCUSTOMER,
            "Other" => FormCategory.OTHER,
            "PurchaseOrderInvoice" => FormCategory.PURCHASEORDERINVOICE,
            "RebateIncentive" => FormCategory.REBATEINCENTIVE,
            "Retail" => FormCategory.RETAIL,
            "StateSpecificDMV" => FormCategory.STATESPECIFICDMV,
            "WeOweYouOweDueBill" => FormCategory.WEOWEYOUOWEDUEBILL,
            _ => FormCategory.OTHER,

        };

        public static string GetCategory(FormCategory category) => category switch
        {
            FormCategory.AFTERMARKET => "Aftermarket",
            FormCategory.BUYERSGUIDE => "BuyersGuide",
            FormCategory.COMMISSION => "Commission",
            FormCategory.CREDITLIFEAH => "CreditLifeAH",
            FormCategory.CUSTOM => "Custom",
            FormCategory.DEALRECAP => "DealRecap",
            FormCategory.ENVELOPEDEALJACKET => "EnvelopeDealJacket",
            FormCategory.EXTENDEDWARRANTIES => "ExtendedWarranties",
            FormCategory.GAP => "Gap",
            FormCategory.INSURANCE => "Insurance",
            FormCategory.LABEL => "Label",
            FormCategory.LEASE => "Lease",
            FormCategory.MAINTENANCE => "Maintenance",
            FormCategory.MEMBERAPPLICATION => "MemberApplication",
            FormCategory.NOTICETOCOSIGNER => "NoticeToCoSigner",
            FormCategory.NOTICETOCUSTOMER => "NoticeToCustomer",
            FormCategory.OTHER => "Other",
            FormCategory.PURCHASEORDERINVOICE => "PurchaseOrderInvoice",
            FormCategory.REBATEINCENTIVE => "RebateIncentive",
            FormCategory.RETAIL => "Retail",
            FormCategory.STATESPECIFICDMV => "StateSpecificDMV",
            FormCategory.WEOWEYOUOWEDUEBILL => "WeOweYouOweDueBill",
            _ => "Other",

        };

        public string GenerateXML()
        {
            var output = new StringBuilder();
            var sw = new StringWriterWithEncoding(output, Encoding.UTF8);
            var xml = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = true});
            
            xml.WriteStartDocument(true);

            xml.WriteStartElement("formDef");
            Settings.GenerateXML(xml);

            foreach (var page in Pages)
                page.GenerateXml(xml);

            xml.WriteStartElement("title");
            xml.WriteString(Title);
            xml.WriteEndElement();

            xml.WriteStartElement("tradePrompt");
            xml.WriteString(TradePrompt.ToString().ToLowerInvariant());
            xml.WriteEndElement();

            xml.WriteStartElement("formPrintType");
            xml.WriteString(GetFormat(FormType));
            xml.WriteEndElement();

            xml.WriteStartElement("salespersonPrompt");
            xml.WriteString(SalesPersonPrompt.ToString().ToLowerInvariant());
            xml.WriteEndElement();

            xml.WriteStartElement("username");
            xml.WriteString(Username);
            xml.WriteEndElement();

            if (BillingName != null)
            {
                xml.WriteStartElement("billingName");
                xml.WriteString(BillingName);
                xml.WriteEndElement();
            }

            foreach(var line in CodeLines)
            {
                if (line.Settings.Type == CodeLineSettings.CodeType.INIT)
                    line.GenerateXml(xml);
            }

            foreach (var line in CodeLines)
            {
                if (line.Settings.Type == CodeLineSettings.CodeType.PROMPT)
                    line.GenerateXml(xml);
            }

            foreach (var line in CodeLines)
            {
                if (line.Settings.Type == CodeLineSettings.CodeType.POST)
                    line.GenerateXml(xml);
            }


            xml.WriteStartElement("formCategory");
            xml.WriteString(GetCategory(Category));
            xml.WriteEndElement();

            foreach (var state in States)
            {
                xml.WriteStartElement("validStates");
                xml.WriteString(state);
                xml.WriteEndElement();
            }

            xml.WriteEndDocument();
            xml.Close();
            return output.ToString();
        }
    }
    public class StringWriterWithEncoding : StringWriter
    {
        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
            : base(sb)
        {
            this.m_Encoding = encoding;
        }
        private readonly Encoding m_Encoding;
        public override Encoding Encoding
        {
            get
            {
                return this.m_Encoding;
            }
        }
    }
}
