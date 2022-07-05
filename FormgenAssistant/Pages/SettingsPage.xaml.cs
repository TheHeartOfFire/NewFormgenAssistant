using FormgenAssistant.SavedItems;
using System.Windows.Controls;
using System.Windows.Input;
using FormgenAssistant.Pages;

namespace FormgenAssistant
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        private static readonly Settings Settings = Settings.Instance;
        public SettingsPage()
        {
            InitializeComponent();
            txtAddress.Text = Settings.MailingAddress;

        }
        
        private void tglFormCodeCAPS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tglFormCodeCAPS.IsOn is null) return;

            Settings.NotesCopyAll = (bool)tglFormCodeCAPS.IsOn;
            Settings.Save();
        }

        private void btnRevertAddress_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtAddress.Text = Settings.MailingAddress;
        }

        private void btnUpdateAddress_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Settings.MailingAddress = txtAddress.Text;
            Settings.Save();
        }
    }
}
