using FormgenAssistant.Controls;
using FormgenAssistant.Dialogues;
using FormgenAssistant.SavedItems.Templates;
using Microsoft.Win32;
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
using TemplateList = FormgenAssistant.SavedItems.Templates.Templates;

namespace FormgenAssistant.Pages;
/// <summary>
/// Interaction logic for Templates.xaml
/// </summary>
public partial class Templates : UserControl
{
    public Templates()
    {
        InitializeComponent();
        RefreshTemplates();
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        var dialogue = new NewTemplateDialogue();
        var success = dialogue.ShowDialog();

        if (success == true && dialogue.NewTemplate is not null)
        {
            TemplateList.Instance.TemplateList.Add(dialogue.NewTemplate);
            TemplateList.Save();
            RefreshTemplates();
            if(SavedItems.Settings.Instance.SelectNewTemplate)
                lBoxTemplateList.SelectedItem = dialogue.NewTemplate.Name;
            UpdateDisplay();
        }
    }

    private void btnRemove_Click(object sender, RoutedEventArgs e)
    {
        var selected = lBoxTemplateList.SelectedIndex;
        TemplateList.Instance.TemplateList.RemoveAt(selected);
        TemplateList.Save();
        RefreshTemplates();
        txtDisplay.Text = string.Empty;
        stkVariables.Children.Clear();

    }

    private void lBoxTemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lBoxTemplateList.SelectedIndex < 0) return;

        var item = TemplateList.Instance.TemplateList[lBoxTemplateList.SelectedIndex];

        stkVariables.Children.Clear();

        for (int i = 0; i < item.Variables; i++)
        {
            stkVariables.Children.Add(CreateVariableTextBox());
        }
        txtDisplay.Text = item.Text; 
        UpdateDisplay();
    }

    private UIElement CreateVariableTextBox()
    {
        var box = new NewTextBox();
        box.Width = 120;
        box.OnTextChanged += Box_OnTextChanged;
        return box;
    }

    private void Box_OnTextChanged(object? sender, EventArgs e)
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var selectedIndex = lBoxTemplateList.SelectedIndex;
        if (selectedIndex < 0) return;

        var variables = new List<string>();

        foreach (NewTextBox item in stkVariables.Children) variables.Add(item.Text);

        txtDisplay.Text = string.Format(TemplateList.Instance.TemplateList[selectedIndex].Text, variables.ToArray());
    }


    private void RefreshTemplates()
    {
        lBoxTemplateList.Items.Clear();

        foreach(var template in TemplateList.Instance.TemplateList)
        {
            lBoxTemplateList.Items.Add(template.Name);
        }    
    }

    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
        var selectedIndex = lBoxTemplateList.SelectedIndex;
        if (selectedIndex < 0) return;

        var item = TemplateList.Instance.TemplateList[selectedIndex];
        var dialogue = new NewTemplateDialogue(item);
        var success = dialogue.ShowDialog();

        if (success == true && dialogue.NewTemplate is not null)
        {
            TemplateList.Instance.TemplateList[selectedIndex] = dialogue.NewTemplate;
            TemplateList.Save();
            RefreshTemplates();
            lBoxTemplateList.SelectedItem = dialogue.NewTemplate.Name;
        }
        UpdateDisplay();
    }

    private void btnCopyTemplate_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(txtDisplay.Text);
    }



}
