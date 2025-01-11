using Newtonsoft.Json;
using System;
using System.IO;

namespace FormgenAssistant.SavedItems
{
    public class Settings
    {
        //Serializable Properties
        public bool NotesCopyAll { get; set; }
        public bool SelectNewTemplate { get; set; }
        public string MailingAddress { get; set; }

        //Singleton
        [JsonIgnore]
        private static Settings? _instance;
        [JsonIgnore]
        public static Settings Instance { 

            get => _instance ??= new Settings();

            set => _instance = value; 
        }

        //Default stuff
        [JsonIgnore]
        private static readonly Settings DefaultSettings = new()
        {
            NotesCopyAll = true,
            MailingAddress = "Attn: AM Forms 300 State Street, #96020, Southlake, TX 76092"
        };
        private Settings() { }
        //Json stuff
        private static readonly string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant";
        private static readonly string FileName = FilePath + "\\Settings.json";
        [JsonConstructor]
        public Settings(bool notesCopyAll, string mailingAddress)
        {
            NotesCopyAll = notesCopyAll;
            MailingAddress = mailingAddress;
        }
        public static void Load()
        {
            Directory.CreateDirectory(FilePath);
            if (!File.Exists(FileName)) Save();

            Instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FileName)) ?? DefaultSettings;
            Instance.MailingAddress ??= DefaultSettings.MailingAddress;

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
