using FormgenAssistant.Pages;
using FormgenAssistant.SavedItems;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Velopack;
using Velopack.Sources;

namespace FormgenAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<UserControl> _pages = [];
        public static UserControl NotesPage { get; private set; }

        public MainWindow()
        {
            VelopackApp.Build()
        .WithFirstRun(v => MessageBox.Show("Thanks for installing my application!"))
        .Run();

            SavedItems.Settings.Load();
			SavedItems.Templates.Templates.Load();
            InitializeComponent();
			_pages.Add(HomePage);
			_pages.Add(FileNameGen);
			_pages.Add(Settings);
			_pages.Add(Notes);
			_pages.Add(Prompts);
			_pages.Add(CodeSnippets);
			_pages.Add(Templates);
            NotesPage = Notes;

            _ = UpdateMyApp();

        }
        private static async Task UpdateMyApp()
        {
            var mgr = new UpdateManager(new GithubSource("https://github.com/TheHeartOfFire/NewFormgenAssistant", null, false));

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
                return; // no update available

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }

        private void btnHomePage_Click(object sender, RoutedEventArgs e)
		{
			HomePage.txtAddress.Text = SavedItems.Settings.Instance.MailingAddress;
			OpenPage(HomePage);
		}

		private void CloseAllPages() => _pages.ForEach(Page => Page.Visibility = Visibility.Hidden);
		private void OpenPage(UserControl? page)
		{
			if (page is null) return;

            CloseAllPages();
			page.Visibility = Visibility.Visible;
        }

		private void btnFileNameGen_Click(object sender, RoutedEventArgs e) =>  OpenPage(FileNameGen);
        private void btnSettings_Click(object sender, RoutedEventArgs e) => OpenPage(Settings);
        private void btnNotes_Click(object sender, RoutedEventArgs e) => OpenPage(Notes);
        private void btnPrompts_Click(object sender, RoutedEventArgs e) => OpenPage(Prompts);
        private void btnSnippets_Click(object sender, RoutedEventArgs e) => OpenPage(CodeSnippets);
        private void btnTemplates_Click(object sender, RoutedEventArgs e) => OpenPage(Templates);

        #region Window Handling for custom Close/Minimize/Maximize buttons


        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RefreshMaximizeRestoreButton()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.maximizeButton.Visibility = Visibility.Collapsed;
                this.restoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.maximizeButton.Visibility = Visibility.Visible;
                this.restoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            RefreshMaximizeRestoreButton();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            ((HwndSource)PresentationSource.FromVisual(this)).AddHook(HookProc);
        }
        public static IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_GETMINMAXINFO)
            {
                // We need to tell the system what our size should be when maximized. Otherwise it will cover the whole screen,
                // including the task bar.
                MINMAXINFO mmi = (MINMAXINFO)(Marshal.PtrToStructure(lParam, typeof(MINMAXINFO)) ?? throw new InvalidOperationException());

                // Adjust the maximized size and position to fit the work area of the correct monitor
                IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

                if (monitor != IntPtr.Zero)
                {
                    MONITORINFO monitorInfo = new()
                    {
                        cbSize = Marshal.SizeOf(typeof(MONITORINFO))
                    };
                    GetMonitorInfo(monitor, ref monitorInfo);
                    var rcWorkArea = monitorInfo.rcWork;
                    var rcMonitorArea = monitorInfo.rcMonitor;
                    mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                    mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                    mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                    mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
                }

                Marshal.StructureToPtr(mmi, lParam, true);
            }

            return IntPtr.Zero;
        }
        private const int WM_GETMINMAXINFO = 0x0024;

        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr handle, uint flags);

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }
    }
    #endregion
}
