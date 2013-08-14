using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using app_4;
using System.Data.Entity.Infrastructure;
using System.Data.SqlServerCe;
using FluentAssertions;

namespace tests
{
    [TestClass]
    public class CESpecific
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
        public void Ce_Insert_LowLevel()
        {
            long ms = 0;

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

                SqlCeConnectionFactory cf = new SqlCeConnectionFactory("System.Data.SqlServerCE.4.0");
                using (SqlCeConnection conn = (SqlCeConnection)cf.CreateConnection(@"c:\temp\myDataPoor.sdf"))
                {
                    conn.Open();

                    // Level1
                    int parentId = SqlCeInsertAdapterLevels.Write_Level1(level1, conn);
                }
            }

            ms.Should().BeLessThan(775);
        }
    }
}
