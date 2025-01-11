using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.SavedItems.Templates
{
    public class Templates
    {
        //Serializable Properties
        public List<Template> TemplateList {  get; set; }

        //Singleton
        [JsonIgnore]
        private static Templates? _instance;
        [JsonIgnore]
        public static Templates Instance
        {

            get => _instance ??= new Templates();

            set => _instance = value;
        }

        private Templates() 
        {
            TemplateList = []; 
        }

        //Json stuff
        private static readonly string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant";
        private static readonly string FileName = FilePath + "\\Templates.json";
        
        [JsonConstructor]
        public Templates(List<Template> templates)
        {
            TemplateList = templates;
        }

        public static void Load()
        {
            Directory.CreateDirectory(FilePath);
            if (!File.Exists(FileName)) Save();
            var json = File.ReadAllText(FileName);
            Instance = JsonConvert.DeserializeObject<Templates>(json) ?? new Templates();
        }

        public static void Save()
        {
            string json = JsonConvert.SerializeObject(Instance);
            Directory.CreateDirectory(FilePath);
            if (!File.Exists(FileName)) File.Create(FileName).Close();
            File.WriteAllText(FileName, json);

        }
    }
}
