using FormgenAssistant.DataTypes.Code;

namespace FormgenAssistant.Interfaces
{
    public interface IExtendableCode
    {
        int DefaultArgCount { get; }
        int ArgIncriment { get; }
        CodeBase AddExtraInputs(int count);
        CodeBase RemoveExtraInputs(int count);
    }
}
