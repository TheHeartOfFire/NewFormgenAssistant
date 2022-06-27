using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.SavedItems
{
    public class Settings
    {
        //Serializable Properties
        public bool Notes_CopyAll { get; set; }

        //Singleton
        [JsonIgnore]
        private static Settings instance = null;
        [JsonIgnore]
        public static Settings Instance { 

            get 
            {
                if (instance is null) instance = new Settings();
                return instance;
            }

            set => instance = value; 
        }

        //Default stuff
        [JsonIgnore]
        private static Settings defaultSettings = new()
        {
            Notes_CopyAll = true
        };
        private Settings() { }
        //Json stuff
        private static readonly string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant";
        private static readonly string FileName = FilePath + "\\Settings.json";
        [JsonConstructor]
        public Settings(bool notes_CopyAll)
        {
            Notes_CopyAll = notes_CopyAll;
        }
        public static void Load()
        {
            Directory.CreateDirectory(FilePath);
            if (!File.Exists(FileName)) Save();

            Instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FileName)) ?? defaultSettings;
            
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
