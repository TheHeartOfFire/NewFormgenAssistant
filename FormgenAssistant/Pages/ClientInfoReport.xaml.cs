using FormgenAssistant.SavedItems;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for ClientInfoReport.xaml
    /// </summary>
    public partial class ClientInfoReport : UserControl
    {
        private static readonly Dealers _dealers = Dealers.Instance;
        public ClientInfoReport()
        {
            InitializeComponent();

            cboServers.ItemsSource = Utils.Servers.Keys;
            cboGroups.ItemsSource = Utils.Servers.Values;
        }

        private void btnLookup_Click(object sender, RoutedEventArgs e)
        {
            LookupDealership();
        }

        private void LookupDealership()
        {
            if (cboServers is null || cboServers.SelectedItem is null) return;

            string ID = cboServers.SelectedItem.ToString().ToUpperInvariant();

            if (!Utils.Servers.ContainsKey(ID)) return;

            if (!Utils.Dealers.ContainsKey(ID))
            {
                var di = new DealerInfo(ID);
                Utils.Dealers.Add(di.ServerID, di);
                _dealers.Save();
            }
                
        }


        private void ViewDealership()
        {
            if (cboServers.SelectedItem == null) return;
            string ID = cboServers.SelectedItem.ToString().ToUpperInvariant();

            if (!Utils.Dealers.ContainsKey(ID))
                LookupDealership();
        }


    }
}
