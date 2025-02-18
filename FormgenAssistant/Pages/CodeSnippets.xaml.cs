using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FormgenAssistant.Controls;
using FormgenAssistantLibrary.DataTypes.Code;
using FormgenAssistantLibrary.DataTypes.Code.Formulae;
using FormgenAssistantLibrary.DataTypes.Code.Functions;
using FormgenAssistantLibrary.Interfaces;
using FormgenAssistantLibrary.Interfaces.DI;

namespace FormgenAssistant.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class CodeSnippets : Page
    {
        private readonly ICodeSnippetGenerator _snippetGenerator;
        public CodeSnippets(ICodeSnippetGenerator snippetGenerator)
        {
            _snippetGenerator = snippetGenerator;

            InitializeComponent();
            InitializeSnippetList();
            stkInputs.Children.Insert(0, CreateFunctionExpander(new CityStateZIPCodeTest()));
            
        }


        private void UpdateContent()
        {
            var selectedSnippet = _snippetGenerator.Snippets.FirstOrDefault(x => x.Name == ((ListBoxItem) lstSnippets.SelectedItem).Content as string);

            AddPrompts.Visibility = Visibility.Collapsed;
            RemovePrompts.Visibility = Visibility.Collapsed;
            if (selectedSnippet is null) return;

            if(selectedSnippet is IExtendableCode)
                AddPrompts.Visibility = Visibility.Visible;


            txtDescription.Text = selectedSnippet.Description;
            GenerateInputBoxes(selectedSnippet);
            UpdateOutput();
        }

        private void GenerateInputBoxes(CodeBase selectedSnippet)
        {
            wrpInputs.Children.Clear();
            foreach (var textBox in selectedSnippet.Inputs.Select(CreateInputTextBox))
            {
                wrpInputs.Children.Add(textBox);
            }
        }

        private Expander CreateFunctionExpander(CodeBase input)
        {
            var stack = new StackPanel();

            var expander = new Expander
            {
                Header = input.GetToken(),
                Foreground = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.MediumSlateBlue), 
                Content = stack,
                IsExpanded = true
            };

            var wrap = new WrapPanel();
            foreach (var value in input.Inputs)
            {
                if (value.Value is CodeBase @base)
                {
                    //wrap.Children.Add(CreateInputTextBox(value));
                    wrap.Children.Add(CreateFunctionExpander(@base));
                }
                else
                    wrap.Children.Add(CreateInputTextBox(value));
            }

            stack.Children.Add(wrap);

            return expander;
        }
        

        private TextBox CreateInputTextBox(CodeInput input)
        {
            var isCode = input.Value is CodeBase;
            var inputValue = input.Value as CodeBase;
            var inputDescription = isCode ? inputValue!.GetToken() : input.Value as string;
            var textBox = new TextBox
            {
                Text = inputDescription,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Background = new SolidColorBrush(Color.FromArgb(255, 32, 25, 56)),
                Foreground = new SolidColorBrush(isCode ? Colors.Red : Colors.White),
                BorderBrush = new SolidColorBrush(Colors.MediumSlateBlue),
                SelectionBrush = new SolidColorBrush(Color.FromArgb(76, 255, 255, 255)),
                ToolTip = input,
                MinWidth = 75
            };
            textBox.GotFocus += (s, e) => textBox.Text = textBox.Text == inputDescription ? "" : textBox.Text;
            textBox.LostFocus += (s, e) =>
                textBox.Text = string.IsNullOrEmpty(textBox.Text) ? inputDescription : textBox.Text;
            
                textBox.TextChanged += (s, e) => UpdateTestOutput();
            
            return textBox;
        }

        private void InitializeSnippetList()
        {

            foreach (var item in _snippetGenerator.Snippets.Select(snippet => new ListBoxItem
                     {
                         Content = snippet.Name,
                         ToolTip = snippet.Description
                     }))
            {
                lstSnippets.Items.Add(item);
            }

            lstSnippets.Items.Add(new CodeBlock(new CityStateZIPCodeTest()));
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
            var selectedSnippet = _snippetGenerator.Snippets.FirstOrDefault(x => x.Name == ((ListBoxItem)lstSnippets.SelectedItem).Content as string);
            if (selectedSnippet is null) return;

            //TODO: Allow CodeBase as Input

            var boxes = wrpInputs.Children;
            var inputs = (from object? box in boxes select box as TextBox into textBox where textBox is not null select textBox.Text).ToList();
            selectedSnippet.SetInputs(inputs);
            txtOutput.Text = selectedSnippet.GetCode();
        }
        private void UpdateTestOutput()
        {
            var selectedSnippet = _snippetGenerator.Snippets.FirstOrDefault(x => x.Name == ((ListBoxItem)lstSnippets.SelectedItem).Content as string);
            if (selectedSnippet is null) return;

            //TODO: Allow CodeBase as Input

            var boxes = wrpInputs.Children;
            var inputs = (from object? box in boxes select box as TextBox into textBox where textBox is not null select textBox.Text).ToList();
            selectedSnippet.SetInputs(inputs);
            txtOutput.Text = selectedSnippet.GetCode();
        }

        private void AddPrompts_Click(object sender, RoutedEventArgs e)
        {
            var selectedSnippet = _snippetGenerator.Snippets.FirstOrDefault(x => x.Name == ((ListBoxItem)lstSnippets.SelectedItem).Content as string);
            if (selectedSnippet is null) return;
            var snippetEx = selectedSnippet as IExtendableCode;
            snippetEx?.AddExtraInputs(1);
            RemovePrompts.Visibility = Visibility.Visible;
            UpdateContent();

        }

        private void RemovePrompts_Click(object sender, RoutedEventArgs e)
        {
            var selectedSnippet = _snippetGenerator.Snippets.FirstOrDefault(x => x.Name == ((ListBoxItem)lstSnippets.SelectedItem).Content as string);

            if (selectedSnippet is not IExtendableCode snippetEx) return;
            
            if (selectedSnippet.InputCount() > snippetEx.DefaultArgCount)
                snippetEx.RemoveExtraInputs(1);
            
            if (selectedSnippet.InputCount() <= snippetEx.DefaultArgCount + snippetEx.ArgIncrement) 
                RemovePrompts.Visibility = Visibility.Collapsed;

            UpdateContent();
        }
    }
}
