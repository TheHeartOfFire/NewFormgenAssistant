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
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
using FormgenAssistant.BubblePages;

namespace FormgenAssistant;
/// <summary>
/// Interaction logic for Bubbles.xaml
/// </summary>
public partial class Bubbles : Window
{
    private Storyboard myStoryboard = new();
    private Screen Screen;
    private readonly int Margin = 10;
    private Point Dpi = new();
    private double ScaleFactor = 1;
    

    public Bubbles()
    {
        InitializeComponent();
        Screen = Screen.FromHandle(new WindowInteropHelper(this).Handle);
        Dpi = Display(Screen, DpiType.Effective);
        ScaleFactor = Dpi.X / 96.0;

        Loaded += Bubbles_Loaded;
        
    }

    private void Bubbles_Loaded(object sender, RoutedEventArgs e)
    {
        Left = (1 / ScaleFactor) * (Screen.Bounds.Width - Margin) - ActualWidth;
        Top = (1 / ScaleFactor) * (Screen.Bounds.Height / 2) - ActualHeight / 2;
    }

    private void SnapToEdge()
    {
        Screen = Screen.FromHandle(new WindowInteropHelper(this).Handle);
        Dpi = Display(Screen, DpiType.Effective);
        ScaleFactor = Dpi.X / 96.0;
        var scaledLeft = Left * ScaleFactor;
        var scaledRight = (Left + stkContent.ActualWidth) * ScaleFactor;
        var scaledMidPoint = (Left + (stkContent.ActualWidth / 2)) * ScaleFactor;

        var inverseScaleFactor = 1 / ScaleFactor;
        var screenLeft = Screen.Bounds.X;
        var screenRight = screenLeft;
        screenRight += Screen.Bounds.Width;
        var MidScreen = screenRight - Screen.Bounds.Width / 2;
        if (scaledLeft != Margin + screenLeft && scaledRight != screenRight - Margin)
        {
            if (scaledMidPoint > MidScreen)
                AnimateMovement(Left, inverseScaleFactor * (screenRight - Margin) - stkContent.ActualWidth);
            else
                AnimateMovement(Left, Margin + screenLeft * inverseScaleFactor);

            myStoryboard.Begin();
        }
    }

    private void AnimateMovement(double from, double to)
    {
        var snapAnimation = new DoubleAnimation
        {
            From = from,
            To = to,
            Duration = new Duration(TimeSpan.FromMilliseconds(100))
        };

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
        snapAnimation.Completed += delegate (object sender, EventArgs e)
        {
            BeginAnimation(LeftProperty, null);
        };
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

        myStoryboard = new Storyboard();
        myStoryboard.Children.Add(snapAnimation);
        Storyboard.SetTarget(snapAnimation, this);
        Storyboard.SetTargetProperty(snapAnimation, new PropertyPath(LeftProperty));
    }

    private Point Display(Screen screen, DpiType type)
    {
        
            uint x, y;
            screen.GetDpi(type, out x, out y);
             return new(x, y);
        
    }

    private void grdGrabber_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void grdGrabber_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        SnapToEdge();
    }
    private void btnLinks_Click(object sender, RoutedEventArgs e)
    {
        OpenPage(new Links(), typeof(Links));

    }

    private void OpenPage(System.Windows.Controls.UserControl page, Type type)
    {
        if (stkContent.Children.Count > 1)
        {
            var contentType = stkContent.Children[0].GetType();
            stkContent.Children.RemoveAt(0);

            if (contentType != type)
                stkContent.Children.Insert(0, page);

        }
        else
            stkContent.Children.Insert(0, new Links());
        UpdateLayout();

    }

    private void BubblesWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (e.PreviousSize != new Size())
            SnapToEdge();
    }

}
public static class ScreenExtensions
{
    public static void GetDpi(this Screen screen, DpiType dpiType, out uint dpiX, out uint dpiY)
    {
        var pnt = new System.Drawing.Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
        var mon = MonitorFromPoint(pnt, 2/*MONITOR_DEFAULTTONEAREST*/);
        GetDpiForMonitor(mon, dpiType, out dpiX, out dpiY);
    }

    //https://msdn.microsoft.com/en-us/library/windows/desktop/dd145062(v=vs.85).aspx
    [DllImport("User32.dll")]
    private static extern IntPtr MonitorFromPoint([In] System.Drawing.Point pt, [In] uint dwFlags);

    //https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510(v=vs.85).aspx
    [DllImport("Shcore.dll")]
    private static extern IntPtr GetDpiForMonitor([In] IntPtr hmonitor, [In] DpiType dpiType, [Out] out uint dpiX, [Out] out uint dpiY);
}

//https://msdn.microsoft.com/en-us/library/windows/desktop/dn280511(v=vs.85).aspx
public enum DpiType
{
    Effective = 0,
    Angular = 1,
    Raw = 2,
}