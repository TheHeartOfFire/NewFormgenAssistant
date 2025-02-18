using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistantLibrary.DataTypes.Code;
using FormgenAssistantLibrary.DataTypes.Code.Formulae;
using FormgenAssistantLibrary.DataTypes.Code.Functions;
using FormgenAssistantLibrary.Interfaces.DI;

namespace FormgenAssistantLibrary
{
    public class CodeSnippetGenerator : ICodeSnippetGenerator
    {
        public List<CodeBase> Snippets { get; } = new();

        public CodeSnippetGenerator()
        {
            Snippets.Add(new CityStateZIPCode());
            Snippets.Add(new DateConversionCode());
            Snippets.Add(new DayAndSuffixCode());
            Snippets.Add(new MonthNameCode());
            Snippets.Add(new NumToTextCode());
            Snippets.Add(new SeplistNumber());
            Snippets.Add(new DmvCalculationCode());
            Snippets.Add(new FuelDropdownDefaultCode());
            Snippets.Add(new CaseCode());
            Snippets.Add(new IfCode());
            Snippets.Add(new SeplistCode());
        }
    }
}
