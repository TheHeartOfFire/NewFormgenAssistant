using FormgenAssistantLibrary.DataTypes;
using FormgenAssistantLibrary.Interfaces.DI;
using Newtonsoft.Json;

namespace FormgenAssistantLibrary.SavedItems;

public class Settings : ISettings
{
    //Json stuff
    private static readonly string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant";
    private static readonly string FileName = FilePath + "\\Settings.json";

    private static readonly SettingsData DefaultSettings =
        new(true, "Attn: AM Forms 300 State Street, #96020, Southlake, TX 76092");
    public SettingsData Data { get; set; }


    public void Load()
    {
        Directory.CreateDirectory(FilePath);
        if (!File.Exists(FileName)) Save();

        Data = JsonConvert.DeserializeObject<SettingsData>(File.ReadAllText(FileName)) ?? DefaultSettings;
        Data.MailingAddress ??= DefaultSettings.MailingAddress;

    }
    
    public void Save()
    {
        string json = JsonConvert.SerializeObject(Data);
        Directory.CreateDirectory(FilePath);
        if (!File.Exists(FileName)) File.Create(FileName).Close();
        File.WriteAllText(FileName, json);

    }
    
}