using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.SavedItems.Templates;
public class Template(string name, string text, ushort variables)
{
    public string Name { get; } = name;
    public string Text { get; } = text;
    public ushort Variables { get; } = variables;
}
