using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB
{
    

    class ParsingProgramm
    {
        static void Main(string[] args)
        {
            string code = File.ReadAllText(@"TestProgramm.cpp");
             Simplifier simplifier = new Simplifier(code);
             string result = simplifier.Simplify();
             File.WriteAllText(@"TestProgramm-result.cpp", result);

             Tuple<double, int> complexity = OperatorCounter.RelativeComplexity(code, result);

             Parser parser = new Parser(@"TestProgramm-result.cpp");
             parser.Parse();
             //parser.PrintStatements();
             string[] arr = parser.GetArray();
             Nesting nesting = new Nesting(arr);
             Console.WriteLine("Max nesting " + nesting.MaxNesting().ToString() + "\nRelative complexity " + complexity.Item1.ToString() + "\nAbsolute complexity " + complexity.Item2.ToString());

            Console.ReadKey();
        }
    }
}


