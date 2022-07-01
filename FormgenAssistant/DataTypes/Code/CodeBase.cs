using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Printing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.VisualBasic.CompilerServices;

namespace FormgenAssistant.DataTypes.Code
{
    public abstract class CodeBase 
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Prefix { get; set; }

        private List<object> Inputs { get; } = new();
        public List<string> InputDescriptions { get; set; } = new();

        public bool HasNoInputs() => Inputs.Count == 0;

        public CodeBase AddInput(string input, int idx = -1)
        {
            if (idx == -1)
                Inputs.Add(input);
            else
                Inputs[idx] = input;

            return this;
        }

        public CodeBase AddInput(CodeBase input, int idx = -1)
        {
            if (idx == -1)
                Inputs.Add(input);
            else
                Inputs[idx] = input;

            return this;
        }
        public CodeBase AddInput(object input, int idx = -1)
        {
            if (idx == -1)
                Inputs.Add(input);
            else
                Inputs[idx] = input;

            return this;
        }

        public CodeBase SetInputs(List<string> inputs)
        {
            Inputs.Clear();

            foreach (var input in inputs)
                AddInput(input);

            return this;
        }

        public CodeBase SetInputs(List<object> inputs)
        {
            Inputs.Clear();

            foreach (var input in inputs)
                AddInput(input);

            return this;
        }

        public List<object> GetInputs() => Inputs;

        public int InputCount() => Inputs.Count;


        public virtual object GetInput(int idx) => Inputs[idx];

        public virtual string GetCode()
        {
            if (HasNoInputs()) return string.Empty;

            StringBuilder output = new();
            output.Append(Prefix);
            output.Append("( ");

            var step = 0;

            foreach (var input in Inputs)
            {
                output.Append(
                    input switch
                    {
                        string s =>
                            s,

                        CodeBase @base =>
                            @base.GetCode(),

                        _ => null
                    });

                if (step != Inputs.Count - 1)
                    output.Append(", ");
                step++;
            }

            output.Append(" )");
            return output.ToString();
        }

        public static implicit operator string(CodeBase @base) => @base.GetCode();
        public static string operator +(CodeBase a, CodeBase b) => a.GetCode() + b.GetCode();
        public static string operator +(string a, CodeBase b) => a + b.GetCode();
        public static string operator +(CodeBase a, string b) => a.GetCode() + b;
    }
}
