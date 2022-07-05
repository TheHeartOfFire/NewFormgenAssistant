using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FormgenAssistant.Controls
{
    /// <summary>
    /// Interaction logic for NiceRadioButotn.xaml
    /// </summary>
    public partial class NiceRadioButotn : UserControl
    {
        SolidColorBrush innerCircleOn = new(Color.FromArgb(255, 123, 104, 238));
        SolidColorBrush innerCircleOff = new(Color.FromArgb(0,0,0,0));
        SolidColorBrush outerCircleOn = new(Color.FromArgb(255, 123, 104, 238));
        SolidColorBrush outerCircleOff = new(Color.FromArgb(255, 123, 104, 238));
        double borderThickness = 2f;
        bool isOn = false;
        public NiceRadioButotn()
        {
            InitializeComponent();
            innerCircle.Fill = innerCircleOff;
            outerCircle.Stroke = outerCircleOff;
            outerCircle.StrokeThickness = borderThickness;
        }

        public SolidColorBrush InnerCircleOn { get => innerCircleOn; set => innerCircleOn=value; }
        public SolidColorBrush InnerCircleOff { get => innerCircleOff; set => innerCircleOff=value; }
        public SolidColorBrush OuterCircleOn { get => outerCircleOn; set => outerCircleOn=value; }
        public SolidColorBrush OuterCircleOff { get => outerCircleOff; set => outerCircleOff=value; }
        public bool IsOn { get => isOn; set => isOn=value; }
        public double BorderThickness1 { get => borderThickness; set => borderThickness=value; }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isOn = !isOn;

            if (isOn)
            {
                innerCircle.Fill = innerCircleOn;
                outerCircle.Stroke = outerCircleOn;
                return;
            }
            innerCircle.Fill = innerCircleOff;
            outerCircle.Stroke = outerCircleOff;
        }

        private void outterCircle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isOn = !isOn;

            if (isOn)
            {
                innerCircle.Fill = innerCircleOn;
                outerCircle.Stroke = outerCircleOn;
                return;
            }
            innerCircle.Fill = innerCircleOff;
            outerCircle.Stroke = outerCircleOff;
        }
    }
}
