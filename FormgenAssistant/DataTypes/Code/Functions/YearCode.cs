namespace FormgenAssistant.DataTypes.Code.Functions
{
    public class YearCode : CodeBase
    {
        public YearCode()
        {
            Name = "Year";
            Prefix = "YEAR";
            Description = "Extract the numeric year from a date";
            AddInput("Date Field");
        }
    }
}
