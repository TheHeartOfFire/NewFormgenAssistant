using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.DataTypes
{
    public class CodeSnippet
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public bool IsExpandable { get; set; } = false;
        public string? CodeExpansion { get; set; }
        public int ExpansionIndex { get; set; }
        public List<string>? InputExpansion { get; set; }
        public List<string> Inputs { get; set; }

        public CodeSnippet(string name, string code, List<string> inputs, string description, string? codeExpansion = null, int expansionIndex = 0, List<string>? inputExpansion = null)
        {
            Name = name;
            Code = code;
            Inputs = inputs;
            Description = description;
            CodeExpansion = codeExpansion;
            InputExpansion = inputExpansion;
            IsExpandable = inputExpansion is not null;
            ExpansionIndex = expansionIndex;
        }

        public string GetCode(params object?[] inputs)
        {
            return string.Format(Code, inputs);
        }
    }
}
