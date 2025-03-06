using FormgenAssistant.DataTypes;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using static FormgenAssistant.DataTypes.DotFormgen;

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for PromptCopier.xaml
    /// </summary>
    /// 
    /*
     * Change .formgen File Name
     * change Form name in .formgen file
     * check .formgen file if form is PDF or Impact
     * if PDF, look for identically named .pdf file and rename it
     * if impact, look for identically named .jpg file and rename it
     * include toggle for including pdf/jpg file renaming
     * include display for if associated image was found
    */

    public partial class PromptCopier : UserControl
    {
        private XmlDocument? _file = new ();
        private string? _filePath;
        private string? _backupFilePath;
        private DotFormgen? _formFile;
        private static readonly string DirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant\\FormgenBackup";
        private bool hasImageFile = false;
        private bool renameImage = false;
        public PromptCopier()
        {
            InitializeComponent();
            lboxPrompts.Items.Clear();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dia = new()
            {
                Filter = "Formgen files (*.formgen)|*.formgen"
            };
            if (dia.ShowDialog() == false) return;

            _filePath = dia.FileName;
            txtFilePath.Text = _filePath[(_filePath.LastIndexOf(@"\", StringComparison.Ordinal) + 1)..];
            _file?.Load(_filePath);
            ReadXML();
            UpdatePrompts();

            bool isPDF = _formFile?.FormType == Format.Pdf;

            if(isPDF && File.Exists(_filePath[..^7] + ".pdf"))
            {
                lblImageFile.Content = "PDF found";
                hasImageFile = true;
            }
            else if (!isPDF && File.Exists(_filePath[..^7] + ".jpg"))
            {
                lblImageFile.Content = "JPG found";
                hasImageFile = true;
            }
            else
            {
                lblImageFile.Content = "No associated image found";
                hasImageFile = false;
            }
            lblRenameImage.Visibility = hasImageFile ? Visibility.Visible : Visibility.Hidden;
            tglRenameImage.Visibility = hasImageFile ? Visibility.Visible : Visibility.Hidden;
        }

        private void ReadXML()
        {
            if (_file?.DocumentElement is null) return;

            _formFile = new DotFormgen(_file.DocumentElement);

            txtUUID.Text = _formFile.Settings.UUID;
        }

        private void btnUUIDGen_Click(object sender, RoutedEventArgs e)
        {
            if (_file?.DocumentElement is null) return;
            if (MessageBox.Show(
                "Are you sure you want to change the UUID for this form. Formgen will no longer recognize this as the same form.",
                "Update UUID",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No) return;

            var uuid = Guid.NewGuid().ToString();
            txtUUID.Text = uuid;
            _file.DocumentElement.Attributes[1].Value = uuid;
            if (_formFile != null) _formFile.Settings.UUID = uuid;
        }

        // ReSharper disable once InconsistentNaming
        private void ctxtDeletePrompt(object sender, RoutedEventArgs e)
        {
            if (_file?.DocumentElement is null) return;
            if (MessageBox.Show(
                "Are you sure you want to delete " + lboxPrompts.SelectedItems.Count + "Item(s)?", 
                "Delete", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning) == MessageBoxResult.No) return;

            foreach (var selection in lboxPrompts.SelectedItems)
            {
                var idx = lboxPrompts.Items.IndexOf(selection);


                _formFile?.CodeLines.Remove(_formFile.GetPrompt(idx));
            }


            UpdatePrompts();
        }
        // ReSharper disable once InconsistentNaming
        private void ctxtClonePrompt(object sender, RoutedEventArgs e)
        {
            if(txtFilePath.Text == string.Empty) return;
            if (lboxPrompts.SelectedIndex<0) return;

            foreach(var selection in lboxPrompts.SelectedItems)
            {
                var idx = lboxPrompts.Items.IndexOf(selection);
                var item = _formFile?.GetPrompt(idx);
                var variable = item?.Settings?.Variable;

                var prompts = _formFile?.CodeLines.Where(x => x.Settings?.Type == CodeLineSettings.CodeType.PROMPT).ToList();

                if (variable != "F0")
                    while (prompts != null && prompts.Exists(x => x.Settings?.Variable == variable))
                        variable = CopyIncrementer(variable);

                if (item == null) continue;
                if (variable == null) continue;
                _formFile?.ClonePrompt(item, variable, _formFile.PromptCount());
            }
            UpdatePrompts();
        }

        private void UpdatePrompts()
        {
            lboxPrompts.Items.Clear();
            foreach (var line in _formFile?.CodeLines.Where(line => line.Settings is {Type: CodeLineSettings.CodeType.PROMPT})!)
                lboxPrompts.Items.Add( 
                    new Controls.PromptItem(
                        PromptDataSettings.PromptDescriptor(line.PromptData!.Settings!.Type), 
                        line.Settings?.Variable, 
                        line.PromptData.Message));
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text == string.Empty) return;
            _file = new XmlDocument();
            _formFile = null;

            txtFilePath.Text = string.Empty;
            txtUUID.Text = string.Empty;
            _filePath = string.Empty;
            lboxPrompts.Items.Clear();
            lblImageFile.Content = "Load File";
            hasImageFile = false;
            lblRenameImage.Visibility = Visibility.Hidden;
            tglRenameImage.Visibility = Visibility.Hidden;
        }

        private static string CopyIncrementer(string? input)
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

        private void lboxPrompts_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            if(lboxPrompts.SelectedItems.Count > 0)
                e.Handled = true;
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text == string.Empty) return;
            OpenFileDialog dia = new()
            {
                Filter = "Backup files (*.bak)|*.bak",
                InitialDirectory = DirPath + "\\" + _formFile?.Settings.UUID
            };
            if (dia.ShowDialog() == false) return;

            _file?.Load(dia.FileName);
            ReadXML();
            if (_filePath != null) _file?.Save(_filePath);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text == string.Empty) return;
            if (MessageBox.Show(
               $"You are about to save your changes to {txtFilePath.Text}. A backup will be created of your original file in case you change your mind. Do you wish to proceed?",
               "Save",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning) == MessageBoxResult.No) return;

            //Create backup file
            Directory.CreateDirectory(DirPath + "\\" + _formFile?.Settings.UUID);
            _backupFilePath = DirPath + "\\" + _formFile?.Settings.UUID + "\\" + DateTime.Now.ToString("mm-dd-yyyy.hh-mm-ss") + ".bak";
            _file?.Save(_backupFilePath);

            var xml = _formFile?.GenerateXML();
            if (xml != null) _file?.LoadXml(xml);
            if (_filePath != null) _file?.Save(_filePath);
        }

        private void tglPromptField_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (txtFilePath.Text is "") return;
            if (lboxPrompts.ContextMenu != null)
            {
                if (lboxPrompts.ContextMenu.Items[0] is MenuItem item) item.IsEnabled = !item.IsEnabled;
                item = (lboxPrompts.ContextMenu.Items[1] as MenuItem)!;
                item.IsEnabled = !item.IsEnabled;
                item = (lboxPrompts.ContextMenu.Items[2] as MenuItem)!;
                item.IsEnabled = !item.IsEnabled;
            }

            if (tglPromptField.IsOn == false)
            {
                UpdatePrompts();
                return;
            }

            LoadFields();
        }

        private void LoadFields()
        {
            lboxPrompts.Items.Clear();
            foreach (var field in (_formFile?.Pages ?? throw new InvalidOperationException()).SelectMany(line => line.Fields))
                lboxPrompts.Items.Add(new Controls.FieldItem(field.Settings?.Type.ToString() ?? string.Empty, field.Settings?.Bold.ToString() ?? string.Empty, field.Expression ?? string.Empty));
        }

        private void ctxtEdit(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text == string.Empty) return;
            if (lboxPrompts.SelectedIndex < 0) return;

            foreach (var selection in lboxPrompts.SelectedItems)
            {
                var idx = lboxPrompts.Items.IndexOf(selection);
                var item = _formFile?.GetField(idx);

                if (item?.Settings != null) item.Settings.Bold = !item.Settings.Bold;
            }
            LoadFields();
        }

        private void btnUndoChanges_Click(object sender, RoutedEventArgs e)
        {
            if (_filePath != null) _file?.Load(_filePath);
            ReadXML();

            if (tglPromptField.IsOn == false)
            {
                UpdatePrompts();
                return;
            }

            LoadFields();
        }

        private void btnUndo_Copy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dia = new()
            {
                Filter = "Formgen files (*.formgen)|*.formgen"
            };
            if (dia.ShowDialog() == false) return;

            var newDoc = new XmlDocument();
            newDoc.Load(dia.FileName);
            var recipient = new DotFormgen(newDoc.DocumentElement ?? throw new InvalidOperationException());

            var recipientNicerName = dia.FileName[(dia.FileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1)..];

            if (_formFile != null && recipient.Pages.Count != _formFile.Pages.Count)
            {
                MessageBox.Show(
                $"{recipientNicerName} has {recipient.Pages.Count} pages, but {txtFilePath.Text} has {_formFile.Pages.Count}. You can only copy to a form with the same number of pages.",
                "Copy",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
                return;
            }

            if (_formFile != null && MessageBox.Show(
                    $"You're about to copy {_formFile.FieldCount()} fields, {_formFile.InitCount()} init(default) lines, {_formFile.PromptCount()} prompts and {_formFile.PostCount()} post prompt lines from {txtFilePath.Text} to {recipientNicerName}. \n" +
                    $"This will overwrite {recipient.FieldCount()} fields, {recipient.InitCount()} init(default) lines, {recipient.PromptCount()} prompts and {recipient.PostCount()} post prompt lines.\n" +
                    $"A backup of {recipientNicerName} will be created, do you wish to proceed?",
                    "Copy",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.No) return;


            Directory.CreateDirectory(DirPath + "\\" + recipient.Settings.UUID);
            _backupFilePath = DirPath + "\\" + recipient.Settings.UUID + "\\" + DateTime.Now.ToString("mm-dd-yyyy.hh-mm-ss") + ".bak";
            _file?.Save(_backupFilePath);

            if (_formFile?.Pages != null) recipient.Pages = _formFile?.Pages ?? throw new InvalidOperationException();
            if (_formFile?.CodeLines != null) recipient.CodeLines = _formFile?.CodeLines ?? throw new InvalidOperationException();

            newDoc.LoadXml(recipient.GenerateXML());
            newDoc.Save(dia.FileName);
        }

        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text == string.Empty) return;
            if (_formFile is null) return;
            if (_file is null && _file!.Name is not null) return;
            if (MessageBox.Show(
               $"You are about to rename {_file.Name ?? "[file not found]"} to {txtFilePath.Text}. A backup will be created of your original file in case you change your mind. Do you wish to proceed?",
               "Save",
               MessageBoxButton.YesNo,
               MessageBoxImage.Warning) == MessageBoxResult.No) return;

            //Create backup file
            Directory.CreateDirectory(DirPath + "\\" + _formFile?.Settings.UUID);
            _backupFilePath = DirPath + "\\" + _formFile?.Settings.UUID + "\\" + DateTime.Now.ToString("mm-dd-yyyy.hh-mm-ss") + ".bak";
            _file?.Save(_backupFilePath);

            _formFile!.Title = txtFilePath.Text;
            File.Move(_file!.Name!, txtFilePath.Text);
            if (hasImageFile)
            {
                if (_formFile.FormType == Format.Pdf)
                {
                    File.Move(_file!.Name![..^7] + ".pdf", txtFilePath.Text[..^7] + ".pdf");
                }
                else
                {
                    File.Move(_file!.Name![..^7] + ".jpg", txtFilePath.Text[..^7] + ".jpg");
                }
            }
            var xml = _formFile?.GenerateXML();
            if (xml != null) _file?.LoadXml(xml);
            if (_filePath != null) _file?.Save(_filePath);
        }

        private void tglRenameImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            renameImage = tglRenameImage.IsOn.HasValue ? tglRenameImage.IsOn.Value : false;
        }
    }
}
