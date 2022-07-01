using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistant.DataTypes.Code.Functions;

namespace FormgenAssistant.DataTypes.Code.Formulae
{
    public class SeplistNumber : CodeBase
    {
        public SeplistNumber()
        {

            Name = "Seplist Number";
            Description = "Allow Numeric fields to be used in seplist.";
            InputDescriptions = new List<string>()
            {
                "Numeric Field",
                "Decimal Places"
            };
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;

            return new IfCode()
                .AddInput((GetInput(0) as string ?? string.Empty) + " != 0")
                .AddInput(new RoundCode()
                    .AddInput(GetInput(0) as string ?? string.Empty)
                    .AddInput(GetInput(1) as string ?? string.Empty))
                .AddInput("");
        }
    }
}
