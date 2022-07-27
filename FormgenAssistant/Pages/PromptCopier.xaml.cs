using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using FormgenAssistantLibrary.DataTypes.FormgenFileStructure;
using FormgenAssistantLibrary.Interfaces.DI;

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for PromptCopier.xaml
    /// </summary>
    public partial class PromptCopier : Page
    {
        private readonly IPromptCopier _copier;

        public PromptCopier(IPromptCopier copier)
        {
            _copier = copier;
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
            
            txtFilePath.Text = dia.FileName[(dia.FileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1)..];
            _copier.ReadXML(dia.FileName);
            UpdatePrompts();
        }

        

        private void btnUUIDGen_Click(object sender, RoutedEventArgs e)
        {
            if (!_copier.IsLoaded()) return;
            if (MessageBox.Show(
                "Are you sure you want to change the UUID for this form. Formgen will no longer recognize this as the same form.",
                "Update UUID",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No) return;
            
            txtUUID.Text = _copier.RegenerateUUID();
        }

        // ReSharper disable once InconsistentNaming
        private void ctxtDeletePrompt(object sender, RoutedEventArgs e)
        {
            if (!_copier.IsLoaded()) return;

            if (MessageBox.Show(
                "Are you sure you want to delete " + lboxPrompts.SelectedItems.Count + "Item(s)?", 
                "Delete", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning) == MessageBoxResult.No) return;

            foreach (var selection in lboxPrompts.SelectedItems)
                _copier.RemovePrompt(lboxPrompts.Items.IndexOf(selection));
            


            UpdatePrompts();
        }
        // ReSharper disable once InconsistentNaming
        private void ctxtClonePrompt(object sender, RoutedEventArgs e)
        {
            if(!_copier.IsLoaded()) return;
            if (lboxPrompts.SelectedIndex<0) return;

            foreach(var selection in lboxPrompts.SelectedItems)
            {
                var idx = lboxPrompts.Items.IndexOf(selection);
                _copier.ClonePrompt(idx);
            }
            UpdatePrompts();
        }

        private void UpdatePrompts()
        {
            lboxPrompts.Items.Clear();
            foreach (var line in _copier.FormFile?.CodeLines.Where(line => line.Settings is {Type: CodeLineSettings.CodeType.PROMPT})!)
                lboxPrompts.Items.Add( 
                    new Controls.PromptItem(
                        PromptDataSettings.PromptDescriptor(line.PromptData!.Settings!.Type), 
                        line.Settings?.Variable, 
                        line.PromptData.Message));
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text == string.Empty) return;

            txtFilePath.Text = string.Empty;
            txtUUID.Text = string.Empty;
            _copier.CloseFile();
            lboxPrompts.Items.Clear();
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
                InitialDirectory = _copier.BackupDirectory
            };
            if (dia.ShowDialog() == false) return;
            _copier.ReadXML(dia.FileName);
            
            //if (_filePath != null) _file?.Save(_filePath);
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
            foreach (var field in (_copier.FormFile?.Pages ?? throw new InvalidOperationException()).SelectMany(line => line.Fields))
                lboxPrompts.Items.Add(new Controls.FieldItem(field.Settings?.Type.ToString() ?? string.Empty, field.Settings?.Bold.ToString() ?? string.Empty, field.Expression ?? string.Empty));
        }

        private void ctxtEdit(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text == string.Empty) return;
            if (lboxPrompts.SelectedIndex < 0) return;

            foreach (var selection in lboxPrompts.SelectedItems)
            {
                var idx = lboxPrompts.Items.IndexOf(selection);
                _copier.ToggleBold(idx);
            }
            LoadFields();
        }

        private void btnUndoChanges_Click(object sender, RoutedEventArgs e)
        {
            _copier.ReadXML(txtFilePath.Text);

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

            if (_copier.FormFile != null && recipient.Pages.Count != _copier.FormFile.Pages.Count)
            {
                MessageBox.Show(
                $"{recipientNicerName} has {recipient.Pages.Count} pages, but {txtFilePath.Text} has {_copier.FormFile.Pages.Count}. You can only copy to a form with the same number of pages.",
                "Copy",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
                return;
            }

            if (_copier.FormFile != null && MessageBox.Show(
                    $"You're about to copy {_copier.FormFile.FieldCount()} fields, {_copier.FormFile.InitCount()} init(default) lines, {_copier.FormFile.PromptCount()} prompts and {_copier.FormFile.PostCount()} post prompt lines from {txtFilePath.Text} to {recipientNicerName}. \n" +
                    $"This will overwrite {recipient.FieldCount()} fields, {recipient.InitCount()} init(default) lines, {recipient.PromptCount()} prompts and {recipient.PostCount()} post prompt lines.\n" +
                    $"A backup of {recipientNicerName} will be created, do you wish to proceed?",
                    "Copy",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.No) return;


            _copier.CopyTo(dia.FileName);
        }
    }
}
