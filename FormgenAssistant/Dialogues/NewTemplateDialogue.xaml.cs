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
            txtName.Focus();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            NewTemplate = new(txtName.Text, txtTemplate.Text, txtVarCount.Text != string.Empty ? ushort.Parse(txtVarCount.Text) : (ushort)0);
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
