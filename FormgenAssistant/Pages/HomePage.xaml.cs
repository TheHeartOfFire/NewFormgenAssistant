using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Properties.Resources.GitHub,
                UseShellExecute = true
            }) ;
        }

        private void btnADP_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Properties.Resources.ADP,
                UseShellExecute = true
            });
        }

        private void btnSF_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Properties.Resources.SalesForce,
                UseShellExecute = true
            });
        }

        private void btnMyApps_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Properties.Resources.MyApps,
                UseShellExecute = true
            });
        }

        private void btnClientInfoReport_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Properties.Resources.ClientInfoReport,
                UseShellExecute = true
            });

        }

        private void btnFormsTracker_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Properties.Resources.FormsTracker,
                UseShellExecute = true
            });
        }

        private void btnWorkday_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Properties.Resources.Workday,
                UseShellExecute = true
            });
        }

        private void btnCST_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AmpsSupport\\CSTLoader.exe",
                UseShellExecute = true
            });
        }

        private void btnFormgen_Click(object sender, RoutedEventArgs e)
        {
            var Info = new System.Diagnostics.ProcessStartInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\NewFormGen.lnk")
            {
                UseShellExecute = true
            };

            System.Diagnostics.Process.Start(Info);

        }
    }
}
