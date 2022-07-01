using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistant.DataTypes.Code.Functions;

namespace FormgenAssistant.DataTypes.Code.Formulae
{
    public class CityStateZIPCode : CodeBase
    {
        public CityStateZIPCode()
        {
            Name = "City, State ZIP";
            Description = "Nicely format a city, state, and ZIP code.";
            InputDescriptions = new List<string>()
            {
                "City",
                "State",
                "ZIP"
            };
        }

        public override string GetCode()
        {
            if (HasNoInputs()) return string.Empty;

            return new SeplistCode()
                .AddInput("\' \'")
                .AddInput(new CaseCode()
                    .AddInput("\', \'")
                    .AddInput(GetInput(0) as string ?? string.Empty)
                    .AddInput(GetInput(1) as string ?? string.Empty))
                .AddInput(GetInput(2) as string ?? string.Empty);
        }
    }
}
