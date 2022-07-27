using System.Windows.Controls;
using System.Windows.Input;
using FormgenAssistant.Pages;
using FormgenAssistantLibrary.DataTypes;
using FormgenAssistantLibrary.Interfaces.DI;
using FormgenAssistantLibrary.SavedItems;
using Microsoft.Extensions.DependencyInjection;

namespace FormgenAssistant
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly ISettings _settings;
        public SettingsPage(ISettings settings)
        {
            InitializeComponent(); 
            _settings = settings;
            txtAddress.Text = _settings.Data.MailingAddress;

        }

        private void tglFormCodeCAPS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tglFormCodeCAPS.IsOn is null) return;

            _settings.Data.NotesCopyAll = (bool)tglFormCodeCAPS.IsOn;
            _settings.Save();
        }

        private void btnRevertAddress_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtAddress.Text = _settings.Data.MailingAddress;
        }

        private void btnUpdateAddress_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _settings.Data.MailingAddress = txtAddress.Text;
            _settings.Save();
        }
    }
}
