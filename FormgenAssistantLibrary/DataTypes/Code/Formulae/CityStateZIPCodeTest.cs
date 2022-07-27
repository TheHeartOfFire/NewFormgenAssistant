using FormgenAssistantLibrary.DataTypes.Code.Functions;

namespace FormgenAssistantLibrary.DataTypes.Code.Formulae;

public class CityStateZIPCodeTest : CodeBase
{
    public CityStateZIPCodeTest()
    {
        Name = "City, State ZIP";
        Prefix = "CSZ";
        Description = "Nicely format a city, state, and ZIP code.";
        AddInput(new SeplistCode()
            .SetInputValue(0, "\' \'")
            .SetInputValue(1, new SeplistCode()
                .SetInputValue(0, "\', \'")
                .SetInputValue(1, "City")
                .SetInputValue(2, "State"))
            .SetInputValue(2, "ZIP"), new SeplistCode().Description!);
    }

    public override string GetCode()
    {
        if (HasNoInputs()) return string.Empty;

        return new SeplistCode()
            .SetInputValue(0, "\' \'")
            .SetInputValue(1, new SeplistCode()
                .SetInputValue(0, "\', \'")
                .SetInputValue(1, GetInput(0) as string ?? string.Empty)
                .SetInputValue(2, GetInput(1) as string ?? string.Empty).GetCode())
            .SetInputValue(2, GetInput(2) as string ?? string.Empty).GetCode();
    }
}