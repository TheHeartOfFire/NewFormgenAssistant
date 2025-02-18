using FormgenAssistantLibrary.DataTypes.Code;

namespace FormgenAssistantLibrary.Interfaces.DI;

public interface ICodeSnippetGenerator
{
    List<CodeBase> Snippets { get; }
}