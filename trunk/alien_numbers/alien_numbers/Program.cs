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
            string[] inp = File.ReadAllLines(".\\simple.txt");
            short num_cases = short.Parse(inp[0]);
            System.Diagnostics.Debug.Assert(num_cases > 0, "Num cases > 0");

            string[] res = new string[num_cases];
            for (int i = 0; i < num_cases; i++)
            {
                string[] record = inp[i + 1].Split(' ');

                int x = 0;
                for (int j = 0; j < record[0].Length; j++)
                {
                    x *= record[1].Length;
                    x += record[1].IndexOf(record[0][j]);
                }

                res[i] = "";
                while (x > 0)
                {
                    res[i] = record[2][x % record[2].Length] + res[i];
                    x /= record[2].Length;
                };
            }
            File.WriteAllLines(".\\out.txt", res);
        }
    }
}
