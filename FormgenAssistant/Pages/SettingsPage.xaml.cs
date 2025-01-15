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
            
            if (Settings.MailingAddress is null) return;

            txtName.Text = Settings.MailingAddress.Name;
            txtStreet.Text = Settings.MailingAddress.Street;
            txtCity.Text = Settings.MailingAddress.City;
            txtState.Text = Settings.MailingAddress.State;
            txtZip.Text = Settings.MailingAddress.PostalCode;

        }
        
        private void tglFormCodeCAPS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tglFormCodeCAPS.IsOn is null) return;

            Settings.NotesCopyAll = (bool)tglFormCodeCAPS.IsOn;
            Settings.Save();
        }

        private void btnRevertAddress_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(Settings.MailingAddress is null) return;

            txtName.Text = Settings.MailingAddress.Name;
            txtStreet.Text = Settings.MailingAddress.Street;
            txtCity.Text = Settings.MailingAddress.City;
            txtState.Text = Settings.MailingAddress.State;
            txtZip.Text = Settings.MailingAddress.PostalCode;
        }

        private void btnUpdateAddress_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Settings.MailingAddress = new(txtName.Text, txtStreet.Text, txtCity.Text, txtState.Text, txtZip.Text);
            Settings.Save();
        }

        private void tglSelectOnTemplateAdded_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tglSelectOnTemplateAdded.IsOn is null) return;

            Settings.SelectNewTemplate = (bool)tglSelectOnTemplateAdded.IsOn;
            Settings.Save();
        }
    }
}
