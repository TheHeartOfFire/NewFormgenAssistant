using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FormgenAssistant
{
    internal class Utils
    {
        private static Dictionary<string, string> _servers = new Dictionary<string, string>();
        private static Dictionary<string, DealerInfo> _dealerships = new Dictionary<string, DealerInfo>();

        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant";
        private static readonly string serversURL = AppData + "\\Servers.json";
        private static readonly string dealersURL = AppData + "\\Dealers.json";

        private static void GetServersJson()
        {

            Directory.CreateDirectory(AppData);
            if (!File.Exists(serversURL)) File.Create(serversURL);
            Servers = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(serversURL)) ?? new Dictionary<string, string>();
        }

        private static void SetServersJson()
        {
            string json = JsonConvert.SerializeObject(Servers);
            Directory.CreateDirectory(AppData);
            if (!File.Exists(serversURL)) File.Create(serversURL);
            File.WriteAllText(serversURL, json);

        }
        private static void GetDealersJson()
        {

            Directory.CreateDirectory(AppData);
            if (!File.Exists(dealersURL)) File.Create(dealersURL);
            Dealerships = JsonConvert.DeserializeObject<Dictionary<string, DealerInfo>>(File.ReadAllText(dealersURL)) ?? new Dictionary<string, DealerInfo>();
        }

        public static void SetDealersJson()
        {
            string json = JsonConvert.SerializeObject(Dealerships);
            Directory.CreateDirectory(AppData);
            if (!File.Exists(dealersURL)) File.Create(dealersURL);
            File.WriteAllText(dealersURL, json);
        }

        private static Dictionary<string, string> values = new Dictionary<string, string>() { { "search", "*" } };

        public static Dictionary<string, string> Servers { get => _servers; set => _servers = value; }
        internal static Dictionary<string, DealerInfo> Dealerships { get => _dealerships; set => _dealerships = value; }

        private static async Task<string> GetServersHTML()
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(values);
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
                if (!Dealerships.ContainsKey(key))
                {
                    var di = new DealerInfo(key);
                    Dealerships.Add(key, di);
                }
                _progress.Report(servers.IndexOf(server)+1);
            }
            _progress.Report(0.0);
            SetServersJson();
        }

        public static void InitAllServers()
        {

            GetServersJson();
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
