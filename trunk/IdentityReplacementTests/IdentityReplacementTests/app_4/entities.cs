using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace app_4
{
    public class Level1
    {
        public int Id { get; set; }
        public List<Level2> levels { get; set; }
        public string Value { get; set; }
    }

    public class Level2
    {
        public int Id { get; set; }
        public List<Level3> levels { get; set; }
        public string Value { get; set; }
    }

    public class Level3
    {
        public int Id { get; set; }
        public List<Level4> levels { get; set; }
        public string Value { get; set; }
    }

    public class Level4
    {
        public int Id { get; set; }
        public List<Level5> levels { get; set; }
        public string Value { get; set; }
    }

    public class Level5
    {
        public int Id { get; set; }
        public List<string> levels { get; set; }
        public string Value { get; set; }
    }

    public class Identity
    {
        [MaxLength(32)]
        public string Id { get; set; }
        public int Key { get; set; }
    }

    public class MetricTracker : IDisposable
    {
        private static Stopwatch s;
        long ms;
        string label;
        Action<long> elapsed;

        public MetricTracker(string Label, Action<long> Elapsed)
        {
            elapsed = Elapsed;
            label = Label;
            Console.WriteLine("Starting: " + Label);
            s = new Stopwatch();
            s.Start();
        }

        public void Dispose()
        {
            s.Stop();
            ms = s.ElapsedMilliseconds;
            Console.WriteLine("Elapsed: " + ms / 1000.0);
            elapsed(ms);
        }
    }

    public class Levels : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;

            modelBuilder.Entity<Level1>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Level2>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Level3>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Level4>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Level5>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Identity>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }

        public DbSet<Level1> Level1 { get; set; }
        public DbSet<Identity> Identities { get; set; }
    }

    public class LevelsPoor : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;

            modelBuilder.Entity<Level1>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Level2>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Level3>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Level4>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Level5>()
                .Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public DbSet<Level1> Level1 { get; set; }
        public DbSet<Identity> Identities { get; set; }
    }

    public class BaseIdentityFactory<T1, T2, T3, T4, T5>
    {
        protected Dictionary<string, int> identities;
        private object readSynchronization;
        private object writeSynchronization;
        private object processSynchronization;

        public Dictionary<string, int> Identities
        {
            get { return identities; }
        }

        public BaseIdentityFactory(Object ReadSynchronization, Object WriteSynchronization, Object ProcessSynchronization)
        {
            identities = new Dictionary<string, int>();
            this.readSynchronization = ReadSynchronization;
            this.writeSynchronization = WriteSynchronization;
            this.processSynchronization = ProcessSynchronization;
        }

        public void ReadFromDatabase(Levels context)
        {
            lock (readSynchronization)
            {
                // The Read operation will always modify data to lock, so that other connections block until the transaction is committed.
                // See/run unit tests if changing, i.e. test Performance_Identities_Stored_In_The_Database_Concurrent_Access.

                foreach (var item in identities.Keys.ToList())  
                {
                    if (!context.Identities.Any(t => t.Id == item))
                    {
                        identities[item] = 0;
                        context.Identities.Add(new Identity() { Id = item, Key = identities[item] });
                    }
                    else
                    {
                        Identity id = context.Identities
                        .First(t => t.Id == item);
                        id.Key++;
                        identities[item] = id.Key;
                    }
                }
                context.SaveChanges();
            }
        }

        public void WriteToDatabase(Levels context)
        {
            lock (writeSynchronization)
            {
                foreach (var item in identities.Keys)
                {
                    Identity id = context.Identities.FirstOrDefault(t => t.Id == item);
                    id.Key = identities[item];
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Walk object graph and assign current identities to all fields named Id for registered classes.
        /// </summary>
        /// <param name="Root"></param>
        public void AssignIdentities(Object Root)
        {
            lock (processSynchronization)
            {
                ObjectWalker w = new ObjectWalker(Root);
                foreach (Object o in w)
                {
                    if (o is T1 || o is T2 || o is T3 || o is T4 || o is T5)
                    {
                        o.GetType().GetProperty("Id").SetValue(o, identities[o.GetType().Name], null);
                        identities[o.GetType().Name]++;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Overrides to present a concrete class.
    /// </summary>
    public class LevelIdentityFactory : BaseIdentityFactory<Level1, Level2, Level3, Level4, Level5>
    {
        public LevelIdentityFactory(Object ReadSynchronization, Object WriteSynchronization, Object ProcessSynchronization)
            : base(ReadSynchronization, WriteSynchronization, ProcessSynchronization)
        {
            Type t = typeof(LevelIdentityFactory);

            Type[] typeParameters = t.BaseType.GetGenericArguments();
            foreach (Type tParam in typeParameters)
            {
                if (!tParam.IsGenericParameter)
                    identities.Add(tParam.Name, 0);
            }
        }
    }
}