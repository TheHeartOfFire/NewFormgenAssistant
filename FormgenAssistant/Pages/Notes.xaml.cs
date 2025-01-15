using FormgenAssistant.SavedItems;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
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
        private List<NotesInfo> NotesList = [];
        private struct NotesInfo
        {
            public string ServerId { get; set; }
            public string Companies { get; set; }
            public string Dealership { get; set; }
            public string ContactName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string NotesText { get; set; }
            public string CaseText { get; set; }
            public string FormsText { get; set; }
            public string DealText { get; set; }
            public TabItem TabItem { get; set; }

            public void Clear()
            {
                ServerId = string.Empty;
                Companies = string.Empty;
                Dealership = string.Empty;
                ContactName = string.Empty;
                Email = string.Empty;
                Phone = string.Empty;
                NotesText = string.Empty;
                CaseText = string.Empty;
                FormsText = string.Empty;
                DealText = string.Empty;
            }
        }
        public Notes()
        {
            InitializeComponent();
            NotesList.Add(new()
            {
                ServerId = txtServerId.Text,
                Companies = txtCompanies.Text,
                Dealership = txtDealer.Text,
                ContactName = txtName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                NotesText = txtNotes.Text,
                CaseText = txtCaseNo.Text,
                FormsText = txtForms.Text,
                DealText = txtDeal.Text,
                TabItem = tiTabItemTemplate
            });
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
            if (tcTabs is not null && tcTabs.SelectedItem is not null)
            {
                var selectedTab = tcTabs.SelectedItem as TabItem;
                if (selectedTab is not null)
                {
                    selectedTab.Header = txtCaseNo.Text.Equals(string.Empty) ? "No Case#": txtCaseNo.Text;
                }
            }
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

        private void btnNameGen_Click(object sender, RoutedEventArgs e)
        {
            txtForms.Text += FileNameGenerator.FileName;
        }

        private void TabControlContextMenuNew(object sender, RoutedEventArgs e)
        {
            var template = NotesList[0].TabItem;

            NotesInfo newNote = new()
            {
                ServerId = txtServerId.Text,
                Companies = txtCompanies.Text,
                Dealership = txtDealer.Text,
                ContactName = txtName.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                NotesText = txtNotes.Text,
                CaseText = txtCaseNo.Text,
                FormsText = txtForms.Text,
                DealText = txtDeal.Text,
                TabItem = template.Clone()
            };

            NotesList.Add(newNote);
            tcTabs.Items.Add(newNote.TabItem);
            tcTabs.SelectedItem = newNote.TabItem;
            SwitchNotes(newNote);

        }

        private void TabControlContextMenuClose(object sender, RoutedEventArgs e)
        {
            var selected = (TabItem)tcTabs.SelectedItem;

            if (selected is null) return;

            foreach (var note in NotesList)
            {
                if (note.TabItem is null ||
                    note.TabItem.Header is null) continue;

                if (note.TabItem.Equals(selected))
                {
                    return;
                }
            }

        }

        private void SwitchNotes(NotesInfo info)
        {
            txtServerId.Text = info.ServerId;
            txtCompanies.Text = info.Companies;
            txtDealer.Text = info.Dealership;
            txtName.Text = info.ContactName;
            txtEmail.Text = info.Email;
            txtPhone.Text = info.Phone;
            txtNotes.Text = info.NotesText;
            txtCaseNo.Text = info.CaseText;
            txtForms.Text = info.FormsText;
            txtDeal.Text = info.DealText;
            UpdateAll(info);
        }

        private void tcTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (TabItem)tcTabs.SelectedItem;
            if (selected is null) return;
            foreach (var note in NotesList)
            {
                if (note.TabItem is null ||
                    note.TabItem.Header is null) continue;
                if (note.TabItem.Equals(selected)) //TODO: improve this comparison method
                {
                    SwitchNotes(note);
                    return;
                }
            }
        }

        private static void UpdateAll(NotesInfo info)
        {
            CaseText = info.CaseText;
            Companies = info.Companies;
            ContactName = info.ContactName;
            DealText = info.DealText;
            Dealership = info.Dealership;
            Email = info.Email;
            FormsText = info.FormsText;
            NotesText = info.NotesText;
            Phone = info.Phone;
            ServerId = info.ServerId;
        }
    }
}
