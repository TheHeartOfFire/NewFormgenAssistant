using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.DataTypes.Code.Functions
{
    public class YearCode : CodeBase
    {
        public YearCode()
        {
            Name = "Year";
            Prefix = "YEAR";
            Description = "Extract the numeric year from a date";
            InputDescriptions = new List<string>()
            {
                "Date Field"
            };
        }
    }
}
