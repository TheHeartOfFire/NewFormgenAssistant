namespace FormgenAssistant.Pages
{
    public partial class PromptCopier
    {
        public enum PromptType
        {
            OneTwoThree,
            OneTwoThreeFour,
            CheckBox,
            Date,
            Decimal,
            Dropdown,
            Integer,
            Label,
            LabelNumber,
            Money,
            RadioButtons,
            Separator,
            StateCode,
            Text,
            VIN,
            YesNo,
            ZIP5,
            ZIP10
        }

        private string PromptDescriptor(PromptType type) => type switch
            {
                PromptType.OneTwoThree => "123",
                PromptType.OneTwoThreeFour => "1234",
                PromptType.CheckBox => "CHK",
                PromptType.Date => "DATE",
                PromptType.Decimal => "1.0",
                PromptType.Dropdown => "DROP",
                PromptType.Integer => "100",
                PromptType.Label => "LBL",
                PromptType.LabelNumber => "LBL#",
                PromptType.Money => "$1.00",
                PromptType.RadioButtons => "RDO",
                PromptType.Separator => "SEP",
                PromptType.StateCode => "ST",
                PromptType.Text => "ABC",
                PromptType.VIN => "VIN",
                PromptType.YesNo => "Y/N",
                PromptType.ZIP5 => "ZIP5",
                PromptType.ZIP10 => "ZIP10",
                _ => "ABC",
            }; 
        
        private string PromptDescriptor(string type) => type switch
            {
                "BuyerCoBoth" => "123",
                "BuyerCoBothOther" => "1234",
                "Checkbox" => "CHK",
                "Date" => "DATE",
                "Decimal" => "1.0",
                "Dropdown" => "DROP",
                "Integer" => "100",
                "InstructionLine" => "LBL",
                "LabelNumber" => "LBL#",
                "Money" => "$1.00",
                "RadioButtons" => "RDO",
                "Separator" => "SEP",
                "StateCode" => "ST",
                "Text" => "ABC",
                "VIN" => "VIN",
                "YesNo" => "Y/N",
                "ZIP5" => "ZIP5",
                "ZIP10" => "ZIP10",
                _ => "ABC",
            };

        private PromptType GetType(string type) => type switch
            {
                "BuyerCoBoth" => PromptType.OneTwoThree,
                "BuyerCoBothOther" => PromptType.OneTwoThreeFour,
                "Checkbox" => PromptType.CheckBox,
                "Date" => PromptType.Date,
                "Decimal" => PromptType.Decimal,
                "Dropdown" => PromptType.Dropdown,
                "Integer" => PromptType.Integer,
                "InstructionLine" => PromptType.Label,
                "LabelNumber" => PromptType.LabelNumber,
                "Money" => PromptType.Money,
                "RadioButtons" => PromptType.RadioButtons,
                "Separator" => PromptType.Separator,
                "StateCode" => PromptType.StateCode,
                "Text" => PromptType.Text,
                "VIN" => PromptType.VIN,
                "YesNo" => PromptType.YesNo,
                "ZIP5" => PromptType.ZIP5,
                "ZIP10" => PromptType.ZIP10,
                _ => PromptType.Text
            };

    }
}
