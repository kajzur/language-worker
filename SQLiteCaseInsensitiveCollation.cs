using System.Data.SQLite;
using System.Globalization;

namespace SQLiteUTF8CIComparison
{
    /// <summary />
    /// This function adds case-insensitive sort feature to SQLite engine 
    /// To initialize, use SQLiteFunction.RegisterFunction() 
    /// before all connections are open 
    /// </summary />
    [SQLiteFunction(FuncType = FunctionType.Collation, Name = "UTF8CI")]
    public class SQLiteCaseInsensitiveCollation : SQLiteFunction
    {
        /// <summary />
        /// CultureInfo for comparing strings in case insensitive manner 
        /// </summary />
        private static readonly CultureInfo _cultureInfo =
            CultureInfo.CreateSpecificCulture("pl-PL");

        /// <summary />
        /// Does case-insensitive comparison using _cultureInfo 
        /// </summary />
        /// Left string
        /// Right string
        /// <returns />The result of a comparison</returns />
        public override int Compare(string x, string y)
        {
            return string.Compare(x, y, _cultureInfo, CompareOptions.IgnoreCase);
        }
    }
}