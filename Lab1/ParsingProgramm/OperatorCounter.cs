using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LAB
{
    static class OperatorCounter
    {
        private static string _do_pattern = @"([\s\n\}\r\t\:\)\/\{;]+)(do)([\s\n\t\{;]+)";
        private static string _operator_pattern = @";";
        private static string _if_pattern = @"if[\s\n\t\r]*\([^(]+\)";
        private static string _for_pattern = @"(for)[\s\n\t]*\([^)]+\)";
        
        public static Tuple<double, int> RelativeComplexity(string code, string simplified_code)
        {
            code = code.SkipComments();
            code = code.SkipStrings();

            int for_amount = (Regex.Matches(code, _for_pattern)).Count;
            int do_amount = (Regex.Matches(code, _do_pattern)).Count;
            int semicolon_amount = (Regex.Matches(simplified_code, _operator_pattern)).Count;
            int if_amount = (Regex.Matches(simplified_code, _if_pattern)).Count;
            int operator_amount = semicolon_amount +if_amount - 2 * do_amount + 2 * for_amount;
            //т.к в do while; вместо do добавляли if(true) и вместо while(..); добавляли if(true); 
            //вместо for(..;..;..) добавляли if(true)

            return new Tuple<double, int>(1.0 * (if_amount -  do_amount) / operator_amount, if_amount - do_amount);
        }

    }
}
