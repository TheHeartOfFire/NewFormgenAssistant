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
using FormgenAssistant.DataTypes.Code;
using FormgenAssistant.DataTypes.Code.Formulae;
using FormgenAssistant.DataTypes.Code.Functions;
using FormgenAssistant.Interfaces;
using FormgenAssistant.SavedItems;

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class CodeSnippets : UserControl
    {
        private readonly List<CodeBase> _snippets = new();
        public CodeSnippets()
        {
            
            InitializeComponent();
            InitializeSnippetList();
        }
        
        private void UpdateContent()
        {
            var selectedSnippet = _snippets.FirstOrDefault(x => x.Name == ((ListBoxItem) lstSnippets.SelectedItem).Content as string);

            if (selectedSnippet is null) return;

            if(selectedSnippet is IExtendableCode)
                AddPrompts.Visibility = Visibility.Visible;
            else
            {
                AddPrompts.Visibility = Visibility.Collapsed;
                RemovePrompts.Visibility = Visibility.Collapsed;
            }


                txtDescription.Text = selectedSnippet.Description;
            wrpInputs.Children.Clear();
            foreach (var input in selectedSnippet.InputDescriptions)
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
            _snippets.Add(new CityStateZIPCode());
            _snippets.Add(new DateConversionCode());
            _snippets.Add(new DayAndSuffixCode());
            _snippets.Add(new MonthNameCode());
            _snippets.Add(new NumToTextCode());
            _snippets.Add(new SeplistNumber());
            _snippets.Add(new CaseCode());
            _snippets.Add(new IfCode());
            _snippets.Add(new SeplistCode());

            foreach (var item in _snippets.Select(snippet => new ListBoxItem
                     {
                         Content = snippet.Name,
                         ToolTip = snippet.Description
                     }))
            {
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
            var selectedSnippet = _snippets.FirstOrDefault(x => x.Name == ((ListBoxItem)lstSnippets.SelectedItem).Content as string);
            if (selectedSnippet is null) return;

            var boxes = wrpInputs.Children;
            var inputs = (from object? box in boxes select box as TextBox into textBox where textBox is not null select textBox.Text).ToList();
            
            txtOutput.Text = selectedSnippet.SetInputs(inputs);
        }

        private void AddPrompts_Click(object sender, RoutedEventArgs e)
        {
            var selectedSnippet = _snippets.FirstOrDefault(x => x.Name == ((ListBoxItem)lstSnippets.SelectedItem).Content as string);
            if (selectedSnippet is null) return;
            var snippetEx = selectedSnippet as IExtendableCode;
            snippetEx?.AddExtraInputs(1);
            RemovePrompts.Visibility = Visibility.Visible;
            UpdateContent();

        }

        private void RemovePrompts_Click(object sender, RoutedEventArgs e)
        {
            var selectedSnippet = _snippets.FirstOrDefault(x => x.Name == ((ListBoxItem)lstSnippets.SelectedItem).Content as string);

            if (selectedSnippet is not IExtendableCode snippetEx) return;
            
            if (selectedSnippet.InputCount() > snippetEx.DefaultArgCount)
                snippetEx.RemoveExtraInputs(1);
            
            if (selectedSnippet.InputCount() <= snippetEx.DefaultArgCount + snippetEx.ArgIncriment) 
                RemovePrompts.Visibility = Visibility.Collapsed;

            UpdateContent();
        }
    }
}
