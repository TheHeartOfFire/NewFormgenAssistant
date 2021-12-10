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

namespace FormgenAssistant.Controls
{
    /// <summary>
    /// Interaction logic for TextBox.xaml
    /// </summary>
    public partial class NewTextBox : UserControl
    {
        public static readonly DependencyProperty text = DependencyProperty.Register(
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

        public event EventHandler? OnTextChanged;
        public NewTextBox()
        {
            InitializeComponent();
            textBox.Text = (string)GetValue(text);
            textBox.TextChanged += textBox_TextChanged;
        }

        public string Text { get => (string)GetValue(text); set => SetValue(text, value); }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged?.Invoke(sender, e);
            SetValue(text, textBox.Text);
        }
    }
}
