using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistant.Interfaces;

namespace FormgenAssistant.DataTypes.Code.Functions
{
    public class SeplistCode : CodeBase, IExtendableCode
    {
        public int DefaultArgCount { get; }
        public int ArgIncriment { get; }

        public SeplistCode()
        {
            Name = "Seplist";
            Prefix = "SEPLIST";
            Description = "Separate a list of items with the given separator.";
            InputDescriptions = new List<string>()
            {
                "Separator",
                "Item",
                "Item"
            };
            DefaultArgCount = 3;
            ArgIncriment = 1;
        }
        public CodeBase AddExtraInputs(int count)
        {
            for (var i = 0; i < count; i++)
            {
                InputDescriptions.Add( "Item");
            }
            return this;
        }

        public CodeBase RemoveExtraInputs(int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (InputDescriptions.Count <= DefaultArgCount) return this;

                InputDescriptions.RemoveAt(InputDescriptions.Count - 1);
            }
            return this;
        }
    }
}
