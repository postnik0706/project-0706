using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlServerCe;
using app_4;
using System.Runtime.Serialization;
using System.Reflection;
using System.Data.Entity.Infrastructure;
using System.Data.Metadata.Edm;
using FluentAssertions;

namespace tests
{
    public class FieldDescriptor
    {
        public int Ordinal { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
    
    public class CachedTableInfo
    {
        private string _table;
        ShipmentContext _context;
        SqlCeResultSet _data;
        private FieldDescriptor[] _keys;

        public CachedTableInfo(string Table, ShipmentContext context, SqlCeResultSet data)
        {
            _table = Table;
            _context = context;
            _data = data;
        }

        public FieldDescriptor[] Keys
        {
            get
            {
                if (_keys == null)
                {
                    List<FieldDescriptor> ret = null;
                    
                    var context = ((IObjectContextAdapter)_context).ObjectContext;
                    var members = context.CreateObjectSet<Shipment>().GetResultType()
                        .EdmType
                        .MetadataProperties
                        .First(p => p.Name == "KeyMembers")
                        .Value as IEnumerable<EdmMember>;

                    var query = from meta in members
                                let prop = (meta as EdmProperty)
                                let type = meta is EdmProperty ? (meta as EdmProperty).TypeUsage.EdmType : null
                                where meta is EdmProperty
                                select new
                                {
                                    Name = prop.Name,
                                    Type = type.Name
                                };

                    ret = new List<FieldDescriptor>();
                    foreach (var item in query)
                    {
                        FieldDescriptor desc = null;
                        for (int i = 0; i < _data.FieldCount; i++)
                        {
                            if (_data.GetName(i) == item.Name)
                            {
                                desc = new FieldDescriptor() { Name = item.Name, Ordinal = i, Type = item.Type };
                                ret.Add(desc);
                                break;
                            }
                        }

                        if (desc == null)
                            throw new InvalidOperationException(string.Format("Key {0} was not found in the database", item.Name));
                    }

                    _keys = ret.ToArray();
                }
                return _keys;
            }
        }

        public bool IsFieldKey(string Field)
        {
            return Keys.Any(t => t.Name == Field);
        }
    }
    
    [TestClass]
    public class ObjectGraphWalk
    {
        [TestInitialize]
        public void StartUp()
        {
            using (ShipmentContext context = new ShipmentContext())
            {
                context.Database.CreateIfNotExists();
            }
        }

        private ObjectIDGenerator gen = new ObjectIDGenerator();

        private Shipment GetTestShipment()
        {
            return new Shipment()
            {
                ShipmentId = 0,
                Weight = 3.14m,
                Description = "test shipment"
            };
        }

        private List<object> WalkObject(Object Input, ShipmentContext context, SqlCeResultSet resultSet)
        {
            bool firstOccurrence;
            gen.GetId(Input, out firstOccurrence);
            if (!firstOccurrence)
                return null;

            CachedTableInfo metadata = new CachedTableInfo(Input.GetType().Name, context, resultSet);

            List<string> processedFields = new List<string>();
            List<object> keys = new List<object>();
            SqlCeUpdatableRecord r = resultSet.CreateRecord();

            // Match the database with memory object.
            for (int i = 0; i < resultSet.FieldCount; i++)
            {
                if (!metadata.IsFieldKey(r.GetName(i)))
                {
                    string fieldName = resultSet.GetName(i);
                    PropertyInfo fi = Input.GetType().GetProperty(fieldName);
                    if (fi != null)
                    {
                        r[i] = fi.GetValue(Input, null);
                        processedFields.Add(fieldName);
                    }
                    else
                        throw new InvalidOperationException(string.Format("Field {0} was not found in the class {1}",
                            fi.Name, Input.GetType().Name));
                }
            }
            resultSet.Insert(r, DbInsertOptions.PositionOnInsertedRow);
            foreach (var item in metadata.Keys)
            {
                keys.Add(r[item.Name]);
            }

            // keys now contain assigned autoincrement fields.
            return keys;
        }

        [TestMethod]
        public void Walk_Simple_Object()
        {
            Shipment shipment = GetTestShipment();
            
            using (SqlCeConnection conn = new SqlCeConnection(@"Data Source=c:\temp\shipments.sdf"))
            using (ShipmentContext c = new ShipmentContext())
            {
                conn.Open();
                using (SqlCeCommand cmd = new SqlCeCommand("Shipments", conn))
                {
                    cmd.CommandType = System.Data.CommandType.TableDirect;

                    using (SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable))
                    {
                        List<object> keys = WalkObject(shipment, c, rs);
                    }
                }

                c.Shipments.Any(t => t.Description == "test shipment").Should().Be(true);
            }
        }
    }
}
