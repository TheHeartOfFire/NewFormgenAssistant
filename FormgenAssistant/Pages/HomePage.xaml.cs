using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            txtAddress.Text = SavedItems.Settings.Instance.MailingAddress;
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
        
        private void txtAddress_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAddress.Text))
                txtAddress.SelectAll();
            
        }

        private void txtAddress_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var mouseDownEvent = new MouseButtonEventArgs(
                Mouse.PrimaryDevice,
                Environment.TickCount,
                MouseButton.Right)
            {
                RoutedEvent = Mouse.MouseUpEvent,
                Source = txtAddress,
            };
            
            InputManager.Current.ProcessInput(mouseDownEvent);
        }
        
    }
}
