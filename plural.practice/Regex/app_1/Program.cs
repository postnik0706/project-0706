using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace regex
{
    class Program
    {
        const string MatchSuccess = "{0} @{1}:{2}";

        static void Main(string[] args)
        {
            string pattern = args[0].Replace("\\n", "\n");
            string subject = args[1].Replace("\\n", "\n");
            Regex regex = new Regex(pattern);
            Match match = regex.Match(subject);
            while (match.Success)
	        {
                Console.WriteLine(MatchSuccess, match.Success, match.Index, match.Length);
	            match = match.NextMatch();
            }
            Console.WriteLine(match.Success);
        }
    }
}