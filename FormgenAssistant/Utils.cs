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

        private static void SetDealersJson()
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
                var response = await client.PostAsync("http://linux.automate.local/cgi-bin/getinfo2.cgi", content).ConfigureAwait(false);
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

        private static async Task GetServers(ProgressBar progress, IProgress<double> progress1)
        {
            var servers = ParseServersHTML(await GetServersHTML().ConfigureAwait(false));
            Thread.CurrentThread.IsBackground = false;
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)async delegate
            {
                progress.IsIndeterminate = false;
                progress.Maximum = servers.Count;
                int prog = 0;
                foreach (var server in servers)
                {
                    if (!Servers.ContainsKey(server[..server.IndexOf(' ')]))
                        Servers.Add(server[..server.IndexOf(' ')], server.Substring(server.IndexOf(' ')));
                    if (!Dealerships.ContainsKey(server[..server.IndexOf(' ')]))
                        Dealerships.Add(server[..server.IndexOf(' ')], await DealerInfo.Create(server.Substring(0, server.IndexOf(' '))).ConfigureAwait(false));

                    prog++;
                    progress1.Report(prog);
                }
                progress1.Report(0.0);

            }, null).Task.ConfigureAwait(false);
            SetDealersJson();
            SetServersJson();

        }

        public static void InitAllServers()
        {

            GetServersJson();
            GetDealersJson();
        }

        public static async Task UpdateAllServers(ProgressBar progress, IProgress<double> progress1)
        {
            try
            {
                await GetServers(progress, progress1).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "oops");
            }
        }
    }
}
