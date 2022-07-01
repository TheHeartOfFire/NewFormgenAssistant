using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.DataTypes.Code.Functions
{
    public class ContainsCode : CodeBase
    {
        public ContainsCode()
        {
            Name = "Contains";
            Prefix = "CONTAINS";
            Description = "Returns TRUE if TextB appears anywhere within TextA";
            InputDescriptions = new List<string>()
            {
                "TextA",
                "TextB"
            };
        }
    }
}
