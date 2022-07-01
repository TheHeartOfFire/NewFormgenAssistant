using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistant.Interfaces;

namespace FormgenAssistant.DataTypes.Code.Functions
{
    public class CaseCode : CodeBase, IExtendableCode
    {
        public int DefaultArgCount { get; }
        public int ArgIncriment { get; }

        public CaseCode()
        {
            Name = "Case Statement";
            Prefix = "CASE";
            Description = "Compare the comparison value to the case values, " +
                          "return the corresponding result of any matches, " +
                          "otherwise return the default value. " +
                          "Identical to multiple nested If Statements.";
            InputDescriptions = new List<string>()
            {
                "Comparison",
                "Case",
                "Result",
                "Case",
                "Result",
                "Default"
            };
            DefaultArgCount = 6;
            ArgIncriment = 2;
        }


        public CodeBase AddExtraInputs(int count)
        {
            for (var i = 0; i < count; i++)
            {
                InputDescriptions.Insert(InputDescriptions.Count - 1, "Case");
                InputDescriptions.Insert(InputDescriptions.Count - 1, "Result");
            }

            return this;
        }

        public CodeBase RemoveExtraInputs(int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (InputDescriptions.Count <= DefaultArgCount) return this;
                
                InputDescriptions.RemoveAt(InputDescriptions.Count - 2);
                InputDescriptions.RemoveAt(InputDescriptions.Count - 2);
            }

            return this;
        }
    }
}
