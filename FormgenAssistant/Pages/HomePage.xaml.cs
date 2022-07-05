using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

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
            AddVersionNumber();
        }
        private void AddVersionNumber()
        {
            var assy = Assembly.GetExecutingAssembly();
            var version = FileVersionInfo.GetVersionInfo(assy.Location);
            lblVersion.Content += version.FileVersion;
        }
        
        private void btnADP_Click(object sender, RoutedEventArgs e) =>
            OpenLink(Properties.Resources.ADP);

        private void btnSF_Click(object sender, RoutedEventArgs e) =>
            OpenLink(Properties.Resources.SalesForce);

        private void btnMyApps_Click(object sender, RoutedEventArgs e) =>
            OpenLink(Properties.Resources.MyApps);

        private void btnClientInfoReport_Click(object sender, RoutedEventArgs e) =>
            OpenLink(Properties.Resources.ClientInfoReport);

        private void btnFormsTracker_Click(object sender, RoutedEventArgs e) =>
            OpenLink(Properties.Resources.FormsTracker);

        private void btnWorkday_Click(object sender, RoutedEventArgs e) =>
            OpenLink(Properties.Resources.Workday);

        private void btnCST_Click(object sender, RoutedEventArgs e) =>
            OpenLink(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AmpsSupport\\CSTLoader.exe");

        private void btnFormgen_Click(object sender, RoutedEventArgs e) =>
            OpenLink(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\NewFormGen.lnk");

        private static void OpenLink(string path) =>
            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
        
    }
}
