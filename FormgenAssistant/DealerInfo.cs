using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant
{
    internal class DealerInfo
    {
        internal Dictionary<string, string> Companies { get; set; }
        internal string ServerID { get; set; }
        internal string DAC { get; set; }
        internal string HACLOC { get; set; }
        internal string AMPSVersion { get; set; }
        internal string COBOLVersion { get; set; }
        internal string COBOLLicense { get; set; }
        internal string COBOLUsers { get; set; }

        private readonly string URL = "http://linux.automate.local/cgi-bin/getinfo2.cgi";
        private readonly Dictionary<string, string> request = new Dictionary<string, string>();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DealerInfo(string ServerID)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this.ServerID = ServerID;
            Companies = new Dictionary<string, string>();
            request.Add("search", ServerID);
        }

        public async static Task<DealerInfo> Create(string ServerID)
        {
            var output = new DealerInfo(ServerID);
            output.ParseServerHTML(await output.GetServerHTML().ConfigureAwait(false));
            return output;
        }

        public async Task<string> GetServerHTML()
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(request);
                var response = await client.PostAsync(URL, content).ConfigureAwait(false);
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        private List<string> ParseServerHTML(string HTML)
        {
            var servers = new List<string>();
            var doc = new HtmlDocument();
            doc.LoadHtml(HTML);
            ParseCompanies(doc);
            ParseSysInfo(doc);
            ParseAMPSInfo(doc);
            ParseCOBOLInfo(doc);

            return servers;
        }

        private void ParseCompanies(HtmlDocument doc)
        {
            var CompanyData = doc.DocumentNode.SelectNodes("//html/body/table/tbody/tr/td/table/tbody")[0].InnerText.Contains("EC2") ||
                doc.DocumentNode.SelectNodes("//html/body/table/tbody/tr/td/table/tbody")[0].InnerText.Contains("VM") ?
                doc.DocumentNode.SelectNodes("//html/body/table/tbody/tr/td/table/tbody")[1].ChildNodes :
                doc.DocumentNode.SelectNodes("//html/body/table/tbody/tr/td/table/tbody")[0].ChildNodes;
            CompanyData.RemoveAt(0);

            foreach (var node in CompanyData)
            {
                if (node.NodeType == HtmlNodeType.Element && node != CompanyData.First(x => x.NodeType == HtmlNodeType.Element))
                {
                    Companies.Add(node.ChildNodes[1].InnerText.Trim(), node.ChildNodes[3].InnerText.Trim());
                }
            }

        }
        private void ParseSysInfo(HtmlDocument doc)
        {
            var SystemInfo = doc.DocumentNode.SelectNodes("//html/body/table/tbody/td/table/tbody")[0].ChildNodes;

            foreach (var node in SystemInfo)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.ChildNodes[1].InnerText.Trim().Equals("DAC"))
                        DAC = node.ChildNodes[3].InnerText.Trim();
                    if (node.ChildNodes[1].InnerText.Trim().Equals("HAC/LOC"))
                        HACLOC = node.ChildNodes[3].InnerText.Trim();
                }
            }
        }
        private void ParseAMPSInfo(HtmlDocument doc)
        {
            var AMPSInfo = doc.DocumentNode.SelectNodes("//html/body/table/tbody/td/table/tbody")[1].ChildNodes;

            foreach (var node in AMPSInfo)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.ChildNodes[1].InnerText.Trim().Equals("AMPS Version"))
                        AMPSVersion = node.ChildNodes[3].InnerText.Trim();
                }
            }
        }
        private void ParseCOBOLInfo(HtmlDocument doc)
        {
            var COBOLInfo = doc.DocumentNode.SelectNodes("//html/body/table/tbody/td/table/tbody")[2].ChildNodes;

            foreach (var node in COBOLInfo)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    if (node.ChildNodes[1].InnerText.Trim().Equals("COBOL Version"))
                        COBOLVersion = node.ChildNodes[3].InnerText.Trim();
                    if (node.ChildNodes[1].InnerText.Trim().Equals("COBOL License"))
                        COBOLLicense = node.ChildNodes[3].InnerText.Trim();
                    if (node.ChildNodes[1].InnerText.Trim().Equals("COBOL Users"))
                        COBOLUsers = node.ChildNodes[3].InnerText.Trim();
                }
            }
        }
    }
}
