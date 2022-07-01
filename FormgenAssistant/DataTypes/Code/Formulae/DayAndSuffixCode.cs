using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistant.DataTypes.Code.Functions;

namespace FormgenAssistant.DataTypes.Code.Formulae
{
    public class DayAndSuffixCode : CodeBase
    {
        public DayAndSuffixCode()
        {
            Name = "Day and Suffix";
            Description = "Returns the day of the month with the suffix. 11 = 11th, 12 = 12th, 13 = 13th, 21 = 21st, 22 = 22nd, etc.";
            InputDescriptions = new List<string>()
            {
                "Numeric Day"
            };
        }
        
        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;
            
            return new NumToTextCode()
                .AddInput(GetInput(0) as string ?? string.Empty)
                .AddInput("0") + 
                   " + " + 
                   new CaseCode()
                .AddExtraInputs(1)
                .AddInput(GetInput(0) as string ?? string.Empty)
                .AddInput("11")
                .AddInput("\'th\'")
                .AddInput("12")
                .AddInput("\'th\'")
                .AddInput("13")
                .AddInput("\'th\'")
                .AddInput(new CaseCode()
                    .AddExtraInputs(1)
                    .AddInput($"{GetInput(0) as string ?? string.Empty} % 10")
                    .AddInput("1")
                    .AddInput("\'st\'")
                    .AddInput("2")
                    .AddInput("\'nd\'")
                    .AddInput("3")
                    .AddInput("\'rd\'")
                    .AddInput("\'th\'")); 
        }
    }
}
