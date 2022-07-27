using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FormgenAssistantLibrary.Interfaces.DI;

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for FileNameGenerator.xaml
    /// </summary>
    public partial class FileNameGenerator : Page
    {
        private readonly IFileNameGenerator _fileNameGenerator;
        
        
        public FileNameGenerator(IFileNameGenerator fileNameGenerator)
        {
            _fileNameGenerator = fileNameGenerator;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cboStates.ItemsSource = _fileNameGenerator.StateCodes;
        }

        private void UpdateFormName()
        {
            switch (tglFormType.IsOn)
            {
                case false when tglLAW.IsOn == true:
                    txtOutput.Text = _fileNameGenerator.LaserLaw(txtCoBank.Text, txtFormName.Text, txtCode.Text, 
                        txtDate.Text, txtOEMDealer.Text, tglFormNameTitle.IsOn!.Value, tglNewUsed.IsOn, 
                        tglFormCodeCAPS.IsOn!.Value, tglCustom.IsOn!.Value );
                    return;
                case false:
                    txtOutput.Text = _fileNameGenerator.Laser(cboStates.Text, txtCoBank.Text, tglFormNameTitle.IsOn!.Value,
                        txtFormName.Text,txtCode.Text, txtDate.Text, txtOEMDealer.Text, tglFormCodeCAPS.IsOn!.Value,
                        tglNewUsed.IsOn, tglCustom.IsOn!.Value, tglVM.IsOn!.Value);
                    return;
                case true when tglLAW.IsOn == true:
                    txtOutput.Text = _fileNameGenerator.ImpactLaw(txtCode.Text, tglFormCodeCAPS.IsOn!.Value, txtDate.Text,
                        txtCoBank.Text, tglFormNameTitle.IsOn!.Value, txtFormName.Text, txtOEMDealer.Text,
                        tglNewUsed.IsOn, tglCustom.IsOn!.Value, tglVM.IsOn!.Value);
                    return;
                case true:
                    txtOutput.Text = _fileNameGenerator.Impact(cboStates.Text, txtCoBank.Text, tglFormNameTitle.IsOn!.Value,
                        txtFormName.Text, txtDate.Text, txtCode.Text,   txtOEMDealer.Text,
                        tglFormCodeCAPS.IsOn!.Value, tglNewUsed.IsOn, tglCustom.IsOn!.Value, tglVM.IsOn!.Value);
                    return;
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cboStates.Text = string.Empty;
            txtOEMDealer.Text = string.Empty;
            txtCoBank.Text = string.Empty;
            txtFormName.Text = string.Empty;
            txtCode.Text = string.Empty;
            txtDate.Text = string.Empty;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtOutput.Text);
        }
    }
}
