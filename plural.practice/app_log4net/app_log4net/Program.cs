using com.foo;
using log4net;
using log4net.Config;

namespace app_log4net
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new System.IO.FileInfo("app_log4net.exe.config"));

            log.Info("Entering app");
            Bar bar = new Bar();
            bar.DoIt();
            log.Info("Exiting app");
        }
    }
}
