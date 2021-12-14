using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant
{
    public class DealerInfo
    {

        public Dictionary<string, string> Companies { get; set; }
        public string ServerID { get; set; }
        public string DAC { get; set; }
        public string HACLOC { get; set; }
        public string AMPSVersion { get; set; }
        public string COBOLVersion { get; set; }
        public string COBOLLicense { get; set; }
        public string COBOLUsers { get; set; }

        private const string Xpath = "//html/body/table/tbody/tr/td/table/tbody";
        private const string Xpath1 = "//html/body/table/tbody/td/table/tbody";
        private static readonly string URL = Properties.Resources.ClientInfoReport;
        private static Dictionary<string, string> request = new Dictionary<string, string>();

        public DealerInfo(string serverID)
        {
            request = new Dictionary<string, string>() { { "search", serverID } };

            ServerID = serverID;

            var html = GetServerHTML().Result;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            Companies = ParseCompanies(doc);

            var sys = ParseSysInfo(doc);
            DAC = sys[0];
            HACLOC = sys[1];

            AMPSVersion = ParseAMPSInfo(doc);

            var COBOL = ParseCOBOLInfo(doc);
            COBOLVersion = COBOL[0];
            COBOLLicense = COBOL[1];
            COBOLUsers = COBOL[2];

        }

        [JsonConstructor]
        public DealerInfo(Dictionary<string,string> companies, string serverID, string dAC, string hACLOC, string aMPSVersion, string cOBOLVersion, string cOBOLLicense, string cOBOLUsers)
        {
            Companies = companies;
            ServerID = serverID;
            DAC = dAC;
            HACLOC = hACLOC;
            AMPSVersion = aMPSVersion;
            COBOLVersion = cOBOLVersion;
            COBOLLicense = cOBOLLicense;
            COBOLUsers = cOBOLUsers;
        }

        public static async Task<string> GetServerHTML()
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(request);

                var response = await client.PostAsync(URL, content);
                var HTML = await response.Content.ReadAsStringAsync();

                return HTML;
            }
        }


        private static Dictionary<string,string> ParseCompanies(HtmlDocument doc)
        {
            var CompanyData = doc.DocumentNode.SelectNodes(Xpath)[0].InnerText.Contains("EC2") ||
                doc.DocumentNode.SelectNodes(Xpath)[0].InnerText.Contains("VM") ?
                doc.DocumentNode.SelectNodes(Xpath)[1].ChildNodes :
                doc.DocumentNode.SelectNodes(Xpath)[0].ChildNodes;
            CompanyData.RemoveAt(0);

            var output = new Dictionary<string, string>();
            foreach (var node in CompanyData)
            {
                if (node.NodeType == HtmlNodeType.Element && node != CompanyData.First(x => x.NodeType == HtmlNodeType.Element))
                {
                    output.Add(node.ChildNodes[1].InnerText.Trim(), node.ChildNodes[3].InnerText.Trim());
                }
            }
            return output;
        }
        private static string[] ParseSysInfo(HtmlDocument doc)
        {
            var SystemInfo = doc.DocumentNode.SelectNodes(Xpath1)[0].ChildNodes;
            string[] output = new string[2];
            foreach (var node in SystemInfo)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.ChildNodes[1].InnerText.Trim().Equals("DAC"))
                        output[0] = node.ChildNodes[3].InnerText.Trim();
                    if (node.ChildNodes[1].InnerText.Trim().Equals("HAC/LOC"))
                        output[1] = node.ChildNodes[3].InnerText.Trim();
                }
            }
            return output;
        }
        private static string ParseAMPSInfo(HtmlDocument doc)
        {
            var AMPSInfo = doc.DocumentNode.SelectNodes(Xpath1)[1].ChildNodes;
            string output = string.Empty;
            foreach (var node in AMPSInfo)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.ChildNodes[1].InnerText.Trim().Equals("AMPS Version"))
                        output = node.ChildNodes[3].InnerText.Trim();
                }
            }

            return output;
        }
        private static string[] ParseCOBOLInfo(HtmlDocument doc)
        {
            var COBOLInfo = doc.DocumentNode.SelectNodes(Xpath1)[2].ChildNodes;
            string[] output = new string[3];
            foreach (var node in COBOLInfo)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.ChildNodes[1].InnerText.Trim().Equals("COBOL Version"))
                        output[0] = node.ChildNodes[3].InnerText.Trim();
                    if (node.ChildNodes[1].InnerText.Trim().Equals("COBOL License"))
                        output[1] = node.ChildNodes[3].InnerText.Trim();
                    if (node.ChildNodes[1].InnerText.Trim().Equals("COBOL Users"))
                        output[2] = node.ChildNodes[3].InnerText.Trim();
                }
            }
            return output;
        }

    }

}
