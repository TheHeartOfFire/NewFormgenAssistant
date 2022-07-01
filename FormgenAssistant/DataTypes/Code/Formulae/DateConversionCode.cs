using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistant.DataTypes.Code.Functions;

namespace FormgenAssistant.DataTypes.Code.Formulae
{
    public class DateConversionCode : CodeBase
    {
        public DateConversionCode()
        {
            Name = "Date Conversion";
            Description = "Converts a date in the format of MM/DD/YYYY to a date in the format of MM/DD/YY.";
            InputDescriptions = new List<string>()
            {
                "Date Field"
            };
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;
            
            return new SeplistCode()
                .AddExtraInputs(1)
                .AddInput(new MonthCode()
                    .AddInput(GetInput(0) as string ?? string.Empty))
                .AddInput(new DayCode()
                    .AddInput(GetInput(0) as string ?? string.Empty))
                .AddInput(new YearCode()
                    .AddInput(GetInput(0) as string ?? string.Empty) + " % 100");
        }
    }
}
