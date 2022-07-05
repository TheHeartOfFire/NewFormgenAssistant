using FormgenAssistant.SavedItems;
using System.Windows.Controls;
using System.Windows.Input;

namespace FormgenAssistant
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        private static readonly Dealers _dealers = Dealers.Instance;
        private static readonly Settings _settings = Settings.Instance;
        public SettingsPage()
        {
            InitializeComponent();

        }
        
        private void tglFormCodeCAPS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tglFormCodeCAPS.IsOn is null) return;

            _settings.NotesCopyAll = (bool)tglFormCodeCAPS.IsOn;
            Settings.Save();
        }
    }
}
