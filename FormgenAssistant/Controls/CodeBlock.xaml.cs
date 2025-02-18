using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FormgenAssistantLibrary.DataTypes.Code;

namespace FormgenAssistant.Controls
{
    /// <summary>
    /// Interaction logic for CodeBlock.xaml
    /// </summary>
    public partial class CodeBlock : UserControl
    {
        public CodeBase Code { get; set; }
        public CodeBlock(CodeBase code)
        {
            Code = code;
            InitializeComponent();
            lblName.Content = code.Name;
            ForegroundProperty.OverrideMetadata(typeof(CodeBlock), new FrameworkPropertyMetadata(
                new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), FrameworkPropertyMetadataOptions.AffectsRender, OnForegroundChanged));
        }

        
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cBlock = (CodeBlock)d;

            cBlock.OnForegroundColorChanged(e);
        }

        private void OnForegroundColorChanged(DependencyPropertyChangedEventArgs e)
        {
            lblName.Foreground = (SolidColorBrush)e.NewValue;
        }
    }
}
