using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistant.DataTypes.Code.Functions;

namespace FormgenAssistant.DataTypes.Code.Formulae
{
    public class FuelDropdownDefaultCode : CodeBase
    {
        public FuelDropdownDefaultCode()
        {
            Name = "Fuel Dropdown Default";
            Description = "This code goes in the Init screen. " +
                          "This serves as the default value for a dropdown named \"FUELTYPE\" with \"Gas\", \"Diesel\", \"Propane\", \"Hybrid\", \"Electric\", and \"Other\" as its options.";
        }

        public override string GetCode()
        {
             return "FUELTYPE = " + new IfCode()
                .AddInput(new ContainsCode()
                    .AddInput("F776")
                    .AddInput("'GAS'"))
                .AddInput("'Gas'")
                .AddInput(new IfCode()
                    .AddInput(new ContainsCode()
                        .AddInput("F776")
                        .AddInput("'DIESEL'"))
                    .AddInput("'Diesel'")
                    .AddInput( new IfCode()
                        .AddInput(new ContainsCode()
                            .AddInput("F776")
                            .AddInput("'PROPANE'"))
                        .AddInput("'Propane'")
                        .AddInput( new IfCode()
                            .AddInput(new ContainsCode()
                                .AddInput("F776")
                                .AddInput("'HYBRID'"))
                            .AddInput("'Hybrid'")
                            .AddInput(new IfCode()
                                .AddInput(new ContainsCode()
                                    .AddInput("F776")
                                    .AddInput("'ELECTRIC'"))
                                .AddInput("'Electric'")
                                .AddInput(new IfCode()
                                    .AddInput(new ContainsCode()
                                        .AddInput("F776")
                                        .AddInput("'OTHER'"))
                                    .AddInput("'Other'")
                                    .AddInput("' '"))))));
        }
    }
}
