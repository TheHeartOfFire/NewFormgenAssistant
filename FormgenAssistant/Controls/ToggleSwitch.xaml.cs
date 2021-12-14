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
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl
    {

        Thickness btnOff = new(-131, 0, 0, 0);
        Thickness btnOn = new(0, 0, -131, 0);

        public static DependencyProperty trackOff = DependencyProperty.Register(
            "TrackOff", typeof(SolidColorBrush),
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                defaultValue: new SolidColorBrush(Color.FromArgb(255, 15, 11, 30)),
            FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnTrackOffChanged)));

        private static void OnTrackOffChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)d;
            tSwitch.OnTrackChanged(e);
        }

        public static readonly DependencyProperty trackOn = DependencyProperty.Register(
            "TrackOn", typeof(SolidColorBrush), 
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                defaultValue: new SolidColorBrush(Color.FromArgb(255, 15, 11, 30)),
            FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnTrackOnChanged)));

        private static void OnTrackOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)d;
            tSwitch.OnTrackChanged(e);

        }

        private void OnTrackChanged(DependencyPropertyChangedEventArgs e)
        {
            rectTrack.Fill = (SolidColorBrush)e.NewValue;
        }

        public static readonly DependencyProperty handleOff = DependencyProperty.Register(
            "HandleOff", typeof(SolidColorBrush), 
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                defaultValue: new SolidColorBrush(Color.FromArgb(255, 200, 200, 200)),
            FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnHandleOffChanged)));

        private static void OnHandleOffChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)d;
            tSwitch.OnHandleChanged(e);

        }

        private void OnHandleChanged(DependencyPropertyChangedEventArgs e)
        {
            elipseHandle.Fill = (SolidColorBrush)e.NewValue;
        }

        public static readonly DependencyProperty handleOn = DependencyProperty.Register(
            "HandleOn", typeof(SolidColorBrush), 
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                defaultValue: new SolidColorBrush(Color.FromArgb(255, 123, 104, 238)),
            FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnHandleOnChanged)));

        private static void OnHandleOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)d;
            tSwitch.OnHandleChanged(e);

        }

        public static readonly DependencyProperty handleThirdState = DependencyProperty.Register(
           "HandleThirdState", typeof(SolidColorBrush),
           typeof(ToggleSwitch),
           new FrameworkPropertyMetadata(
               defaultValue: new SolidColorBrush(Color.FromArgb(255, 200, 200, 200)),
           FrameworkPropertyMetadataOptions.AffectsRender,
           new PropertyChangedCallback(OnHandleThirdStateChanged)));

        private static void OnHandleThirdStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)d;
            tSwitch.OnHandleChanged(e);

        }

        public static readonly DependencyProperty borderOff = DependencyProperty.Register(
            "BorderOff", typeof(SolidColorBrush), 
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                defaultValue: new SolidColorBrush(Color.FromArgb(255, 123, 104, 238)),
            FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnBorderOffChanged)));

        private static void OnBorderOffChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)d;
            tSwitch.OnBorderChanged(e);

        }

        public static readonly DependencyProperty borderOn = DependencyProperty.Register(
            "BorderOn", typeof(SolidColorBrush), 
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                defaultValue: new SolidColorBrush(Color.FromArgb(255, 123, 104, 238)),
            FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnBorderOnChanged)));

        private static void OnBorderOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)d;
            tSwitch.OnBorderChanged(e);

        }

        private void OnBorderChanged(DependencyPropertyChangedEventArgs e)
        {
            elipseHandle.Stroke = (SolidColorBrush)e.NewValue;
        }

        public static readonly DependencyProperty borderThickness = DependencyProperty.Register(
            "BorderChanged", typeof(double),
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                defaultValue: 2.0,
            FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnBorderThicknessChanged)));

        private static void OnBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)d;
            tSwitch.OnBorderThicknessChanged(e);
        }

        private void OnBorderThicknessChanged(DependencyPropertyChangedEventArgs e)
        {
            rectTrack.StrokeThickness = (double)e.NewValue;
        }

        public static readonly DependencyProperty isOn = DependencyProperty.Register(
            "IsOn", typeof(bool?),
            typeof(ToggleSwitch),
            new FrameworkPropertyMetadata(
                defaultValue: false,
            FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnIsOnChanged)));

        private static void OnIsOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)d;
            tSwitch.OnIsOnChanged(e);
        }

        private void OnIsOnChanged(DependencyPropertyChangedEventArgs e)
        {
            if ((bool?)e.NewValue == true)
            {
                rectTrack.Fill = (SolidColorBrush)GetValue(trackOn);
                rectTrack.Stroke = (SolidColorBrush)GetValue(borderOn);
                elipseHandle.Fill = (SolidColorBrush)GetValue(handleOn);
                elipseHandle.Margin = btnOn;
                return;
            }
            if ((bool?)e.NewValue == false)
            {
                rectTrack.Fill = (SolidColorBrush)GetValue(trackOff);
                rectTrack.Stroke = (SolidColorBrush)GetValue(borderOff);
                elipseHandle.Fill = (SolidColorBrush)GetValue(handleOff);
                elipseHandle.Margin = btnOff;
            }
        }

        private static readonly DependencyProperty threeState = DependencyProperty.Register(
           "ThreeState", typeof(bool),
           typeof(ToggleSwitch),
           new FrameworkPropertyMetadata(
               defaultValue: false,
           FrameworkPropertyMetadataOptions.AffectsRender,
           new PropertyChangedCallback(On3StateChanged)));

        private static void On3StateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleSwitch tSwitch = (ToggleSwitch)d;
            tSwitch.On3StateChanged(e);
        }

        private void On3StateChanged(DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                IsOn = null;
                elipseHandle.Fill = (SolidColorBrush)GetValue(handleThirdState);
                elipseHandle.Margin = new Thickness(0,0,0,0);
                return;
            }
            IsOn = false;
        }


        public ToggleSwitch()
        {
            InitializeComponent();

            elipseHandle.Fill = (SolidColorBrush)GetValue(handleOff);
            elipseHandle.Margin = btnOff;

            if ((bool)GetValue(threeState) && (bool?)GetValue(isOn) is null)
            {
                elipseHandle.Fill = (SolidColorBrush)GetValue(handleThirdState);
                elipseHandle.Margin = new Thickness(0, 0, 0, 0);
            }

            rectTrack.Fill = (SolidColorBrush)GetValue(trackOff);
            rectTrack.Stroke = (SolidColorBrush)GetValue(borderOff);
            rectTrack.StrokeThickness = (double)GetValue(borderThickness);
        }

        public bool? IsOn { get => (bool?)GetValue(isOn); set => SetValue(isOn, value); }
        public SolidColorBrush TrackOff { get => (SolidColorBrush)GetValue(trackOff); set => SetValue(trackOff, value); }
        public SolidColorBrush TrackOn { get => (SolidColorBrush)GetValue(trackOn); set => SetValue(trackOn, value); }
        public SolidColorBrush HandleOff { get => (SolidColorBrush)GetValue(handleOff); set => SetValue(handleOff, value); }
        public SolidColorBrush HandleOn { get => (SolidColorBrush)GetValue(handleOn); set => SetValue(handleOn, value); }
        public SolidColorBrush HandleThirdState { get => (SolidColorBrush)GetValue(handleThirdState); set => SetValue(handleThirdState, value); }
        public SolidColorBrush BorderOff { get => (SolidColorBrush)GetValue(borderOff); set => SetValue(borderOff, value); }
        public SolidColorBrush BorderOn { get => (SolidColorBrush)GetValue(borderOn); set => SetValue(borderOn, value); }
        public double BorderThickness1 { get => (double)GetValue(borderThickness); set => SetValue(borderThickness, value); }
        public bool ThreeState { get => (bool)GetValue(threeState); set => SetValue(threeState, value); }

        private void elipseHandle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToggleButton();
        }

        private void rectTrack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ToggleButton();
        }
        private void ToggleButton()
        {
            if ((bool)GetValue(threeState) && (bool?)GetValue(isOn) is null)//toggle is changing FROM third state->Turn it off
            {
                rectTrack.Fill = (SolidColorBrush)GetValue(trackOff);
                rectTrack.Stroke = (SolidColorBrush)GetValue(borderOff);
                elipseHandle.Fill = (SolidColorBrush)GetValue(handleOff);
                elipseHandle.Margin = btnOff;
                SetValue(isOn, false);
                return;
            }
            if ((bool?)GetValue(isOn) is true)//toggle is changing FROM on->go to third state
            {
                rectTrack.Fill = (SolidColorBrush)GetValue(trackOn);
                rectTrack.Stroke = (SolidColorBrush)GetValue(borderOn);
                elipseHandle.Fill = (SolidColorBrush)GetValue(handleThirdState);
                elipseHandle.Margin = new Thickness(0, 0, 0, 0);
                SetValue(isOn, null);
                return;
            }
            rectTrack.Fill = (SolidColorBrush)GetValue(trackOn);//Toggle is changing FROM off->turn it on
            rectTrack.Stroke = (SolidColorBrush)GetValue(borderOn);
            elipseHandle.Fill = (SolidColorBrush)GetValue(handleOn);
            elipseHandle.Margin = btnOn;
            SetValue(isOn, true);
        }
    }
}
