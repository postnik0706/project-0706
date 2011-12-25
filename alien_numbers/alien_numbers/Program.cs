using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace alien_numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inp = File.ReadAllLines(".\\A-large-practice.in");
            short num_cases = short.Parse(inp[0]);
            System.Diagnostics.Debug.Assert(num_cases > 0, "Num cases > 0");

            string[] res = new string[num_cases];
            for (int i = 0; i < num_cases; i++)
            {
                string[] record = inp[i + 1].Split(' ');
                res[i] = String.Format("Case #{0}: {1}",
                    i + 1,
                    dec_to_target(
                    source_to_dec(record[0], record[1]),
                    record[2]));
            }
            File.WriteAllLines(".\\out.txt", res);
        }

        static int source_to_dec(string alien_number, string source_language)
        {
            int n_alien_number = 0;
            for (int i = alien_number.Length-1; i >= 0 ; i--)
                n_alien_number += source_language.IndexOf(alien_number[alien_number.Length - i - 1])
                    * (int)(Math.Pow(source_language.Length, i));
            return n_alien_number;
        }

        static string dec_to_target(int dec, string target_language)
        {
            string source_language = "0123456789";
            string res = "";
            
            int num_digits = 1;
            while ((int)(dec / Math.Pow((double)target_language.Length, num_digits)) > 0)
                num_digits++;

            for (int j = num_digits - 1; j >= 0; j--)
            {
                int digit = (int)
                    (dec /
                    System.Math.Pow(target_language.Length, j));
                res += target_language[digit];
                dec = (int)(dec %
                    System.Math.Pow(target_language.Length, j));
            };
            return res;
        }
    }
}
