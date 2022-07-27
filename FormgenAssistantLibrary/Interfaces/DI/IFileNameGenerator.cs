namespace FormgenAssistantLibrary.Interfaces.DI;

public interface IFileNameGenerator
{
    List<string> StateCodes { get; }

    string LaserLaw(string bank, string formName, string code, string date, string dealer,
        bool formNameTitle, bool? newUsed, bool codeCaps, bool custom);

    string Laser(string state, string bank, bool formNameTitle, string formName, string code, string date,
        string dealer, bool codeCaps, bool? newUsed, bool custom, bool vm);

    string ImpactLaw(string code, bool codeCaps, string date, string bank, bool formNameTitle, string formName,
        string dealer, bool? newUsed, bool custom, bool vm);

    string Impact(string state, string bank, bool formNameTitle, string formName, string code, string date,
        string dealer, bool codeCaps, bool? newUsed, bool custom, bool vm);
}