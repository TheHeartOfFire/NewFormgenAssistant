using AMFormsCST.Core.Types.FormgenFileStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static AMFormsCST.Core.Types.FormgenFileStructure.CodeLineSettings;
using static AMFormsCST.Core.Types.FormgenFileStructure.DotFormgen;

namespace AMFormsCST.Core;
public class FormgenUtils
{
    private XmlDocument? _file = new();
    private string? _filePath;
    private string? _backupFilePath;
    private DotFormgen? _formFile;
    private static readonly string DirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant\\FormgenBackup";
    private bool hasImageFile = false;
    private bool renameImage = false;
    private string? _fileName;
    private string? _imageFileName;
    private string? _uuid;

    private void OpenFile(string filePath)
    {
        _filePath = filePath;
        _fileName = _filePath[(_filePath.LastIndexOf(@"\", StringComparison.Ordinal) + 1)..^8];
        _file?.Load(_filePath);
        ReadXML();

        bool isPDF = _formFile?.FormType == Format.Pdf;

        if (isPDF && File.Exists(_filePath[..^8] + ".pdf"))
        {
            _imageFileName = "PDF found";
            hasImageFile = true;
        }
        else if (!isPDF && File.Exists(_filePath[..^8] + ".jpg"))
        {
            _imageFileName = "JPG found";
            hasImageFile = true;
        }
        else
        {
            _imageFileName = "No associated image found";
            hasImageFile = false;
        }
    }
    private void ReadXML()
    {
        if (_file?.DocumentElement is null) return;

        _formFile = new DotFormgen(_file.DocumentElement);

        _uuid = _formFile.Settings.UUID;
    }
    private void CloseFile()
    {
        if (_filePath == string.Empty) return;
        _file = new XmlDocument();
        _formFile = null;

        _filePath = string.Empty;
        _uuid = string.Empty;
        _filePath = string.Empty;
        hasImageFile = false;
    }
    private void ClonePrompt(CodeLine[] selected )
    {
        if (_filePath == string.Empty) return;

        foreach (var selection in selected)
        {
            var idx = selected.ToList().IndexOf(selection);
            var item = _formFile?.GetPrompt(idx);
            var variable = item?.Settings?.Variable;

            var prompts = _formFile?.CodeLines.Where(x => x.Settings?.Type == CodeType.PROMPT).ToList();

            if (variable != "F0")
                while (prompts != null && prompts.Exists(x => x.Settings?.Variable == variable))
                    variable = CopyIncrementor(variable);

            if (item == null) continue;
            if (variable == null) continue;
            _formFile?.ClonePrompt(item, variable, _formFile.PromptCount());
        }
        //UpdatePrompts();
    }
    private CodeLine[] GetPrompts() => GetCodeLines(CodeType.PROMPT);

    private CodeLine[] GetCodeLines(CodeType type)
    {
        if (_filePath == string.Empty) return [];
        var codeLines = _formFile?.CodeLines.Where(x => x.Settings?.Type != type).ToArray();
        return codeLines ?? [];
    }
    private static string CopyIncrementor(string? input)
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

    private void LoadBackup(string filePath)
    {
        if (_filePath == string.Empty || filePath == string.Empty) return;

        _file?.Load(filePath);
        ReadXML();
        if (_filePath != null) _file?.Save(_filePath);
    }
    private void SaveFile()
    {
        if (_filePath == string.Empty) return;

        CreateBackup(_formFile?.Settings.UUID);

        var xml = _formFile?.GenerateXML();
        if (xml != null) _file?.LoadXml(xml);
        if (_filePath != null) _file?.Save(_filePath);
    }
    private void EditPrompts(CodeLine[] prompts)
    {
        if (_filePath == string.Empty) return; 
        if(prompts == null) return;

        foreach (var selection in prompts)
        {
            var idx = prompts.ToList().IndexOf(selection);
            var item = _formFile?.GetField(idx);

            if (item?.Settings != null) item.Settings.Bold = !item.Settings.Bold;
        }
    }
    private void CopyPromptsTo(string fromFilePath)
    {
        var newDoc = new XmlDocument();
        newDoc.Load(fromFilePath);
        var recipient = new DotFormgen(newDoc.DocumentElement ?? throw new InvalidOperationException());

        if (_formFile is null ) return;
        if (recipient.Pages.Count != _formFile.Pages.Count) return;

        CreateBackup(recipient.Settings.UUID);

        _backupFilePath = DirPath + "\\" + recipient.Settings.UUID + "\\" + DateTime.Now.ToString("mm-dd-yyyy.hh-mm-ss") + ".bak";
        _file?.Save(_backupFilePath);

        if (_formFile?.Pages != null) recipient.Pages = _formFile?.Pages ?? throw new InvalidOperationException();
        if (_formFile?.CodeLines != null) recipient.CodeLines = _formFile?.CodeLines ?? throw new InvalidOperationException();

        newDoc.LoadXml(recipient.GenerateXML());
        newDoc.Save(fromFilePath);
    }

    private void Rename(string newName)
    {
        if (_filePath == string.Empty) return;
        if (_formFile is null) return;
        if (_file is null || _file!.BaseURI is not null) return;

        var oldName = _file!.BaseURI![(_file!.BaseURI!.LastIndexOf('/') + 1)..^8];
        var fileDir = _file!.BaseURI!.Replace("file:///", string.Empty);
        fileDir = fileDir[..(fileDir.LastIndexOf('/') + 1)];

        CreateBackup(_formFile.Settings.UUID);

        _formFile!.Title = newName;
        var xml = _formFile.GenerateXML();
        if (xml != null) _file!.LoadXml(xml);
        if (_filePath != null) _file!.Save(fileDir + oldName + ".formgen");

        File.Move(fileDir + oldName + ".formgen", fileDir + newName + ".formgen");

        if (hasImageFile && renameImage)
        {
            if (_formFile!.FormType == Format.Pdf)
            {
                File.Move(fileDir + oldName + ".pdf", fileDir + newName + ".pdf");
            }
            else
            {
                File.Move(fileDir + oldName + ".jpg", fileDir + newName + ".jpg");
            }
        }

        _file?.Load(fileDir + newName + ".formgen");
        ReadXML();
        //UpdatePrompts();
    }

    private void CreateBackup(string uuid)
    {
        if(_formFile is null) return;

        Directory.CreateDirectory(DirPath + "\\" + uuid);
        _backupFilePath = DirPath + "\\" + uuid + "\\" + DateTime.Now.ToString("mm-dd-yyyy.hh-mm-ss") + ".bak";
        _file?.Save(_backupFilePath);
    }
}
