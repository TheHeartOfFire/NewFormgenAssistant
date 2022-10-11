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

namespace FormgenAssistant.Pages;
/// <summary>
/// Interaction logic for MultiNotes.xaml
/// </summary>
public partial class MultiNotes : UserControl
{
    public MultiNotes()
    {
        InitializeComponent();
    }
    //https://stackoverflow.com/questions/62463491/how-to-make-a-quick-window-to-rename-tab
    //public partial class TabEditWindow : Window
    //{
    //    public TabEditWindow(TabControl tabs)
    //    {
    //        this.InitializeComponent();
    //        this.tabs = tabs;
    //        base.Topmost = true;
    //    }


    //    public void Show(TabItem tab)
    //    {
    //        this.tab = tab;
    //        base.Show();
    //    }

    //    private void RenameTab(object sender, RoutedEventArgs e)
    //    {
    //        base.Hide();
    //        TextBox textBox = this.tab.Header as TextBox;
    //        textBox.IsEnabled = true;
    //        textBox.Focus();
    //        textBox.SelectAll();
    //    }

    //    private void CloseTab(object sender, RoutedEventArgs e)
    //    {
    //        base.Hide();
    //        this.tabs.Items.Remove(this.tab);
    //    }

    //    private TabControl tabs;
    //    private TabItem tab;
    //}
}
