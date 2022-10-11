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
        CreateTab("Item1");
        CreateTab("Item2");
        CreateTab("Item3");
    }
    
    public void CreateTab(string Name)
    {
        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/035f7878-c577-486d-8561-3d193bb21fbe/ho-to-allow-the-user-to-edit-the-tab-header-?forum=wpf
        // Create DataTemplate
        DataTemplate tabItemTemplate = new DataTemplate();
        tabItemTemplate.DataType = typeof(TabItem);

        // Create TextBox
        FrameworkElementFactory textBoxFactory = new FrameworkElementFactory(typeof(TextBox));
        textBoxFactory.SetBinding(TextBox.TextProperty, new Binding("."));
        textBoxFactory.SetValue(TextBox.BackgroundProperty, new SolidColorBrush(Color.FromArgb(255,15,11,30)));
        var fore = System.Drawing.Color.MediumSlateBlue;
        textBoxFactory.SetValue(TextBox.ForegroundProperty, new SolidColorBrush(Color.FromArgb(fore.A, fore.R, fore.G, fore.B)));
        textBoxFactory.SetValue(TextBox.BorderBrushProperty, new SolidColorBrush(Color.FromArgb(255, 15, 11, 30)));

        var focusedStyle = new Style(typeof(TextBox));
        var focusedBorder = new Setter();
        focusedBorder.Property = TextBox.BorderBrushProperty;
        focusedBorder.Value = new SolidColorBrush(Color.FromArgb(255, 15, 11, 30));
        focusedStyle.Setters.Add(focusedBorder);

        textBoxFactory.SetValue(TextBox.FocusVisualStyleProperty, focusedStyle);
        
        tabItemTemplate.VisualTree = textBoxFactory;

        // Create the TabItem
        TabItem item1 = new TabItem();
        item1.HeaderTemplate = tabItemTemplate;
        item1.Header = Name;
        item1.Content = new Notes();
        item1.BorderBrush = new SolidColorBrush(Color.FromArgb(fore.A, fore.R, fore.G, fore.B));
        item1.Background = new SolidColorBrush(Color.FromArgb(255, 15, 11, 30));

        // Add the TabItem to the TabControl
        tcContent.Items.Add(item1);
    }
}
