using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using FormgenAssistant.SavedItems;

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class CodeSnippets : UserControl
    {
        private readonly Snippets Snippets = Snippets.Instance;
        public CodeSnippets()
        {
            
            InitializeComponent();
            InitializeSnippetList();

        }

        private void UpdateContent()
        {
            var selectedSnippet = Snippets.CodeSnippets.FirstOrDefault(x => x.Name == ((ListBoxItem)lstSnippets.SelectedItem).Content);
            if (selectedSnippet is null) return;
            txtDescription.Text = selectedSnippet.Description;
            wrpInputs.Children.Clear();
            foreach (var input in selectedSnippet.Inputs)
            {
                var textBox = new TextBox
                {
                    Text = input,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5),
                    Background = new SolidColorBrush() { Color = Color.FromArgb(255, 32, 25, 56) },
                    Foreground = new SolidColorBrush() { Color = Color.FromArgb(255, 255, 255, 255) },
                    BorderBrush = new SolidColorBrush() { Color = Colors.MediumSlateBlue },
                    ToolTip = input,
                    MinWidth = 75
                };
                textBox.GotFocus += (s, e) => textBox.Text = textBox.Text == input ? "": textBox.Text;
                textBox.LostFocus += (s, e) =>
                    textBox.Text = string.IsNullOrEmpty(textBox.Text) ? input : textBox.Text;
                textBox.TextChanged += (s,e) => UpdateOutput();

                wrpInputs.Children.Add(textBox);
            }
            UpdateOutput();
        }
        
        private void InitializeSnippetList()
        {
            foreach (var snippet in Snippets.CodeSnippets)
            {
                var item = new ListBoxItem
                {
                    Content = snippet.Name,
                    ToolTip = snippet.Description
                };
                lstSnippets.Items.Add(item);
            }
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
        private void btnCopy_Click(object sender, RoutedEventArgs e) => Clipboard.SetText(txtOutput.Text);

        private void lstSnippets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateContent();
        }

        private void UpdateOutput()
        {
            var selectedSnippet = Snippets.CodeSnippets.FirstOrDefault(x => x.Name == ((ListBoxItem)lstSnippets.SelectedItem).Content);
            if (selectedSnippet is null) return;

            var boxes = wrpInputs.Children;
            var inputs = (from object? box in boxes select box as TextBox into textBox where textBox is not null select textBox.Text).ToList();
            txtOutput.Text = selectedSnippet.GetCode(inputs.ToArray());
        }
    }
}
