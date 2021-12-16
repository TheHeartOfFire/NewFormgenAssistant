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
    /// Interaction logic for Notes.xaml
    /// </summary>
    public partial class Notes : UserControl
    {
        public Notes()
        {
            InitializeComponent();
        }

        private void btnCopyServer_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtServerId.Text);
        private void btnCopyCompanies_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtCompanies.Text);
        private void btnCopyDealer_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtDealer.Text);
        private void btnCopyName_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtName.Text);
        private void btnCopyEmail_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtEmail.Text);
        private void btnCopyPhone_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtPhone.Text + " x" + txtPhoneExt);
        private void btnCopyNotes_Click(object sender, RoutedEventArgs e) =>  Clipboard.SetText(txtNotes.Text);

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtServerId.Text = string.Empty;
            txtCompanies.Text = string.Empty;
            txtDealer.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtPhoneExt.Text = string.Empty;
            txtNotes.Text = string.Empty;
        }
    }
}
