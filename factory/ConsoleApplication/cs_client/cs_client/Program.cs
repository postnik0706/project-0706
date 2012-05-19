using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cs_client
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceConsole.SecurityConsoleClient cli = new ServiceConsole
                .SecurityConsoleClient("BasicHttpBinding_ISecurityConsole");
            
            ServiceConsole.BuildingSensorsResponse response = new ServiceConsole.BuildingSensorsResponse();
            cli.ReportStatus(null, "test");
        }
    }
}
