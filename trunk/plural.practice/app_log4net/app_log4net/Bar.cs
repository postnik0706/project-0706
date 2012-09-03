using log4net;

namespace com.foo
{
    class Bar
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Bar));
        
        internal void DoIt()
        {
            log.Debug("Did it again!");
            log.Warn("And here is a warning");
            log.Error("And here is an exception");
        }
    }
}
