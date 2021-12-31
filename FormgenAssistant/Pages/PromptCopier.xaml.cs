using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        private List<PromptItem> Prompts = new List<PromptItem>();
        private List<XmlNode> Nodes = new List<XmlNode>();
        public PromptCopier()
        {
            InitializeComponent();
            lboxPrompts.Items.Clear();
            //lboxPrompts.Items.Add(new Controls.PromptItem(PromptDescriptor(PromptType.RadioButtons), "rdoPrompt1", "I am a radio prompt for testing purposes!"));
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dia = new OpenFileDialog();
            dia.Filter = "Formgen files (*.formgen)|*.formgen";
            if (dia.ShowDialog() == false) return;

            txtFilePath.Text = dia.FileName;
            File.Load(txtFilePath.Text);
            ReadXML();


        }

        private void ReadXML()
        {
            //UUID is inside of <formDef> and is named publishedUUID and is a string
            //Prompts are each a different <codeLines> node with a type of "PROMPT" with a name of destVariable
            //inside of prompts are <promptData>
            //inside of prompt Data are <promptMessage>
            if (File.DocumentElement is null) return;
            txtUUID.Text =  File.DocumentElement.Attributes[1].Value;

            foreach(XmlNode node in File.DocumentElement.ChildNodes)
            {
                
                if (node.Name == "codeLines" && node.Attributes is not null && node.Attributes[1].Value == "PROMPT")
                {
                    Prompts.Add(new PromptItem(node.Attributes[0].Value, 
                        node.Attributes[2].Value, 
                        node.FirstChild is not null && node.FirstChild.FirstChild is not null ? node.FirstChild.FirstChild.InnerText : "None",
                        node.FirstChild is not null ? node.FirstChild.Attributes[0].Value : "None"));
                    Nodes.Add(node);
                }
            }
            UpdatePrompts();
        }

        private void btnUUIDGen_Click(object sender, RoutedEventArgs e)
        {
            if (File.DocumentElement is null) return;

            var UUID = Guid.NewGuid().ToString();
            txtUUID.Text = UUID;
            File.DocumentElement.Attributes[1].Value = UUID;
            File.Save(txtFilePath.Text);
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


                foreach (XmlNode node in File.DocumentElement.ChildNodes)
                {

                    if (node.Name == "codeLines" && node.Attributes is not null && node.Attributes[1].Value == "PROMPT")
                    {
                        if (node.Attributes[2].Value == Prompts[idx].variable)
                        {
                            File.DocumentElement.RemoveChild(node);
                        }
                    }
                }
                Nodes.RemoveAt(idx);
                Prompts.RemoveAt(idx);
            }


            UpdatePrompts();
            File.Save(txtFilePath.Text);
        }
        private void ctxtClonePrompt(object sender, RoutedEventArgs e)
        {
            if(txtFilePath.Text == string.Empty) return;
            if (lboxPrompts.SelectedIndex<0) return;

            foreach(var selection in lboxPrompts.SelectedItems)
            {
                int idx = lboxPrompts.Items.IndexOf(selection);
                var item = Prompts[idx];
                var oldNode = Nodes[idx];
                var variable = item.variable;
                while (Prompts.Exists(x => x.variable == variable))
                    variable = CopyIncrimentor(variable);

                var newItem = new PromptItem(Prompts.Count.ToString(), variable, item.message, oldNode.FirstChild.Attributes[0].Value);

                var newNode = Nodes[idx].Clone();
                newNode.Attributes[0].Value = newItem.position;
                newNode.Attributes[2].Value = newItem.variable;
                if (newItem.message != "None")
                    newNode.FirstChild.FirstChild.InnerText = newItem.message;

                Nodes.Add(newNode);
                File.DocumentElement.AppendChild(newNode);
                Prompts.Add(newItem);
            }
            UpdatePrompts();
            File.Save(txtFilePath.Text);
        }

        private void UpdatePrompts()
        {
            lboxPrompts.Items.Clear();
            foreach (var prompt in Prompts)
            {
                lboxPrompts.Items.Add( new Controls.PromptItem(PromptDescriptor(prompt.type),prompt.variable, prompt.message));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            File = new XmlDocument();
            Nodes.Clear();
            Prompts.Clear();

            txtFilePath.Text = string.Empty;
            txtUUID.Text = string.Empty;
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
    }

    public class PromptItem
    {
        public string position { get; set; }
        public string variable { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public PromptItem(string Position, string Variable, string Message, string Type)
        {
            position = Position;
            variable = Variable;
            message = Message;
            type = Type;
        }
    }
}
