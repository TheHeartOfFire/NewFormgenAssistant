using FormgenAssistant.SavedItems;
using Microsoft.Win32;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private async void btnLookupDealers_Click(object sender, RoutedEventArgs e)
        {
            return;//disabled for now

            var result = MessageBox.Show("This process takes over an hour to complete if no other dealerships have already been downloaded.\n\nPlease do not perform this action during normal working house.\n\nDo you wish to proceed?", "Are you sure?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                
                prgDealerLookup.Visibility = Visibility.Visible;
                prgDealerLookup.IsIndeterminate = true;

                var progress = new Progress<double>();
                progress.ProgressChanged+=Progress_ProgressChanged;

                _=Task.Run(() => Utils.UpdateAllServers(progress).ConfigureAwait(false));
                
            }

        }

        private void Progress_ProgressChanged(object? sender, double e)
        {
            if (e == 0.0)
            {
                prgDealerLookup.Visibility = Visibility.Hidden;
                _dealers.Save();
            }

            if(e == 1.0)
            {
                prgDealerLookup.IsIndeterminate = false;
                prgDealerLookup.Maximum = Utils.Servers.Count;
            }

            prgDealerLookup.Value = e;
        }

        private void tglFormCodeCAPS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tglFormCodeCAPS.IsOn is null) return;

            _settings.Notes_CopyAll = (bool)tglFormCodeCAPS.IsOn;
            Settings.Save();
        }
    }
}
