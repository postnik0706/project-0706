using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivatorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            if ( (args.Length != 3) || ( (args.Length >= 1) && (args[0] == "/?")) )
                Utilities.ShowInColor(
@"Usage parameters:
/Bldg <Bldg ID> /Activate
/Bldg <Bldg ID> /Deactivate
", ConsoleColor.Green);

            foreach (var item in args)
            {
                if ( (item == "/Bldg") && (item.Length > item.IndexOf(item)) )

            }
        }
    }
}
