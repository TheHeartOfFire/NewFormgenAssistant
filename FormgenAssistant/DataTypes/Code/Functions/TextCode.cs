namespace FormgenAssistant.DataTypes.Code.Functions
{
    public class TextCode : CodeBase
    {
        public TextCode()
        {
            Name = "Text";
            Prefix = "TEXT";
            Description = "Convert a Date or Numeric field to a Text field";
            AddInput("Value");
        }
    }
}
