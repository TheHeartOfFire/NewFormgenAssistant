using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistantLibrary.DataTypes;

public class SettingsData
{
    public bool NotesCopyAll { get; set; }
    public string MailingAddress { get; set; }

    public SettingsData(bool notesCopyAll, string mailingAddress)
    {
        MailingAddress = mailingAddress;
        NotesCopyAll = notesCopyAll;
    }
}