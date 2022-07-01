using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.DataTypes.Code.Functions
{
    public class RoundCode : CodeBase
    {
        public RoundCode()
        {
            Name = "Round";
            Prefix = "ROUND";
            Description = "Round a number to a specified number of decimal places. Uses nearest rounding. i.e. 0.5 rounds to 1.0, 0.4 rounds to 0.0";
            InputDescriptions = new List<string>()
            {
                "Number",
                "Decimal Places"
            };
        }
    }
}
