using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormgenAssistant.DataTypes.Code.Functions
{
    public class TextCode : CodeBase
    {
        public TextCode()
        {
            Name = "Text";
            Prefix = "TEXT";
            Description = "Convert a Date or Numeric field to a Text field";
            InputDescriptions = new List<string>()
            {
                "Input"
            };
        }
    }
}
