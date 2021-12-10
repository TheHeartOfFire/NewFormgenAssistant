using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for FileNameGenerator.xaml
    /// </summary>
    public partial class FileNameGenerator : UserControl
    {
        List<string> StateCodes = new List<string> { "AK","AZ","AR","CA","CO","CT","DE","DC","FL","GA","HI","ID","IL","IN","IA","KS","KY","LA","ME","MD","MA","MI","MN","MS","MO","MT","NE","NV","NH","NJ","NM","NY","NC","ND","OH","OK","OR","PA","PR","RI","SC","SD","TN","TX","UT","VT","VA","VI","WA","WV","WI","WY"};
        public FileNameGenerator()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cboStates.ItemsSource = StateCodes;
        }

        private void UpdateFormName()
        {
            if (!tglFormType.IsOn && tglLAW.IsOn)
            {
                txtOutput.Text = LaserLaw();
                return;
            }
            if (!tglFormType.IsOn)
            {
                txtOutput.Text = Laser();
                return;
            }
            if (tglFormType.IsOn && tglLAW.IsOn)
            {
                txtOutput.Text = ImpactLaw();
                return;
            }
            if (tglFormType.IsOn)
            {
                txtOutput.Text = Impact();
                return;
            }
        }

        private string LaserLaw()
        {
            string result = "LAW ";
            result += txtCoBank.Text != "" ? txtCoBank.Text + " " : "";
            result += tglFormNameTitle.IsOn ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(txtFormName.Text) : txtFormName.Text;
            result += (txtCode.Text != "" || txtDate.Text != "") ? " [LAW " : "";
            result += txtCode.Text != "" ? tglFormCodeCAPS.IsOn ? txtCode.Text.ToUpperInvariant() : txtCode.Text + " " : "";
            result += txtDate.Text != "" ? txtDate.Text + " " : "";
            result += (txtCode.Text != "" || txtDate.Text != "") ? "] " : "";
            result += txtOEMDealer.Text != "" ? "(" + txtOEMDealer.Text + ")" : "";
            result += tglCustom.IsOn ? " - Custom" : "";
            return result.Replace('/','-');
        }

        private string Laser()
        {
            string result = "";
            result += cboStates.Text != "" ? cboStates.Text + " " : "";
            result += txtCoBank.Text != "" ? txtCoBank.Text + " " : "";
            result += tglFormNameTitle.IsOn ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(txtFormName.Text) : txtFormName.Text;

            result += (txtCode.Text != "" || txtDate.Text != "" || txtOEMDealer.Text != "") ? " [" : "";
            result += txtCode.Text != "" ? tglFormCodeCAPS.IsOn ? txtCode.Text.ToUpperInvariant() : txtCode.Text + " " : "";
            result += ((txtCode.Text != "" || txtOEMDealer.Text != "") && txtDate.Text != "") ? " (" : "";
            result += txtDate.Text != "" ? txtDate.Text + " " : "";
            result += ((txtCode.Text != "" || txtOEMDealer.Text != "") && txtDate.Text != "") ? ")" : "";
            result += txtOEMDealer.Text != "" ? "(" + txtOEMDealer.Text + ")" : "";
            result += (txtCode.Text != "" || txtDate.Text != "" || txtOEMDealer.Text != "") ? "] " : "";

            result += tglCustom.IsOn ? " - Custom" : "";
            result += tglVM.IsOn ? " - VM" : "";
            return result.Replace('/', '-');
        }

        private string ImpactLaw()
        {
            string result = "LAW ";
            result += txtCode.Text != "" ? tglFormCodeCAPS.IsOn ? txtCode.Text.ToUpperInvariant() : txtCode.Text + " " : "";
            result += txtDate.Text != "" ? txtDate.Text + " " : "";
            result += txtCoBank.Text != "" ? txtCoBank.Text + " " : "";
            result += tglFormNameTitle.IsOn ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(txtFormName.Text) : txtFormName.Text;
            result += txtOEMDealer.Text != "" ? "(" + txtOEMDealer.Text + ")" : "";
            result += tglCustom.IsOn ? " - Custom" : "";
            result += tglVM.IsOn ? " - VM" : "";
            return result.Replace('/', '-');
        }

        private string Impact()
        {
            string result = "";
            result += cboStates.Text != "" ? cboStates.Text + " " : "";
            result += txtCoBank.Text != "" ? txtCoBank.Text + " " : "";
            result += tglFormNameTitle.IsOn ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(txtFormName.Text) : txtFormName.Text;

            result += (txtCode.Text != "" || txtDate.Text != "" || txtOEMDealer.Text != "") ? " (" : "";
            result += txtCode.Text != "" ? tglFormCodeCAPS.IsOn ? txtCode.Text.ToUpperInvariant() : txtCode.Text + " " : "";
            result += ((txtCode.Text != "" || txtOEMDealer.Text != "") && txtDate.Text != "") ? " [" : "";
            result += txtDate.Text != "" ? txtDate.Text + " " : "";
            result += ((txtCode.Text != "" || txtOEMDealer.Text != "") && txtDate.Text != "") ? "]" : "";
            result += txtOEMDealer.Text != "" ? "[" + txtOEMDealer.Text + "]" : "";
            result += (txtCode.Text != "" || txtDate.Text != "" || txtOEMDealer.Text != "") ? ") " : "";

            result += tglCustom.IsOn ? " - Custom" : "";
            result += tglVM.IsOn ? " - VM" : "";
            return result.Replace('/', '-');
        }

        private void UpdateFormName(object sender, EventArgs e)
        {
            UpdateFormName();
        }

        private void UpdateFormName(object sender, MouseButtonEventArgs e)
        {
            UpdateFormName();
        }

        private void txtOutput_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOutput.Text))
            {
                txtOutput.SelectAll();
            }
        }

        private void txtOutput_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var mouseDownEvent = new MouseButtonEventArgs(
                Mouse.PrimaryDevice,
                Environment.TickCount,
                MouseButton.Right)
                {
                    RoutedEvent = Mouse.MouseUpEvent,
                    Source = txtOutput,
                };


            InputManager.Current.ProcessInput(mouseDownEvent);
        }
    }
}
