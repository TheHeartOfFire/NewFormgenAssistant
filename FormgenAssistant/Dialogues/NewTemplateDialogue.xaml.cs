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
            
            foreach (var option in Enum.GetValues<Template.TemplateType>())
            {
                cboType.Items.Add(option);
            }
            cboType.SelectedValue = SavedItems.Templates.Template.TemplateType.Other;
            txtName.Focus();
        }
        public NewTemplateDialogue(Template template)
        {
            InitializeComponent();
            foreach (var option in Enum.GetValues<Template.TemplateType>())
            {
                cboType.Items.Add(option);
            }
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
            cboType.SelectedValue = template.Type;
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
            NewTemplate.Type = (Template.TemplateType)cboType.SelectedValue;
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
                MinWidth = 50,
                Margin = new Thickness(5,0,5, 0)
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
            item.Items.Add(AddContextMenuItem(box, "Case #", "Notes:CaseNumber"));
            item.Items.Add(AddContextMenuItem(box, "Forms", "Notes:Forms"));
            item.Items.Add(AddContextMenuItem(box, "First Name", "Notes:FirstName"));
            item.Items.Add(AddContextMenuItem(box, "A/M Mail Address", "Notes:AMMailingAddress"));
            item.Items.Add(AddContextMenuItem(box, "A/M Mail Name", "Notes:AMMailName"));
            item.Items.Add(AddContextMenuItem(box, "A/M Mail Street", "Notes:AMMailStreet"));
            item.Items.Add(AddContextMenuItem(box, "A/M Mail City", "Notes:AMMailCity"));
            item.Items.Add(AddContextMenuItem(box, "A/M Mail State", "Notes:AMMailState"));
            item.Items.Add(AddContextMenuItem(box, "A/M Mail Zip", "Notes:AMMailZip"));
            item.Items.Add(AddContextMenuItem(box, "Form Name Generator", "Notes:FormNameGenerator"));
            item.Items.Add(AddContextMenuItem(box, "Deal #", "Notes:DealNumber"));

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
