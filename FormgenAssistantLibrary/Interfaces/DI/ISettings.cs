using FormgenAssistantLibrary.DataTypes;

namespace FormgenAssistantLibrary.Interfaces.DI;

public interface ISettings
{
    SettingsData Data { get; set; }
    void Load();
    void Save();
}