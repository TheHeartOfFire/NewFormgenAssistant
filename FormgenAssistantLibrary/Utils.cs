using FormgenAssistantLibrary.Interfaces.DI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistantLibrary;

public class Utils : IUtils
{

    public void OpenLink(string path) =>
        Process.Start(new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        });
}
