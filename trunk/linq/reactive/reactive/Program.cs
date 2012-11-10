using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Threading;

namespace reactive
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Thread {0}", Thread.CurrentThread.ManagedThreadId);
            /*
            var o = Observable.Start(() =>
                {
                    Console.WriteLine("Calc");
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(3000); 
                    Console.WriteLine("Done");
                });
            o.First();
            */

            var observable = Observable.Generate<int, int>(
                0,
                x =>
                {
                    printThreadId("Comparing " + x);
                    return x < 500000;
                },
                x =>
                {
                    printThreadId("Incrementing " + x);
                    return x + 1;
                },
                x =>
                {
                    printThreadId("Selecting " + x);
                    return x;
                },
                    NewThreadScheduler.Default);

            

            Func<int, IObservable<Unit>> process = state =>
                Observable.Start(() =>
                    {
                        printThreadId("Processing " + state);
                        //Thread.Sleep(10);
                        return Unit.Default;
                    });

            var query =
                from x in observable
                select process(x);
            
            var disp =
                query
                    .Merge()
                    .ObserveOn(NewThreadScheduler.Default)
                    .Subscribe();
            
            /*var disp = observable
                .ObserveOn(NewThreadScheduler.Default)
                .Subscribe(state =>
                {
                    printThreadId("Processing " + state);
                    Thread.Sleep(1000);
                });
            */

            /*
            var query = from number in Enumerable.Range(1, 200)
                        select SnoozeNumberProduction(number);

            var obsQuery = query.ToObservable(NewThreadScheduler.Default);
            var bufSeq = obsQuery.Buffer(TimeSpan.FromSeconds(1));

            Random random = new Random();
            Int32 count = 0;
            bufSeq.ObserveOn(NewThreadScheduler.Default).Subscribe(list =>
                {
                    Console.WriteLine("({0}) Numbers from {1}-{2} produced on Thread ID {3}", list.Count, list[0], list[list.Count - 1], Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(1000);
                    count++;
                    
                    if (count == 4)
                    {
                        Console.WriteLine("count reached to 4, blocking ... press any key to continue ");
                        Console.ReadKey(); // Block and build up the queue
                    }

                    Console.WriteLine("Woken " + list[0] + " - " + list[list.Count - 1]);
                });
            */

            /*Action leafAction = () => Console.WriteLine("leafAction.");
            Action innerAction = () =>
            {
                Console.WriteLine("innerAction start." + Thread.CurrentThread.ManagedThreadId);
                Scheduler.NewThread.Schedule(leafAction);
                Console.WriteLine("innerAction end." + Thread.CurrentThread.ManagedThreadId);
            };
            Action outerAction = () =>
            {
                Console.WriteLine("outer start." + Thread.CurrentThread.ManagedThreadId);
                Scheduler.NewThread.Schedule(innerAction);
                Console.WriteLine("outer end." + Thread.CurrentThread.ManagedThreadId);
            };
            Scheduler.Immediate.Schedule(outerAction);
            */

            Console.ReadKey();
        }

        private static void printThreadId(string p)
        {
            Console.WriteLine("{0} on [Worker.{1}]", p, Thread.CurrentThread.ManagedThreadId);
        }

        private static void SnoozeNumberProduction(int number)
        {
            Thread.Sleep(250);
            Console.WriteLine("{0}, {1}", number, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
