using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FormgenAssistant;
using FormgenAssistantLibrary.Interfaces.DI;
using Microsoft.Extensions.DependencyInjection;

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private readonly IUtils? _utils;
        private readonly ISettings _settings;
        private const string AdpLink = "https://online.adp.com/signin/v1";
        private const string SalesForceLink = "https://account.activedirectory.windowsazure.com/applications/signin/16d2b7b8-7000-4c9c-8476-212e03ddea4c?tenantId=c45b48f3-13bb-448b-9356-ba7b863c2189";
        private const string MyAppsLink = "https://myapplications.microsoft.com";
        private const string CLILink = "http://linux.automate.local/cgi-bin/getinfo2.cgi";
        private const string FormsTrackerLink = "https://solerainc-my.sharepoint.com/:x:/r/personal/felicia_carpenter_solera_com/_layouts/15/doc2.aspx?sourcedoc=%7B11CD9173-C662-4CB0-886A-1D98FB86D55C%7D&file=Forms%20Tracker.xlsx&action=default&mobileredirect=true&DefaultItemOpen=1&cid=b7817432-b777-48c0-ae5d-c94958348e0a";
        private const string WorkDayLink = "https://wd5.myworkday.com/solera/d/home.htmld";
        private const string CSTLink = "\\AmpsSupport\\CSTLoader.exe";
        private const string FormgenLink = "\\NewFormGen.lnk";
        
        public HomePage(IUtils utils, ISettings settings)
        {
            InitializeComponent();
            _utils = utils;
            _settings = settings;
            txtAddress.Text = _settings.Data.MailingAddress;
            AddVersionNumber();
        }
        private void AddVersionNumber()
        {
            var assy = Assembly.GetExecutingAssembly();
            var version = FileVersionInfo.GetVersionInfo(assy.Location);
            lblVersion.Content += version.FileVersion;
        }

        private void btnADP_Click(object sender, RoutedEventArgs e) =>
            _utils!.OpenLink(AdpLink);

        private void btnSF_Click(object sender, RoutedEventArgs e) => 
            _utils!.OpenLink(SalesForceLink);


        private void btnMyApps_Click(object sender, RoutedEventArgs e) =>
            _utils!.OpenLink(MyAppsLink);

        private void btnClientInfoReport_Click(object sender, RoutedEventArgs e) =>
            _utils!.OpenLink(CLILink);

        private void btnFormsTracker_Click(object sender, RoutedEventArgs e) =>
            _utils!.OpenLink(FormsTrackerLink);

        private void btnWorkday_Click(object sender, RoutedEventArgs e) =>
            _utils!.OpenLink(WorkDayLink);

        private void btnCST_Click(object sender, RoutedEventArgs e) =>
            _utils!.OpenLink(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + CSTLink);

        private void btnFormgen_Click(object sender, RoutedEventArgs e) =>
            _utils!.OpenLink(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + FormgenLink);
        
        
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
