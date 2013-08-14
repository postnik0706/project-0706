using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using app_4;
using FluentAssertions;
using System.Transactions;
using System.IO;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlServerCe;

namespace tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Startup()
        {
            if (File.Exists(Program.DB_FILE))
                File.Delete(Program.DB_FILE);
            if (File.Exists(Program.DB_FILE_POOR))
                File.Delete(Program.DB_FILE_POOR);

            using (Levels context = new Levels())
            {
                context.Database.CreateIfNotExists();
                Level1 level = context.Level1.FirstOrDefault(t => t.Id == 3);
                if (level == null)
                {
                    context.Level1.Add(new Level1() { Id = 3, Value = "A" });
                    context.SaveChanges();
                }
                else
                {
                    level.Value = "A";
                    context.SaveChanges();
                }
            }

            using (LevelsPoor context = new LevelsPoor())
            {
                context.Database.CreateIfNotExists();
                Level1 level = context.Level1.FirstOrDefault(t => t.Id == 3);
                if (level == null)
                {
                    context.Level1.Add(new Level1() { Id = 3, Value = "A" });
                    context.SaveChanges();
                }
                else
                {
                    level.Value = "A";
                    context.SaveChanges();
                }
            }
        }
        
        [TestMethod]
        public void Concurrency_Test_Must_Succeed()
        {
            ManualResetEventSlim ev = new ManualResetEventSlim(false);

            Task task1 = new TaskFactory().StartNew(() =>
            {
                // First thread starts editing.
                using (Levels context = new Levels())
                {
                    ev.Wait();

                    Trace.WriteLine("[First thread] fires up");

                    ObjectContext o = ((IObjectContextAdapter)context).ObjectContext;
                    o.Connection.Open();
                    try
                    {
                        using (TransactionScope tran = new TransactionScope(
                            TransactionScopeOption.RequiresNew,
                        new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead }))
                        {
                            // Protected Read
                            Trace.WriteLine("[First thread] Protected Read");
                            Level1 level = context.Level1.First(t => t.Id == 3);
                            string beforeEdit = level.Value;

                            // Protected modification
                            context.Database.ExecuteSqlCommand(
                                "update Level1 set value = {0} where id = {1}",
                                level.Value + "C", 3);

                            Trace.WriteLine("[First thread] Some long work");
                            Thread.Sleep(1000);
                            Trace.WriteLine("[First thread] End of long work");

                            context.SaveChanges();
                            tran.Complete();
                            Trace.WriteLine("[First thread] changes saved");
                        }
                    }
                    finally
                    {
                        o.Connection.Close();
                    }
                    Thread.Sleep(1000);      // so that the second thread was enough time to save
                }

                using (Levels context = new Levels())
                {
                    Trace.WriteLine("[First thread] Verification");
                    Level1 level = context.Level1.First(t => t.Id == 3);
                    level.Value.Should()
                        .Contain("A")
                        .And.Contain("B")
                        .And.Contain("C");
                    Trace.WriteLine("[First thread] ended");
                }
            });

            Task task2 = new TaskFactory().StartNew(() =>
            {
                // Second thread also edits
                using (Levels context = new Levels())
                {
                    ev.Wait();

                    Trace.WriteLine("[Second thread] fires up");
                    Thread.Sleep(200);      // So that the first thread starts read first

                    ObjectContext o = ((IObjectContextAdapter)context).ObjectContext;
                    o.Connection.Open();
                    try
                    {
                        using (TransactionScope tran = new TransactionScope(
                            TransactionScopeOption.RequiresNew,
                        new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead }))
                        {
                            Trace.WriteLine("[First thread] Protected Read");
                            Level1 level = context.Level1.First(t => t.Id == 3);
                            string beforeEdit = level.Value;

                            // Protected modification
                            level.Value += "B";

                            // some work
                            Trace.WriteLine("[Second thread] Some work");

                            context.SaveChanges();
                            
                            tran.Complete();
                            Trace.WriteLine("[Second thread] changes saved");
                        }
                    }
                    finally
                    {
                        o.Connection.Close();
                    }
                }

                using (Levels context = new Levels())
                {
                    Trace.WriteLine("[Second thread] Verification");
                    Level1 level = context.Level1.First(t => t.Id == 3);
                    level.Value.Should()
                        .Contain("A")
                        .And.Contain("B")
                        .And.Contain("C");
                    Trace.WriteLine("[Second thread] ended");
                }
            });

            Thread.Sleep(500);
            ev.Set();

            Task.WaitAll(new Task[] { task1, task2 });
        }

        [TestMethod]
        public void Poor_Performance()
        {
            long ms = 0;

            using (MetricTracker m = new MetricTracker("Starting simple object", t => ms = t))
            {
                Level1 level1 = new Level1();
                level1.Value = "test";
                level1.levels = new List<Level2>();

                Trace.WriteLine("Starting the test");
                for (int i = 0; i < 1296; i++)
                {
                    Level2 curLevel2 = new Level2();
                    level1.levels.Add(curLevel2);
                    curLevel2.Value = "test" + i.ToString();
                    curLevel2.levels = new List<Level3>();
                }
                using (LevelsPoor context = new LevelsPoor())
                {
                    context.Level1.Add(level1);
                    context.SaveChanges();
                    Trace.WriteLine("Done simple save");
                }
                ms.Should().BeLessThan(2960);
                ms = 0;
            }

            using (LevelsPoor context = new LevelsPoor())
            using (new MetricTracker("Starting complex object", t => ms = t))
            {
                Level1 level1 = new Level1();
                level1.Value = "test";
                level1.levels = new List<Level2>();

                for (int i = 0; i < 5; i++)
                {
                    Level2 curLevel2 = new Level2();
                    level1.levels.Add(curLevel2);
                    curLevel2.Value = "test" + i.ToString();
                    curLevel2.levels = new List<Level3>();

                    for (int j = 0; j < 5; j++)
                    {
                        Level3 curLevel3 = new Level3();
                        curLevel2.levels.Add(curLevel3);
                        curLevel3.Value = "test" + j.ToString();
                        curLevel3.levels = new List<Level4>();

                        for (int k = 0; k < 10; k++)
                        {
                            Level4 curLevel4 = new Level4();
                            curLevel3.levels.Add(curLevel4);
                            curLevel4.Value = "test" + k.ToString();
                            curLevel4.levels = new List<Level5>();

                            for (int l = 0; l < 10; l++)
                            {
                                Level5 curLevel5 = new Level5();
                                curLevel4.levels.Add(curLevel5);
                                curLevel5.Value = "test" + l.ToString();
                            }
                        }
                    }
                }

                context.Level1.Add(level1);
                context.SaveChanges();
            }

            ms.Should().BeLessThan(15000);
            ms = 0;
        }

        [TestMethod]
        public void Performance_Complex_Object()
        {
            long ms = 0;

            int[] levelIds = new int[5];

            using (MetricTracker m = new MetricTracker("Starting simple object", t => ms = t))
            {
                Level1 level1 = new Level1();
                level1.Value = "test";
                level1.Id = levelIds[0]++;
                level1.levels = new List<Level2>();

                Trace.WriteLine("Starting the test");
                for (int i = 0; i < 1296; i++)
                {
                    Level2 curLevel2 = new Level2();
                    level1.levels.Add(curLevel2);
                    curLevel2.Id = levelIds[1]++;
                    curLevel2.Value = "test" + i.ToString();
                    curLevel2.levels = new List<Level3>();
                }
                using (Levels context = new Levels())
                {
                    context.Level1.Add(level1);
                    context.SaveChanges();
                    Trace.WriteLine("Done simple save");
                }
                ms.Should().BeLessThan(2960);
                ms = 0;
            }

            using (new MetricTracker("Starting complex object", t => ms = t))
            using (Levels context = new Levels())
            {
                SaveComplexObject(context, levelIds);
            }

            ms.Should().BeLessThan(1440);
            ms = 0;
        }

        private void SaveComplexObject(Levels context, int[] levelIds)
        {
            // Protected Read            
            Level1 level1 = new Level1();
            level1.Value = "test";
            level1.Id = levelIds[0]++;
            level1.levels = new List<Level2>();

            for (int i = 0; i < 5; i++)
            {
                Level2 curLevel2 = new Level2();
                level1.levels.Add(curLevel2);
                curLevel2.Id = levelIds[1]++;
                curLevel2.Value = "test" + i.ToString();
                curLevel2.levels = new List<Level3>();

                for (int j = 0; j < 5; j++)
                {
                    Level3 curLevel3 = new Level3();
                    curLevel2.levels.Add(curLevel3);
                    curLevel3.Id = levelIds[2]++;
                    curLevel3.Value = "test" + j.ToString();
                    curLevel3.levels = new List<Level4>();

                    for (int k = 0; k < 10; k++)
                    {
                        Level4 curLevel4 = new Level4();
                        curLevel3.levels.Add(curLevel4);
                        curLevel4.Id = levelIds[3]++;
                        curLevel4.Value = "test" + k.ToString();
                        curLevel4.levels = new List<Level5>();

                        for (int l = 0; l < 10; l++)
                        {
                            Level5 curLevel5 = new Level5();
                            curLevel4.levels.Add(curLevel5);
                            curLevel5.Id = levelIds[4]++;
                            curLevel5.Value = "test" + l.ToString();
                        }
                    }
                }
            }

            context.Level1.Add(level1);
            context.SaveChanges();
        }

        public static Level1 GetSample()
        {
            Level1 level1 = new Level1();
            level1.Value = "test";
            level1.levels = new List<Level2>();

            for (int i = 0; i < 5; i++)
            {
                Level2 curLevel2 = new Level2();
                level1.levels.Add(curLevel2);
                curLevel2.Value = "test" + i.ToString();
                curLevel2.levels = new List<Level3>();

                for (int j = 0; j < 5; j++)
                {
                    Level3 curLevel3 = new Level3();
                    curLevel2.levels.Add(curLevel3);
                    curLevel3.Value = "test" + j.ToString();
                    curLevel3.levels = new List<Level4>();

                    for (int k = 0; k < 10; k++)
                    {
                        Level4 curLevel4 = new Level4();
                        curLevel3.levels.Add(curLevel4);
                        curLevel4.Value = "test" + k.ToString();
                        curLevel4.levels = new List<Level5>();

                        for (int l = 0; l < 10; l++)
                        {
                            Level5 curLevel5 = new Level5();
                            curLevel4.levels.Add(curLevel5);
                            curLevel5.Value = "test" + l.ToString();
                        }
                    }
                }
            }

            return level1;
        }

        [TestMethod]
        public void Performance_Complex_Object_In_Transaction()
        {
            long ms = 0;

            int[] levelIds = new int[5];

            using (new MetricTracker("Starting complex object", t => ms = t))
            {
                using (Levels context = new Levels())
                {
                    ObjectContext o = ((IObjectContextAdapter)context).ObjectContext;
                    o.Connection.Open();
                    try
                    {
                        using (TransactionScope tran = new TransactionScope(
                            TransactionScopeOption.RequiresNew,
                        new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead }))
                        {
                            SaveComplexObject(context, levelIds);
                            tran.Complete();
                        }
                    }
                    finally
                    {
                        o.Connection.Close();
                    }
                }
            }

            ms.Should().BeLessThan(1440);
        }

        private void Test_Identities_Stored_In_The_Database(Action<long> ms, Object ReadSynchronization, Object WriteSynchronization, Object ProcessSynchronization)
        {
            LevelIdentityFactory ids = new LevelIdentityFactory(ReadSynchronization, WriteSynchronization, ProcessSynchronization);

            using (Levels context = new Levels())
            using (new MetricTracker("Starting complex object", ms))
            {

                ObjectContext o = ((IObjectContextAdapter)context).ObjectContext;
                o.Connection.Open();
                try
                {
                    // Unprotected object creation and work.
                    Level1 level1 = GetSample();

                    using (TransactionScope tran = new TransactionScope(
                        TransactionScopeOption.RequiresNew,
                    new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                    {
                        // Protected identification set read.
                        ids.ReadFromDatabase(context);

                        // Protected Id assignment.
                        ids.AssignIdentities(level1);

                        // Protected object save.
                        context.Level1.Add(level1);
                        context.SaveChanges();

                        // Protected new Id values write.
                        ids.WriteToDatabase(context);

                        // Completion
                        tran.Complete();
                    }
                }
                finally
                {
                    o.Connection.Close();
                }
            }
        }

        [TestMethod]
        public void Performance_Identities_Stored_In_The_Database()
        {
            long ms = 0;
            Object readSynchronization = new object();
            Object writeSynchronization = new object();
            Object processSynchronization = new object();
            Test_Identities_Stored_In_The_Database(t => ms = t, readSynchronization, writeSynchronization, processSynchronization);
            ms.Should().BeLessThan(1440);

            using (Levels context = new Levels())
            {
                LevelIdentityFactory ids = new LevelIdentityFactory(readSynchronization, writeSynchronization, processSynchronization);
                ids.ReadFromDatabase(context);
                ids.Identities["Level1"].Should().Be(2);        // + "A"
                ids.Identities["Level2"].Should().Be(6);        // + 1 extra number per each insert
                ids.Identities["Level3"].Should().Be(26);       // + 1 extra number per each insert
                ids.Identities["Level4"].Should().Be(251);      // + 1 extra number per each insert
                ids.Identities["Level5"].Should().Be(2501);     // + 1 extra number per each insert
            }
        }

        [TestMethod]
        public void Performance_Identities_Stored_In_The_Database_Concurrent_Access()
        {
            ManualResetEventSlim ev = new ManualResetEventSlim(false);
            Object readSynchronization = new object();
            Object writeSynchronization = new object();
            Object processSynchronization = new object();
            long ms1 = 0;
            long ms = 0;

            Action a = new Action(() =>
                {
                    using (Levels context = new Levels())
                    {
                        ev.Wait();
                        Test_Identities_Stored_In_The_Database(t => ms1 += t, readSynchronization, writeSynchronization, processSynchronization);
                        Trace.WriteLine(string.Format("[Thread {0}]: elapsed {1}", Thread.CurrentThread.ManagedThreadId.ToString(), (ms1 /1000.00).ToString()));
                    }
                });

            const int NUM_TASKS = 20;

            using (new MetricTracker("Starting multiple threads", t => ms += t))
            {
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < NUM_TASKS; i++)
                {
                    tasks.Add(new TaskFactory().StartNew(a));
                }
                ev.Set();

                Task.WaitAll(tasks.ToArray());
            }

            Console.WriteLine("Elapsed: {0}", (ms1 / 1000.00).ToString());
            ms1.Should().BeLessThan(17900);

            using (Levels context = new Levels())
            {
                LevelIdentityFactory ids = new LevelIdentityFactory(readSynchronization, writeSynchronization, processSynchronization);
                ids.ReadFromDatabase(context);
                ids.Identities["Level1"].Should().Be(NUM_TASKS + 1);        // + "A"
                ids.Identities["Level2"].Should().Be(NUM_TASKS * 5 + 1);        // + 1 extra number per each insert
                ids.Identities["Level3"].Should().Be(NUM_TASKS * 25 + 1);       // + 1 extra number per each insert
                ids.Identities["Level4"].Should().Be(NUM_TASKS * 250 + 1);      // + 1 extra number per each insert
                ids.Identities["Level5"].Should().Be(NUM_TASKS * 2500 + 1);     // + 1 extra number per each insert
            }
        }
    }
}