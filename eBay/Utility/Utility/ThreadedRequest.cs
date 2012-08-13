using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eBay.Service.Core.Sdk;
using System.Threading;

namespace Utility
{
    public class ThreadedRequest
    {
        private List<PageProducer> pages = new List<PageProducer>();
        private List<Thread> threads;
        private ApiContext apiContext;
        private int processorCount;

        public List<PageProducer> Pages
        {
            get
            {
                return pages;
            }
        }

        public ThreadedRequest()
        {
        }

        public ThreadedRequest(ApiContext ApiContext, PageObjectFactory Factory)
        {
            Log.AddLogInfo("Requesting Page 0");
            
            
            //processorCount = Environment.ProcessorCount;
            processorCount = 1;

            pages.Add(Factory.CreatePageObject(ApiContext, 1));             // Page 1 will return the number of pages
            pages[0].Execute();

            if (pages[0].NumPages > 1)
            {
                Log.AddLogInfo(String.Format("Number of pages: {0}", pages[0].NumPages));
                for (int i = 2; i <= pages[0].NumPages; i++)
                    pages.Add(Factory.CreatePageObject(ApiContext, i));

                int page = 1;
                
                if (processorCount > 1)
                    threads = new List<Thread>();
                
                while (page < pages[0].NumPages)
                {
                    for (int i = 0; i < processorCount; i++)
                    {
                        if (page >= pages[0].NumPages)
                            break;

                        Log.AddLogInfo(String.Format("Moved on to page {0}", page));
                        if (processorCount > 1)
                        {
                            threads.Add(new Thread(pages[page].Execute));
                            Log.AddLogInfo(String.Format("Created thread {0}", threads[threads.Count - 1].ManagedThreadId));
                            threads[i].Start();
                        }
                        else
                        {
                            pages[page].Execute();
                            Log.AddLogInfo(String.Format("Run page {0}", page));
                        }
                        page++;
                    }

                    Log.AddLogInfo("Entering Join");

                    if (processorCount > 1)
                    {
                        try
                        {
                            foreach (var t in threads)
                                t.Join();
                            Log.AddLogInfo("Join quit");
                            threads.Clear();
                        }
                        catch (Exception e)
                        {
                            Log.AddLogInfo(String.Format("Exception while joining: {0}", e.Message));
                            throw;
                        }
                    }
                }
            }
        }
    }
}
