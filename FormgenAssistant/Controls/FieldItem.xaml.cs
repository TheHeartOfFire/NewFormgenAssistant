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

namespace FormgenAssistant.Controls
{
    /// <summary>
    /// Interaction logic for PromptItem.xaml
    /// </summary>
    public partial class FieldItem : UserControl
    {

        public FieldItem(string Descriptor, string VariableName, string Message)
        {
            InitializeComponent();
            lblDescriptor.Content = Descriptor;
            lblName.Content = VariableName.Equals("F0")?"None":VariableName;
            lblMessage.Content = Message;
        }
        /// <summary>
        /// For designer use only, do not call this for live code.
        /// </summary>
        public FieldItem()
        {
            InitializeComponent();
            lblDescriptor.Content = "ABC";
            lblName.Content = "txtPrompt1";
            lblMessage.Content = "Use the Parameterized constructor for live use.";
        }
    }
}
