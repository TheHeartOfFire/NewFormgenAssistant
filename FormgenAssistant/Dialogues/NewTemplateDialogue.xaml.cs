using FormgenAssistant.Controls;
using FormgenAssistant.SavedItems.Templates;
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

namespace FormgenAssistant.Dialogues
{
    /// <summary>
    /// Interaction logic for NewTemplateDialogue.xaml
    /// </summary>
    public partial class NewTemplateDialogue : Window
    {
        public Template? NewTemplate { get; private set; }
        public NewTemplateDialogue()
        {
            InitializeComponent();
            txtName.Focus();
        }
        public NewTemplateDialogue(Template template)
        {
            InitializeComponent();
            NewTemplate = template;

            txtName.Text = template.Name;
            txtTemplate.Text = template.Text;
            txtVarCount.Text = template.Variables.ToString();
            stkDefaults.Children.Clear();
            foreach (var item in template.VariableDefaults)
            {
                var control = GenerateTextBox();
                control.Text = item;
                stkDefaults.Children.Add(control);
            }
            txtName.Focus();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var defaults = new List<string>();

            foreach (var item in stkDefaults.Children)
            {
                if (item is NewTextBox control)
                {
                    defaults.Add(control.Text);
                }
            }

            NewTemplate = new(txtName.Text, txtTemplate.Text, txtVarCount.Text != string.Empty ? ushort.Parse(txtVarCount.Text) : (ushort)0, defaults);
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void txtVarCount_OnTextChanged(object sender, EventArgs e)
        {
            var success = int.TryParse(txtVarCount.Text, out int count);

            if (!success) return;

            if (stkDefaults.Children.Count == count) return;

            bool NotEnough = stkDefaults.Children.Count < count;


            while(stkDefaults.Children.Count != count)
            {
                if (NotEnough)
                {
                    stkDefaults.Children.Add(GenerateTextBox());
                    continue;
                }

                stkDefaults.Children.RemoveAt(stkDefaults.Children.Count - 1);
            }

        }

        private static NewTextBox GenerateTextBox()
        {
            var box = new NewTextBox
            {
                MaxWidth = 120,
                MinWidth = 50
            };
            GenerateContextMenu(box);
            return box;
        }
        
        private static void GenerateContextMenu(NewTextBox box)
        {
            var item = new MenuItem
            {
                Header = "Reserved Variable Names"
            };

            item.Items.Add(AddContextMenuItem(box, "Server ID#", "Notes:ServerID"));
            item.Items.Add(AddContextMenuItem(box, "Company#(s)", "Notes:Companies"));
            item.Items.Add(AddContextMenuItem(box, "Dealership Name", "Notes:Dealership"));
            item.Items.Add(AddContextMenuItem(box, "Contact Name", "Notes:ContactName"));
            item.Items.Add(AddContextMenuItem(box, "E-Mail Address", "Notes:EmailAddress"));
            item.Items.Add(AddContextMenuItem(box, "Phone#", "Notes:Phone"));
            item.Items.Add(AddContextMenuItem(box, "Notes", "Notes:Notes"));

            box.ContextMenu ??= new();
            box.ContextMenu.Items.Add(item);
        }

        private static MenuItem AddContextMenuItem(NewTextBox box, string header, string text)
        {
            var item = new MenuItem
            {
                Header = header
            };
            item.Click += (s, e) =>
            {
                box.Text = text;
            };
            return item;
        }
    }
}
