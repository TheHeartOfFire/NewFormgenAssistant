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
    /// Interaction logic for CodeBlock.xaml
    /// </summary>
    public partial class CodeBlock : UserControl
    {
        
        public CodeBlock()
        {
            InitializeComponent();
            BackgroundProperty.OverrideMetadata(typeof(CodeBlock), new FrameworkPropertyMetadata(
                new SolidColorBrush(Color.FromArgb(255, 15, 11, 30)), FrameworkPropertyMetadataOptions.AffectsRender, OnBackgroundChanged));
            ForegroundProperty.OverrideMetadata(typeof(CodeBlock), new FrameworkPropertyMetadata(
                new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), FrameworkPropertyMetadataOptions.AffectsRender, OnForegroundChanged));
            BorderBrushProperty.OverrideMetadata(typeof(CodeBlock), new FrameworkPropertyMetadata(
                new SolidColorBrush(Colors.MediumSlateBlue), FrameworkPropertyMetadataOptions.AffectsRender, OnBorderBrushChanged));
        }


        private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cBlock = (CodeBlock) d;

            cBlock.OnBackgroundColorChanged(e);
        }

        private void OnBackgroundColorChanged(DependencyPropertyChangedEventArgs e)
        {
            grdBackground.Background = (SolidColorBrush)e.NewValue;
        }
        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cBlock = (CodeBlock)d;

            cBlock.OnForegroundColorChanged(e);
        }

        private void OnForegroundColorChanged(DependencyPropertyChangedEventArgs e)
        {
            Title.Foreground = (SolidColorBrush)e.NewValue;
        }
        private static void OnBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cBlock = (CodeBlock)d;

            cBlock.OnBorderBrushColorChanged(e);
        }

        private void OnBorderBrushColorChanged(DependencyPropertyChangedEventArgs e)
        {
            Title.BorderBrush = (SolidColorBrush)e.NewValue;
        }
    }
}
