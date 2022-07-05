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
            AddInput("Separator");
            AddInput("Item");
            AddInput("Item");

            DefaultArgCount = 3;
            ArgIncriment = 1;
            
        }
        public CodeBase AddExtraInputs(int count)
        {
            for (var i = 0; i < count; i++)
                AddInput("Item");
            
            return this;
        }

        public CodeBase RemoveExtraInputs(int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (InputCount() <= DefaultArgCount) return this;

                RemoveInput(InputCount() - 1);
            }
            return this;
        }
    }
}
