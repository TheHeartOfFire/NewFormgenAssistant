using FormgenAssistant.SavedItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FormgenAssistant
{
    public static class Utils
    {
        public static TabItem Clone(this TabItem? original) => original is not null ? new()
        {
            Header = "No Case#",
            Style = original.Style,
            Background = original.Background,
            Foreground = original.Foreground,
            BorderBrush = original.BorderBrush,
            ContextMenu = original.ContextMenu
        } : new TabItem();

        public static bool Equals(this TabItem original, TabItem? other) => 
            original is not null && 
            other is not null && 
            original.GetHashCode() == other.GetHashCode();
    }
}
