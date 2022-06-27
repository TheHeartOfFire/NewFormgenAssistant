using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormgenAssistant.DataTypes;
using Newtonsoft.Json;

namespace FormgenAssistant.SavedItems
{
    public class Snippets
    {
        //Serializable Properties
        public List<CodeSnippet> CodeSnippets { get; set; }

        //Singleton
        [JsonIgnore]
        private static Snippets _instance = null;
        [JsonIgnore]
        public static Snippets Instance
        {

            get { return _instance ??= new Snippets(); }

            set => _instance = value;
        }

        //Default stuff
        [JsonIgnore]
        private static readonly Snippets DefaultSnippets = new()
        {
            CodeSnippets = new List<CodeSnippet>()
            {
                //new
                //(
                //    "Seplist",
                //    "SEPLIST( \'{0}\', {1}, {2} )",
                //    new List<string>()
                //    {
                //        { "Separator"},
                //        { "Item"},
                //        { "Item"}
                //    },
                //    "Separate a list of items with the given separator.",
                //    "{0}, {1}",
                //    2,
                //    new List<string>()
                //    {
                //        {"Item"},
                //        {"Item"}
                //    }
                //),
                //new
                //(
                //    "If Statement",
                //    "IF( {0}, {1}, {2} )",
                //    new List<string>()
                //    {
                //        { "Condition"},
                //        { "IfTrue"},
                //        { "IfFalse"}
                //    },
                //    "If the condition is true, return the true value, otherwise return the false value.",
                //    "IF( {0}, {1}, {2}",
                //    2,
                //    new List<string>()
                //    {
                //        { "Condition"},
                //        { "IfTrue"},
                //        { "IfFalse"}
                //    }
                //),
                //new
                //(
                //    "Case Statement",
                //    "CASE( {0}, {1}, {2}, {3}, {4}, {5} )",
                //    new List<string>()
                //    {
                //        { "Comparison"},
                //        { "Case"},
                //        { "Result"},
                //        { "Case"},
                //        { "Result"},
                //        { "Default"}
                //    },
                //    "Compare the comparison value to the case values, return the corresponding result of any matches, otherwise return the default value. Identical to multiple nested If Statements.",
                //    "{0}, {1}, {2}",
                //    4,
                //    new List<string>()
                //    {
                //        { "Result"},
                //        { "Case"},
                //        { "Result"}
                //    }

                //),
                new
                (
                    "Month Names",
                    "CASE( {0}, 1, 'JANUARY', 2, 'FEBRUARY', 3, 'MARCH', 4, 'APRIL', 5, 'MAY', 6, 'JUNE', 7, 'JULY', 8, 'AUGUST', 9, 'SEPTEMBER', 10, 'OCTOBER', 11, 'NOVEMBER', 12, 'DECEMBER', 'Invalid Month' )",
                    new List<string>()
                    {
                        { "Date Field"}
                    },
                    "Get the name of the specified month. 1 = January, 2 = February, etc."
                ),
                new
                (
                
                    "Day and Suffix",
                    "TEXT( ROUND( {0}, 0))+CASE( {0}, 11, 'th', 12, 'th', 13, 'th', CASE( {0} % 10, 1, 'st', 2, 'nd', 3, 'rd', 'th' ))",
                    new List<string>()
                    {
                        { "Date Field"}
                    },
                    "Returns the day of the month with the suffix. 11 = 11th, 12 = 12th, 13 = 13th, 21 = 21st, 22 = 22nd, etc."
                ),
                new
                (

                    "Left Justified Numbers",
                    "TEXT( ROUND( {0}, {1} ))",
                    new List<string>()
                    {
                        { "Numeric Field"},
                        { "Decimal Places"}
                    },
                    "Allows Numbers to be left Justified."
                ),
                new
                (

                    "Date Conversion",
                    "SEPLIST( \'/\', MONTH( {0} ), DAY( {0} ), YEAR( {0} ) % 100 )",
                    new List<string>()
                    {
                        { "Date Field"}
                    },
                    "Converts a date in the format of MM/DD/YYYY to a date in the format of MM/DD/YY."
                ),
                new
                (

                    "Seplist Number",
                    "IF( {0} != 0, ROUND({0}, {1} ))",
                    new List<string>()
                    {
                        { "Numeric Field"},
                        { "Decimal Places"}
                    },
                    "Allow Numeric fields to be used in seplist."
                )
            }
        };
    
        private Snippets() { }
        //Json stuff
        private static readonly string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FormgenAssistant";
        private static readonly string FileName = FilePath + "\\CodeSnippets.json";
        [JsonConstructor]
        public Snippets(List<CodeSnippet> codeSnippets)
        {
            CodeSnippets = codeSnippets;
        }
        public static void Load()
        {
            Directory.CreateDirectory(FilePath);
            if (!File.Exists(FileName)) Save();

            Instance = JsonConvert.DeserializeObject<Snippets>(File.ReadAllText(FileName)) ?? DefaultSnippets;
            if (Instance.CodeSnippets == null)
            {
                Instance.CodeSnippets = DefaultSnippets.CodeSnippets;
                Save();
            }
        }

        public static void Save()
        {
            string json = JsonConvert.SerializeObject(Instance);
            Directory.CreateDirectory(FilePath);
            if (!File.Exists(FileName)) File.Create(FileName).Close();
            File.WriteAllText(FileName, json);

        }
    }
}
