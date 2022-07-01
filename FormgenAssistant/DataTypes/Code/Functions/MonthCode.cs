using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.DataTypes.Code.Functions
{
    public class MonthCode : CodeBase
    {
        public MonthCode()
        {
            Name = "Month";
            Prefix = "MONTH";
            Description = "Extract the numeric month from a date";
            InputDescriptions = new List<string>()
            {
                "Date Field"
            };
        }
    }
}
