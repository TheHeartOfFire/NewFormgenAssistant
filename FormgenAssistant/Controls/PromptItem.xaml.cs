using System.Windows.Controls;

namespace FormgenAssistant.Controls
{
    /// <summary>
    /// Interaction logic for PromptItem.xaml
    /// </summary>
    public partial class PromptItem : UserControl
    {

        public PromptItem(string descriptor, string? VariableName, string? Message)
        {
            InitializeComponent();
            LblDescriptor.Content = descriptor;
            LblName.Content = VariableName is "F0"?"None":VariableName;
            LblMessage.Content = Message;
        }
        /// <summary>
        /// For designer use only, do not call this for live code.
        /// </summary>
        public PromptItem()
        {
            InitializeComponent();
            LblDescriptor.Content = "ABC";
            LblName.Content = "txtPrompt1";
            LblMessage.Content = "Use the Parameterized constructor for live use.";
        }
    }
}
