
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
    internal class Dealers
    {
        private static readonly string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant";
        private readonly string FileName = FilePath + "\\Dealers.json";
        public Dictionary<string, DealerInfo> Content { get; set; }

        private const string Xpath = "//html/body/table/tbody/tr/td/table/tbody";
        private const string Xpath1 = "//html/body/table/tbody/td/table/tbody";
        private static readonly string URL = Properties.Resources.ClientInfoReport;
        private static Dictionary<string, string> request = new Dictionary<string, string>();
        private Dealers()
        {
            Content = new Dictionary<string, DealerInfo>();
            Load();
        }
        private static Dealers? instance = null;
        public static Dealers Instance
        {
            get
            {
                if (instance is null) instance = new Dealers();
                return instance;
            }
        }
        private void Load()
        {
            Directory.CreateDirectory(FilePath);
            if (!File.Exists(this.FileName)) File.Create(this.FileName);
            this.Content = JsonConvert.DeserializeObject<Dictionary<string, DealerInfo>>(File.ReadAllText(this.FileName)) ?? new Dictionary<string, DealerInfo>();
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this.Content);
            Directory.CreateDirectory(FileName);
            if (!File.Exists(this.FileName)) File.Create(this.FileName);
            File.WriteAllText(this.FileName, json);

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
    }
}
