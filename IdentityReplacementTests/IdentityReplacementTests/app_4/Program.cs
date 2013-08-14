using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace app_4
{
    public class Program
    {
        public static string DB_FILE = @"c:\temp\myData.sdf";
        public static string DB_FILE_POOR = @"c:\temp\myDataPoor.sdf";

        static int[] levelIds = new int[5];

        public static Level1 GetSample()
        {
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

            return level1;
        }

        public static void BatchTest()
        {
            Level1 level1 = new Level1();
            level1.Value = "test";
            level1.Id = levelIds[0]++;
            level1.levels = new List<Level2>();
            long ms = 0;

            using (new MetricTracker("Simple save test", t => ms = t))
            {
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
                }
            }

            using (new MetricTracker("Complex save test - preparation ", t => ms = t))
            {
                level1 = GetSample();
            }

            using (Levels context = new Levels())
            using (new MetricTracker("Complex save test - Save", t => ms = t))
            {
                context.Level1.Add(level1);
                context.SaveChanges();
            }
        }

        static void Main(string[] args)
        {
            if (File.Exists(DB_FILE))
                File.Delete(DB_FILE);
        
            BatchTest();

            /*using (Levels context = new Levels())
            {
                LevelIdentityFactory f = new LevelIdentityFactory();
                f.ReadFromDatabase(context);
                f.AssignIdentities(GetSample_ex());
                //f.Increase();
                f.WriteToDatabase(context);
            }*/

            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }
    }
}