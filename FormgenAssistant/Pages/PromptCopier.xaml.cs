using FormgenAssistant.DataTypes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for PromptCopier.xaml
    /// </summary>
    public partial class PromptCopier : UserControl
    {
        private XmlDocument File = new XmlDocument();
        private string FilePath;
        private string backupFilePath;
        private DotFormgen FormFile;
        private static readonly string DirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant\\FormgenBackup";
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

            FilePath = dia.FileName;
            txtFilePath.Text = FilePath[(FilePath.LastIndexOf(@"\") + 1)..];
            File.Load(FilePath);
            ReadXML();
            UpdatePrompts();
        }

        private void ReadXML()
        {
            if (File.DocumentElement is null) return;

            FormFile = new DotFormgen(File.DocumentElement);

            txtUUID.Text = FormFile.Settings.UUID;
        }

        private void btnUUIDGen_Click(object sender, RoutedEventArgs e)
        {
            if (File.DocumentElement is null) return;
            if (MessageBox.Show(
                "Are you sure you want to change the UUID for this form. Formgen will no longer recognize this as the same form.",
                "Update UUID",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No) return;

            var UUID = Guid.NewGuid().ToString();
            txtUUID.Text = UUID;
            File.DocumentElement.Attributes[1].Value = UUID;
            FormFile.Settings.UUID = UUID;
        }

        private void ctxtDeletePrompt(object sender, RoutedEventArgs e)
        {
            if (File.DocumentElement is null) return;
            if (MessageBox.Show(
                "Are you sure you want to delete " + lboxPrompts.SelectedItems.Count + "Item(s)?", 
                "Delete", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning) == MessageBoxResult.No) return;

            foreach (var selection in lboxPrompts.SelectedItems)
            {
                int idx = lboxPrompts.Items.IndexOf(selection);


                FormFile.CodeLines.Remove(FormFile.GetPrompt(idx));
            }


            UpdatePrompts();
        }
        private void ctxtClonePrompt(object sender, RoutedEventArgs e)
        {
            if(txtFilePath.Text == string.Empty) return;
            if (lboxPrompts.SelectedIndex<0) return;

            foreach(var selection in lboxPrompts.SelectedItems)
            {
                int idx = lboxPrompts.Items.IndexOf(selection);
                var item = FormFile.GetPrompt(idx);
                var variable = item.Settings.Variable;

                var prompts = FormFile.CodeLines.Where(x => x.Settings.Type == CodeLineSettings.CodeType.PROMPT).ToList();

                if (variable != "F0")
                    while (prompts.Exists(x => x.Settings.Variable == variable))
                        variable = CopyIncrimentor(variable);

                FormFile.ClonePrompt(item, variable, FormFile.PromptCount());
            }
            UpdatePrompts();
        }

        private void UpdatePrompts()
        {
            lboxPrompts.Items.Clear();
            foreach (var line in FormFile.CodeLines)
            {   
                if(line.Settings.Type == CodeLineSettings.CodeType.PROMPT)
                lboxPrompts.Items.Add( new Controls.PromptItem(PromptDataSettings.PromptDescriptor(line.PromptData.Settings.Type), line.Settings.Variable, line.PromptData.Message));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text == string.Empty) return;
            File = new XmlDocument();
            FormFile = null;

            txtFilePath.Text = string.Empty;
            txtUUID.Text = string.Empty;
            FilePath = string.Empty;
            lboxPrompts.Items.Clear();
        }

        private static string CopyIncrimentor(string input)
        {
            var index = input.Length-1;
            int number;
            while (int.TryParse(input[index].ToString(), out _))
            {
                index--;
            }

            _ = int.TryParse(input.Substring(index+1), out number);

            number++;
            var output = input.Substring(0, index+1) + number.ToString();
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
                InitialDirectory = DirPath + "\\" + FormFile.Settings.UUID
            };
            if (dia.ShowDialog() == false) return;

            File.Load(dia.FileName);
            ReadXML();
            File.Save(FilePath);
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
            Directory.CreateDirectory(DirPath + "\\" + FormFile.Settings.UUID);
            backupFilePath = DirPath + "\\" + FormFile.Settings.UUID + "\\" + DateTime.Now.ToString("mm-dd-yyyy.hh-mm-ss") + ".bak";
            File.Save(backupFilePath);

            var xml = FormFile.GenerateXML();
            File.LoadXml(xml);
            File.Save(FilePath);
        }

        private void tglPromptField_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (txtFilePath.Text == string.Empty) return;
            var item = lboxPrompts.ContextMenu.Items[0] as MenuItem;
            item.IsEnabled = !item.IsEnabled;
            item = lboxPrompts.ContextMenu.Items[1] as MenuItem;
            item.IsEnabled = !item.IsEnabled;
            item = lboxPrompts.ContextMenu.Items[2] as MenuItem;
            item.IsEnabled = !item.IsEnabled;

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
            foreach (var line in FormFile.Pages)
                foreach (var field in line.Fields)
                    lboxPrompts.Items.Add(new Controls.FieldItem(field.Settings.Type.ToString(), field.Settings.Bold.ToString(), field.Expression));
        }

        private void ctxtEdit(object sender, RoutedEventArgs e)
        {
            if (txtFilePath.Text == string.Empty) return;
            if (lboxPrompts.SelectedIndex < 0) return;

            foreach (var selection in lboxPrompts.SelectedItems)
            {
                int idx = lboxPrompts.Items.IndexOf(selection);
                var item = FormFile.GetField(idx);

                item.Settings.Bold = !item.Settings.Bold;
            }
            LoadFields();
        }

        private void btnUndoChanges_Click(object sender, RoutedEventArgs e)
        {
            File.Load(FilePath);
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
            var Recipient = new DotFormgen(newDoc.DocumentElement);

            var RecipientNicerName = dia.FileName[(dia.FileName.LastIndexOf(@"\") + 1)..];

            if (Recipient.Pages.Count != FormFile.Pages.Count)
            {
                MessageBox.Show(
                $"{RecipientNicerName} has {Recipient.Pages.Count} pages, but {txtFilePath.Text} has {FormFile.Pages.Count}. You can only copy to a form with the same number of pages.",
                "Copy",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show(
                $"You're about to copy {FormFile.FieldCount()} fields, {FormFile.InitCount()} init(default) lines, {FormFile.PromptCount()} prompts and {FormFile.PostCount()} post prompt lines from {txtFilePath.Text} to {RecipientNicerName}. \n" +
                $"This will overwrite {Recipient.FieldCount()} fields, {Recipient.InitCount()} init(default) lines, {Recipient.PromptCount()} prompts and {Recipient.PostCount()} post prompt lines.\n" +
                $"A backup of {RecipientNicerName} will be created, do you wish to proceed?",
                "Copy",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No) return;


            Directory.CreateDirectory(DirPath + "\\" + Recipient.Settings.UUID);
            backupFilePath = DirPath + "\\" + Recipient.Settings.UUID + "\\" + DateTime.Now.ToString("mm-dd-yyyy.hh-mm-ss") + ".bak";
            File.Save(backupFilePath);

            Recipient.Pages = FormFile.Pages;
            Recipient.CodeLines = FormFile.CodeLines;
            
            newDoc.LoadXml(Recipient.GenerateXML());
            newDoc.Save(dia.FileName);
        }
    }
}
