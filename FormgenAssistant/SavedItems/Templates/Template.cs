using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.SavedItems.Templates;
public class Template(string name, string text, ushort variables, List<string> defaults) : IEquatable<Template>, IComparable<Template>
{
    public string Name { get; set; } = name;
    public string Text { get; set; } = text;
    public ushort Variables { get; set; } = variables;
    public List<string> VariableDefaults { get; set; } = defaults;
    public TemplateType Type { get; set; } = TemplateType.Other;

    public int CompareTo(Template? other)
    {
        switch (Type)
        {
            case TemplateType.PublishComments:
                return other is not null ? other.Type switch
                {
                    TemplateType.PublishComments => Name.CompareTo(other.Name),
                    TemplateType.InternalComments => -1,
                    TemplateType.ClosureComments => -1,
                    TemplateType.Other => -1,
                    _ => 1
                } : 1;
            case TemplateType.InternalComments:
                return other is not null ? other.Type switch
                {
                    TemplateType.PublishComments => 1,
                    TemplateType.InternalComments => Name.CompareTo(other.Name),
                    TemplateType.ClosureComments => -1,
                    TemplateType.Other => -1,
                    _ => 1
                } : 1;
            case TemplateType.ClosureComments:
                return other is not null ? other.Type switch
                {
                    TemplateType.PublishComments => 1,
                    TemplateType.InternalComments => 1,
                    TemplateType.ClosureComments => Name.CompareTo(other.Name),
                    TemplateType.Other => -1,
                    _ => 1
                } : 1;
            default:
                return other is not null ? Name.CompareTo(other.Name) : 1;
        }
    }

    public bool Equals(Template? other)
    {
        return other is not null &&
               GetHashCode() == other.GetHashCode();
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Text, Variables, VariableDefaults, Type);
    }

    public enum TemplateType
    {
        PublishComments,
        InternalComments,
        ClosureComments,
        Other
    }
}
