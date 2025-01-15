﻿using FormgenAssistant.SavedItems;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for Notes.xaml
    /// </summary>
    public partial class Notes : UserControl
    {
        public static string? ServerId { get; private set; }
        public static string? Companies { get; private set; }
        public static string? Dealership { get; private set; }
        public static string? ContactName { get; private set; }
        public static string? Email { get; private set; }
        public static string? Phone { get; private set; }
        public static string? NotesText { get; private set; }
        public static string? CaseText { get; private set; }
        public static string? FormsText { get; private set; }
        public static string? DealText { get; private set; }
        public Notes()
        {
            InitializeComponent();
        }

        private void btnCopyServer_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtServerId.Text);
        private void btnCopyCompanies_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtCompanies.Text);
        private void btnCopyDealer_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtDealer.Text);
        private void btnCopyName_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtName.Text);
        private void btnCopyEmail_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtEmail.Text);
        private void btnCopyPhone_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtPhone.Text != "" ? txtPhone.Text + (txtPhoneExt.Text != "" ? " x" + txtPhoneExt.Text : "") : "");
        private void btnCopyNotes_Click(object sender, RoutedEventArgs e) =>  Clipboard.SetText(txtNotes.Text);
        private void btnCopyCase_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtCaseNo.Text);
        private void btnCopyForms_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtForms.Text);
        private void btnCopyDeal_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtDeal.Text);

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if(Settings.Instance.NotesCopyAll) CopyAll();
            txtServerId.Text = string.Empty;
            txtCompanies.Text = string.Empty;
            txtDealer.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtPhoneExt.Text = string.Empty;
            txtNotes.Text = string.Empty;
            txtCaseNo.Text = string.Empty;
            txtForms.Text = string.Empty;
            txtDeal.Text = string.Empty;
        }

        private void btnCopyAll_Click(object sender, RoutedEventArgs e)
        {
            CopyAll();
        }

        private void CopyAll()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Server: " + txtServerId.Text + "\tCompany(s): " + txtCompanies.Text);
            sb.AppendLine("Dealership: " + txtDealer.Text);
            sb.AppendLine("Contact: " + txtName.Text + "\tE-Mail: " + txtEmail.Text + "\tPhone: " + (txtPhone.Text != "" ? txtPhone.Text + (txtPhoneExt.Text != "" ? " x" + txtPhoneExt.Text : "") : ""));
            sb.AppendLine("Case: " + txtCaseNo.Text + "\nTest Deal: " + txtDeal.Text + "\nForms: " + txtForms.Text);
            sb.AppendLine("Notes: " + txtNotes.Text);
            Clipboard.SetText(sb.ToString());
        }

        private void txtServerId_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.ServerId = txtServerId.Text.ToUpper();
        }

        private void txtCompanies_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.Companies = txtCompanies.Text;
        }

        private void txtDealer_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.Dealership = txtDealer.Text;
        }

        private void txtName_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.ContactName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(txtName.Text.ToLowerInvariant());
        }

        private void txtEmail_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.Email = txtEmail.Text;
        }

        private void txtPhone_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.Phone = txtPhone.Text != "" ? txtPhone.Text + (txtPhoneExt.Text != "" ? " x" + txtPhoneExt.Text : "") : "";
        }

        private void txtPhoneExt_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.Phone = txtPhone.Text != "" ? txtPhone.Text + (txtPhoneExt.Text != "" ? " x" + txtPhoneExt.Text : "") : "";
        }

        private void txtNotes_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.NotesText = txtNotes.Text;
        }

        private void txtCaseNo_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.CaseText = txtCaseNo.Text;
        }

        private void txtForms_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.FormsText = txtForms.Text;
        }
        private void txtDeal_OnTextChanged(object sender, System.EventArgs e)
        {
            Notes.DealText = txtDeal.Text;
        }

        private void txtServerId_LostFocus(object sender, RoutedEventArgs e)
        {
            txtServerId.Text = txtServerId.Text.ToUpperInvariant();
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtName.Text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(txtName.Text.ToLowerInvariant());
        }
    }
}
