using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FormgenAssistant
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("This process takes over an hour to complete if no other dealerships have already been downloaded.\n\nPlease do not perform this action during normal working house.\n\nDo you wish to proceed?", "Are you sure?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                
                prgDealerLookup.Visibility = Visibility.Visible;
                prgDealerLookup.IsIndeterminate = true;

                var progress = new Progress<double>();
                progress.ProgressChanged+=Progress_ProgressChanged;


                await Utils.UpdateAllServers(prgDealerLookup, progress).ConfigureAwait(false);
                
            }

        }

        private void Progress_ProgressChanged(object? sender, double e)
        {
            if (e == 0.0)
                prgDealerLookup.Visibility = Visibility.Hidden;
            prgDealerLookup.Value = e;
        }
    }
}
