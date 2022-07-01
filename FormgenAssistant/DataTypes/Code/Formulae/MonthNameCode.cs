using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistant.DataTypes.Code.Functions;

namespace FormgenAssistant.DataTypes.Code.Formulae
{
    public class MonthNameCode : CodeBase
    {
        public MonthNameCode()
        {
            Name = "Month Names";
            Description = "Get the name of the specified month. 1 = January, 2 = February, etc.";
            InputDescriptions = new List<string>()
            {
                "Date Field"
            };
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;
            
            return new CaseCode()
                .AddExtraInputs(10)
                .AddInput(GetInput(0) as string ?? string.Empty)
                .AddInput("1")
                .AddInput("\'JANUARY\'")
                .AddInput("2")
                .AddInput("\'FEBRUARY\'")
                .AddInput("3")
                .AddInput("\'MARCH\'")
                .AddInput("4")
                .AddInput("\'APRIL\'")
                .AddInput("5")
                .AddInput("\'MAY\'")
                .AddInput("6")
                .AddInput("\'JUNE\'")
                .AddInput("7")
                .AddInput("\'JULY\'")
                .AddInput("8")
                .AddInput("\'AUGUST\'")
                .AddInput("9")
                .AddInput("\'SEPTEMBER\'")
                .AddInput("10")
                .AddInput("\'OCTOBER\'")
                .AddInput("11")
                .AddInput("\'NOVEMBER\'")
                .AddInput("12")
                .AddInput("\'DECEMBER\'")
                .AddInput("\'Invalid Month\'");
        }
    }
}
