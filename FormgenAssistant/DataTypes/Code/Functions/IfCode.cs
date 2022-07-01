using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.DataTypes.Code.Functions
{
    public class IfCode : CodeBase
    {
        public IfCode()
        {
            Name = "If Statement";
            Prefix = "IF";
            Description = "If the condition is true, return the true value, otherwise return the false value.";
            InputDescriptions = new List<string>()
            {
                "Condition",
                "ResultIfTrue",
                "ResultIfFalse"
            };
        }
}
}
