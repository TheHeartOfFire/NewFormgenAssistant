using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace FormgenAssistant.SavedItems
{
    public class Settings
    {
        //Serializable Properties
        public bool NotesCopyAll { get; set; }
        public bool SelectNewTemplate { get; set; }
        public bool AlwaysOnTop { get; set; }
        public Address? MailingAddress { get; set; }

        public class Address(string name, string address, string city, string state, string zip)
        {
            public string Name { get; } = name;
            public string Street { get; } = address;
            public string City { get; } = city;
            public string State { get; } = state;
            public string PostalCode { get; } = zip;

            public string Print()
            {
                var sb = new StringBuilder();
                sb.AppendLine(Name);
                sb.AppendLine(Street);
                sb.AppendLine($"{City}, {State} {PostalCode}");
                return sb.ToString();
            }

            public string Flat() => $"{Name} {Street} {City}, {State} {PostalCode}";

            public static implicit operator string(Address mailingAddress) => new StringBuilder()
                .AppendLine(mailingAddress.Name)
                .AppendLine(mailingAddress.Street)
                .AppendLine($"{mailingAddress.City}, {mailingAddress.State} {mailingAddress.PostalCode}")
                .ToString();
        }

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
            AlwaysOnTop = false,
            NotesCopyAll = true,
            MailingAddress = new Address("Attn: A/M Forms (Sue)", 
                                         "131 Griffis Rd", 
                                         "Gloversville", "NY", "12078")
        };
        private Settings() { }
        //Json stuff
        private static readonly string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant";
        private static readonly string FileName = FilePath + "\\Settings.json";
        [JsonConstructor]
        public Settings(bool notesCopyAll, bool alwaysOnTop, Address mailingAddress)
        {
            AlwaysOnTop = alwaysOnTop;
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
