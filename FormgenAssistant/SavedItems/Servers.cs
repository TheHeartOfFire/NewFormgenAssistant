
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.SavedItems
{
    internal class Servers
    { 
        private static readonly string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant";
        private string FileName = FilePath + "\\Servers.json";
        private static Dictionary<string, string> searchTerms = new Dictionary<string, string>() { { "search", "*" } };
        public Dictionary<string, string> Content { get; set; }
        private Servers()
        {
            Content = new Dictionary<string, string>();
            Load();
        }

        private static Servers? instance = null;
        public static Servers Instance { 
            get { 
                if (instance is null) instance = new Servers(); 
                return instance;
            } 
        }

        private void Load()
        {
            Directory.CreateDirectory(FilePath);
            if (!File.Exists(this.FileName)) File.Create(this.FileName);
            this.Content = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(this.FileName)) ?? new Dictionary<string, string>();
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this.Content);
            Directory.CreateDirectory(FileName);
            if (!File.Exists(this.FileName)) File.Create(this.FileName);
            File.WriteAllText(this.FileName, json);

        }
        private static async Task<string> GetServersHTML()
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(searchTerms);
                var response = await client.PostAsync(Properties.Resources.ClientInfoReport, content).ConfigureAwait(false);
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        public async Task DownloadServers()
        {
            var servers = ParseServersHTML(await GetServersHTML().ConfigureAwait(false));

            foreach (var server in servers)
            {
                var key = server[..server.IndexOf(' ')];

                if (!Content.ContainsKey(key))
                    Content.Add(key, server[server.IndexOf(' ')..]);
            }
            Save();
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
    }
}
