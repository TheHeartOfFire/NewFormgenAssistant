using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.DataTypes.Code.Functions
{
    internal class DayCode : CodeBase
    {
        public DayCode()
        {
            Name = "Day";
            Prefix = "DAY";
            Description = "Extract the numeric day from a date";
            InputDescriptions = new List<string>()
            {
                "Date Field"
            };
        }
    }
}
