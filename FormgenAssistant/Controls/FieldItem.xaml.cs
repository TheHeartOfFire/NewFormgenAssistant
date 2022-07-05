using System.Windows.Controls;

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
