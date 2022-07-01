using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistant.DataTypes.Code.Functions;

namespace FormgenAssistant.DataTypes.Code.Formulae
{
    public class NumToTextCode : CodeBase
    {
        public NumToTextCode()
        {
            Name = "Number to Text";
            Description = "Converts a number to text. " +
                          "This is needed because just using the NUM() function produces strange results in FormGen. " +
                          "Can also be used to left justify numbers";
            InputDescriptions = new List<string>()
            {
                "Number",
                "Decimal Places"
            };
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;
            
            return new TextCode()
                .AddInput(new RoundCode()
                    .AddInput(GetInput(0) as string ?? string.Empty));
        }
    }
}
