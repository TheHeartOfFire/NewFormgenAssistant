using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FormgenAssistantLibrary.DataTypes.FormgenFileStructure;
using FormgenAssistantLibrary.Interfaces.DI;

namespace FormgenAssistantLibrary;

public class PromptCopier : IPromptCopier
{
    public string? UUID { get; set; }
    public XmlDocument? File { get; set; } = new();
    private string? _filePath;
    private string? _backupFilePath;
    public DotFormgen? FormFile { get; set; }
    private static readonly string DirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant\\FormgenBackup";

    public string BackupDirectory => DirPath + "\\" + FormFile?.Settings.UUID;

    public bool IsLoaded() => File?.DocumentElement is not null;
    public void ReadXML(string filePath)
    {
        _filePath = filePath;
        
        File!.Load(_filePath);
        
        if (File!.DocumentElement is null) return;

        FormFile = new DotFormgen(File.DocumentElement);

        UUID = FormFile.Settings.UUID;
    }

    public string RegenerateUUID()
    {
        UUID = Guid.NewGuid().ToString();
        File.DocumentElement.Attributes[1].Value = UUID;
        if (FormFile != null) FormFile.Settings.UUID = UUID;

        return UUID;
    }

    public string CopyIncrementer(string? input)
    {
        if (input == null) return input ?? string.Empty;

        var index = input.Length - 1;
        while (int.TryParse(input[index].ToString(), out _))
        {
            index--;
        }

        _ = int.TryParse(input.AsSpan(index + 1), out int number);

        number++;
        var output = string.Concat(input.AsSpan(0, index + 1), number.ToString());
        return output;

    }

    public void RemovePrompt(int idx)
    {
        FormFile?.CodeLines.Remove(FormFile.GetPrompt(idx));
    }

    public void ClonePrompt(int idx)
    {

        var item = FormFile?.GetPrompt(idx);
        var variable = item?.Settings?.Variable;

        var prompts = FormFile?.CodeLines.Where(x => x.Settings?.Type == CodeLineSettings.CodeType.PROMPT).ToList();

        if (variable != "F0")
            while (prompts != null && prompts.Exists(x => x.Settings?.Variable == variable))
                variable = CopyIncrementer(variable);

        if (item == null) return;
        if (variable == null) return;
        FormFile?.ClonePrompt(item, variable, FormFile.PromptCount());
    }

    public void CloseFile()
    {

        File = new XmlDocument();
        FormFile = null;
        _filePath = string.Empty;
    }

    public void SaveFile()
    {
        Directory.CreateDirectory(DirPath + "\\" + FormFile?.Settings.UUID);
        _backupFilePath = DirPath + "\\" + FormFile?.Settings.UUID + "\\" + DateTime.Now.ToString("mm-dd-yyyy.hh-mm-ss") + ".bak";
        File?.Save(_backupFilePath);

        var xml = FormFile?.GenerateXML();
        if (xml != null) File?.LoadXml(xml);
        if (_filePath != null) File?.Save(_filePath);
    }

    public void ToggleBold(int idx)
    {

        var item = FormFile?.GetField(idx);

        if (item?.Settings != null) item.Settings.Bold = !item.Settings.Bold;
    }

    public void CopyTo(string newFile)
    {
        var newDoc = new XmlDocument();
        newDoc.Load(newFile);
        var recipient = new DotFormgen(newDoc.DocumentElement ?? throw new InvalidOperationException());

        Directory.CreateDirectory(DirPath + "\\" + recipient.Settings.UUID);
        _backupFilePath = DirPath + "\\" + recipient.Settings.UUID + "\\" + DateTime.Now.ToString("mm-dd-yyyy.hh-mm-ss") + ".bak";
        File?.Save(_backupFilePath);

        if (FormFile?.Pages != null) recipient.Pages = FormFile?.Pages ?? throw new InvalidOperationException();
        if (FormFile?.CodeLines != null) recipient.CodeLines = FormFile?.CodeLines ?? throw new InvalidOperationException();

        newDoc.LoadXml(recipient.GenerateXML());
        newDoc.Save(newFile);
    }
}