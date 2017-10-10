using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace LAB
{
    public static class Extensions
    {
        public static string Replace(this Match match, string source, string replacement)
        {
            return source.Substring(0, match.Index) + replacement + source.Substring(match.Index + match.Length);
        }

        public static string SkipComments(this string code)
        {
            string single_comment_pattern = @"//([^\n]+)";
            string mult_comment_pattern = @"/\*.+(?=\*/)\*/";
            code = Regex.Replace(code, single_comment_pattern, "");
            code = Regex.Replace(code, mult_comment_pattern, "", RegexOptions.Singleline);
            return code;
        }

        public static string SkipStrings(this string code)
        {
            string string_pattern = "\"[^\"]+\"";
            code = Regex.Replace(code, string_pattern, "");
            return code;
        }
    }


    class Simplifier
    {
        private string _initial_code;
        private string _simplified_code;

        string _empty_switch_pattern = @"switch[\s\n\t]*\([^)]+\)[\s\n\t]*([^\{\:;]+)?;";
        string _simple_switch_pattern = @"switch[\s\n\t]*\([^)]+\)[\s\n\t]*[^\{\:]+\:";
        //simple_switch может содержать case или default,который содержит один оператор либо много операторов в скобках,либо просто содержать один оператор без case или default
        string _switch_pattern = @"switch[\s\n\t]*\(([^)]+)\)([\s\n\t]*)\{";//switch с фигурными скобками
        string _case_pattern = @"case([\s\n\t\n]+)([^:]+)\:";
        string _first_case_pattern = @"\{([\s\n\t\r]*)(case)([\s\n\t\n]+)([^:]+)\:";

        string _for_pattern = @"for[\s\n\t]*\([^)]+\)";

        string _while_pattern = @"while[\s\n\t]*\([^)]+\)";
        string _do_pattern = @"([\s\n\}\r\t\:\)\/\{;]+)(do)([\s\n\t\{;]+)";
        string _default_pattern = @"(default)([\s\n\t]*)\:([^}]+)";

        public Simplifier(string code)
        {
            _initial_code = code;
            _simplified_code = _initial_code;

        }

        private void ReplaceEmptySwitch()
        {
            _simplified_code = Regex.Replace(_simplified_code, _empty_switch_pattern, @"if(true)$1;");
        }
        private void ReplaceSimpleSwitch()
        {
            Match match = Regex.Match(_simplified_code, _simple_switch_pattern);
            match = match.NextMatch();
            match = match.NextMatch();
            _simplified_code = Regex.Replace(_simplified_code, _simple_switch_pattern, @"if(true)");
        }

        private void ReplaceFor()
        {
            _simplified_code = Regex.Replace(_simplified_code, _for_pattern, @"if(true)");
            //заменяем все for на if(true),оставляя пробелы/табуляции/enter, чтобы if(true) не слилось с остальным текстом
        }

        private void ReplaceFirstCases()
        {
            _simplified_code = Regex.Replace(_simplified_code, _first_case_pattern, @"{$1if(true){$3");
            //заменяем первый case на if(true),2-я { - открывающая скобка для операторов внутри case,1-я {-относится к switch,которая потом удалится
        }

        private void ReplaceCases()
        {
            _simplified_code = Regex.Replace(_simplified_code, _case_pattern, @"}else if(true){");
            //закрываем скобкой } операторы предыдущего case и открываем { для операторов текущего case;откр.скобка последнего case закроется закрывающей скобкой switch
        }

        private void ReplaceSwitch()
        {
            _simplified_code = Regex.Replace(_simplified_code, _switch_pattern, "");
        }

        private void ReplaceDefault()
        {
            _simplified_code = Regex.Replace(_simplified_code, _default_pattern, @"}else$2{$3");
            //заменяем на else,открываем скобку для его операторов,она закроется закр.скобкой от switch
        }

        private void ReplaceWhile()
        {
            _simplified_code = Regex.Replace(_simplified_code, _while_pattern, @"if(true)");
            //удаляем switch и открывающую скобку после него
        }

        private void ReplaceDo()//и do и while в do while заменяем if, вложенность от этого не поменяется,т.е они всегда на одном уровне.
        {
            _simplified_code = Regex.Replace(_simplified_code, _do_pattern, @"$1if(true)$3");
        }

        public string Simplify()
        {
            _simplified_code = _simplified_code.SkipComments();
            _simplified_code = _simplified_code.SkipStrings();
            ReplaceFor();
            ReplaceWhile();
            ReplaceDo();
            ReplaceEmptySwitch();
            ReplaceSimpleSwitch();
            ReplaceFirstCases();
            ReplaceCases();
            ReplaceSwitch();
            ReplaceDefault();

            return _simplified_code;

        }
    }

}
