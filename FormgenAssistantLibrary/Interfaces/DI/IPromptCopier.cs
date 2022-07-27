using System.Xml;
using FormgenAssistantLibrary.DataTypes.FormgenFileStructure;

namespace FormgenAssistantLibrary.Interfaces.DI;

public interface IPromptCopier
{
    string? UUID { get; set; }
    XmlDocument? File { get; set; }
    DotFormgen? FormFile { get; set; }
    string BackupDirectory { get; }
    bool IsLoaded();
    void ReadXML(string filePath);
    string RegenerateUUID();
    string CopyIncrementer(string? input);
    void RemovePrompt(int idx);
    void ClonePrompt(int idx);
    void CloseFile();
    void SaveFile();
    void ToggleBold(int idx);
    void CopyTo(string newFile);
}