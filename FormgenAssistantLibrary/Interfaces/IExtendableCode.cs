using FormgenAssistantLibrary.DataTypes.Code;

namespace FormgenAssistantLibrary.Interfaces
{
    public interface IExtendableCode
    {
        int DefaultArgCount { get; }
        int ArgIncrement { get; }
        CodeBase AddExtraInputs(int count);
        CodeBase RemoveExtraInputs(int count);
    }
}
