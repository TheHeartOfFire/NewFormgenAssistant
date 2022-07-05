using FormgenAssistant.SavedItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace FormgenAssistant
{
    internal class Utils
    {
        private static readonly Servers _servers = SavedItems.Servers.Instance;
        private static readonly Dealers _dealers = SavedItems.Dealers.Instance;
        public static Dictionary<string, string> Servers { 
            get 
            {
                if (_servers.Content is not null)
                    return _servers.Content;
                return new Dictionary<string, string>();
            }
            set 
            { 
                _servers.Content = value;
            } 
        }
        public static Dictionary<string, DealerInfo> Dealers
        {
            get
            {
                if (_dealers.Content is not null)
                    return _dealers.Content;
                return new Dictionary<string, DealerInfo>();
            }
            set
            {
                _dealers.Content = value;
            }
        }


        private static Dictionary<string, string> searchTerms = new Dictionary<string, string>() { { "search", "*" } };


        private static async Task<string> GetServersHTML()
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(searchTerms);
                var response = await client.PostAsync(Properties.Resources.ClientInfoReport, content).ConfigureAwait(false);
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        private static List<string> ParseServersHTML(string HTML)
        {
            var servers = new List<string>();
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(HTML);

            var Nodes = doc.DocumentNode.SelectSingleNode("//html/body").ChildNodes[5].SelectSingleNode("//tbody/tr/td/table/tbody").SelectNodes("//tr");

            foreach (var node in Nodes)
            {
                if (!node.InnerText.Contains("Password"))
                    servers.Add(node.InnerText.Trim());
            }
            string rudeBit = "\n              \n            \n            \n              \n";
            Array.ForEach(servers[0].Replace(rudeBit, "~").Split('~'), s => servers.Add(s));
            servers.RemoveAt(0);
            servers = servers.Distinct().ToList();
            return servers;
        }

        private static IProgress<double> _progress = new Progress<double>();

        private static async Task GetServers()
        {
            var servers = ParseServersHTML(await GetServersHTML().ConfigureAwait(false));

            foreach (var server in servers)
            {
                var key = server[..server.IndexOf(' ')];

                if (!Servers.ContainsKey(key))
                    Servers.Add(key, server.Substring(server.IndexOf(' ')));
                if (!Dealers.ContainsKey(key))
                {
                    var di = new DealerInfo(key);
                    Dealers.Add(key, di);
                }
                _progress.Report(servers.IndexOf(server)+1);
            }
            _progress.Report(0.0);
            _servers.Save();
        }

        public static async Task UpdateAllServers(IProgress<double> progress1)
        {
            try
            {
                _progress = progress1;
                await GetServers().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _progress.Report(0.0);
                MessageBox.Show(ex.Message, "oops");
            }
        }
    }
}
