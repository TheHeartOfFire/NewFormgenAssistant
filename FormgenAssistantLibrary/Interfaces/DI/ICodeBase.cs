using FormgenAssistantLibrary.DataTypes.Code;

namespace FormgenAssistantLibrary.Interfaces.DI;

public interface ICodeBase
{
    string? Name { get; set; }
    string? Description { get; set; }
    string? Prefix { get; set; }
    List<CodeInput> Inputs { get; }
    bool HasNoInputs();
    string GetToken();
    CodeBase AddInput(CodeInput input);
    CodeBase AddInput(string description);
    CodeBase AddInput(int index, string description);
    CodeBase AddInput(string value, string description);
    CodeBase AddInput(CodeBase value, string description);
    CodeBase RemoveInput(int index);
    CodeBase SetInputValue(int index, string value);
    CodeBase SetInputValue(int index, CodeBase value);
    CodeBase SetInputDescription(int index, string value);
    CodeBase SetInputs(List<string> inputs);
    List<CodeInput> GetInputs();
    int InputCount();
    object GetInput(int idx);
    object GetDescription(int idx);
    string GetCode();
}