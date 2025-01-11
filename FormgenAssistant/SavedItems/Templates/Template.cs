using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.SavedItems.Templates;
public class Template(string name, string text, ushort variables, List<string> defaults)
{
    public string Name { get; set; } = name;
    public string Text { get; set; } = text;
    public ushort Variables { get; set; } = variables;
    public List<string> VariableDefaults { get; set; } = defaults;
}
