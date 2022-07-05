using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FormgenAssistant.Controls
{
    /// <summary>
    /// Interaction logic for TextBox.xaml
    /// </summary>
    public partial class NewTextBox : UserControl
    {
        private static readonly DependencyProperty text = DependencyProperty.Register(
            "Text", typeof(string),
            typeof(NewTextBox),
            new FrameworkPropertyMetadata(
                defaultValue: string.Empty,
            FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnTextValChanged)));

        private static void OnTextValChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NewTextBox tBox = (NewTextBox)d;
            tBox.OnTextValChanged(e);

        }

        private void OnTextValChanged(DependencyPropertyChangedEventArgs e)
        {
            textBox.Text = (string)e.NewValue;
        }

        private static readonly DependencyProperty textWrapping = DependencyProperty.Register(
            "TextWrapping", typeof(TextWrapping),
            typeof(NewTextBox),
            new FrameworkPropertyMetadata(
                defaultValue: TextWrapping.NoWrap,
            FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnTextWrapChanged)));

        private static void OnTextWrapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NewTextBox tBox = (NewTextBox)d;
            tBox.OnTextWrapChanged(e);

        }

        private void OnTextWrapChanged(DependencyPropertyChangedEventArgs e)
        {
            textBox.TextWrapping = (TextWrapping)e.NewValue;
        }

        public event EventHandler? OnTextChanged;
        public NewTextBox()
        {
            InitializeComponent();
            textBox.Text = (string)GetValue(text);
            textBox.TextChanged += textBox_TextChanged;
        }

        public string Text { get => (string)GetValue(text); set => SetValue(text, value); }

        public TextWrapping TextWrapping{ get => (TextWrapping) GetValue(textWrapping); set => SetValue(textWrapping, value);
    }

    private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged?.Invoke(sender, e);
            SetValue(text, textBox.Text);
        }

        private void Grid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            textBox.Focus();
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {

            textBox.Focus();
        }
    }
}
