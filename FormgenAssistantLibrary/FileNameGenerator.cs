using FormgenAssistantLibrary.Interfaces.DI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistantLibrary;

public class FileNameGenerator : IFileNameGenerator
{
    public List<string> StateCodes { get; } = new()
    {
        "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA",
        "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK",
        "OR", "PA", "PR", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "VI", "WA", "WV", "WI", "WY"
    }; 


    public string LaserLaw(string bank, string formName, string code, string date, string dealer, 
        bool formNameTitle, bool? newUsed, bool codeCaps, bool custom)
    {
        var result = "LAW ";
        result += bank != "" ? bank + " " : "";
        result += formNameTitle == true ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(formName ?? string.Empty) : formName;
        result += (code != "" || date != "") ? " [LAW " : "";
        result += code != "" ? codeCaps == true ? code?.ToUpperInvariant() : code + " " : "";
        result += date != "" ? date + " " : "";
        result += (code != "" || date != "") ? "]" : "";
        result += dealer != "" ? " (" + dealer + ")" : "";
        result += newUsed switch
        {
            false => " (SOLD)",
            true => " (TRADE)",
            _ => ""
        };
        result += custom == true ? " - Custom" : "";
        return result.Replace('/', '-');
    }

    public string Laser(string state, string bank, bool formNameTitle, string formName, string code, string date,
        string dealer, bool codeCaps, bool? newUsed, bool custom, bool vm)
    {
        var result = "";
        result += state != "" ? state + " " : "";
        result += bank != "" ? bank + " " : "";
        result += formNameTitle == true ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(formName ?? string.Empty) : formName;

        result += (code != "" || date != "" || dealer != "") ? " [" : "";
        result += code != "" ? codeCaps == true ? code.ToUpperInvariant() : code + " " : "";
        result += ((code != "" || dealer != "") && date != "") ? " (" : "";
        result += date != "" ? date : "";
        result += ((code != "" || dealer != "") && date != "") ? ")" : "";
        result += dealer != "" ? "(" + dealer + ")" : "";
        result += (code != "" || date != "" || dealer != "") ? "]" : "";

        result += newUsed switch
        {
            false => " (SOLD)",
            true => " (TRADE)",
            _ => ""
        };
        result += custom == true ? " - Custom" : "";
        result += vm == true ? " - VM" : "";
        return result.Replace('/', '-');
    }

    public string ImpactLaw(string code, bool codeCaps, string date, string bank, bool formNameTitle, string formName,
        string dealer, bool? newUsed, bool custom, bool vm)
    {
        var result = "LAW ";
        result += code != "" ? codeCaps == true ? code.ToUpperInvariant() : code + " " : "";
        result += date != "" ? date + " " : "";
        result += bank != "" ? bank + " " : "";
        result += formNameTitle == true ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(formName) : formName;
        result += dealer != "" ? "(" + dealer + ")" : "";
        result += newUsed switch
        {
            false => "(SOLD)",
            true => "(TRADE)",
            _ => ""
        };
        result += custom == true ? " - Custom" : "";
        result += vm == true ? " - VM" : "";
        return result.Replace('/', '-');
    }

    public string Impact(string state, string bank, bool formNameTitle, string formName, string code, string date,
        string dealer, bool codeCaps, bool? newUsed, bool custom, bool vm)
    {
        var result = "";
        result += state != "" ? state + " " : "";
        result += bank != "" ? bank + " " : "";
        result += formNameTitle == true ? CultureInfo.InvariantCulture.TextInfo.ToTitleCase(formName) : formName;

        result += (code != "" || date != "" || dealer != "") ? " (" : "";
        result += code != "" ? codeCaps == true ? code.ToUpperInvariant() : code + " " : "";
        result += ((code != "" || dealer != "") && date != "") ? " [" : "";
        result += date != "" ? date : "";
        result += ((code != "" || dealer != "") && date != "") ? "]" : "";
        result += dealer != "" ? "[" + dealer + "]" : "";
        result += (code != "" || date != "" || dealer != "") ? ")" : "";

        result += newUsed switch
        {
            false => " (SOLD)",
            true => " (TRADE)",
            _ => ""
        };
        result += custom == true ? " - Custom" : "";
        result += vm == true ? " - VM" : "";
        return result.Replace('/', '-');
    }
}