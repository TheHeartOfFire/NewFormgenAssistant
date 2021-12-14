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
        Dictionary<string, string> _servers = new Dictionary<string, string>();
        Dictionary<string, DealerInfo> _dealerships = new Dictionary<string, DealerInfo>();
        public ClientInfoReport()
        {
            InitializeComponent();
            _servers = Utils.Servers;
            _dealerships = Utils.Dealerships;

            cboServers.ItemsSource = _servers.Keys;
            cboGroups.ItemsSource = _servers.Values;
        }

        private void btnLookup_Click(object sender, RoutedEventArgs e)
        {
            LookupDealership();
        }

        private void LookupDealership()
        {
            if (cboServers.SelectedItem == null) return;
            string ID = cboServers.SelectedItem.ToString().ToUpperInvariant();

            if (!_servers.ContainsKey(ID)) return;

            if (!_dealerships.ContainsKey(ID))
            {
                var di = new DealerInfo(ID);
                Utils.Dealerships.Add(di.ServerID, di);
                Utils.SetDealersJson();
                _dealerships = Utils.Dealerships;
            }
                
        }


        private void ViewDealership()
        {
            if (cboServers.SelectedItem == null) return;
            string ID = cboServers.SelectedItem.ToString().ToUpperInvariant();

            if (!_dealerships.ContainsKey(ID))
                LookupDealership();



        }


    }
}
