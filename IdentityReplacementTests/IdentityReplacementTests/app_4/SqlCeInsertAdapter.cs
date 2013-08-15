using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Reflection;
using VTechnologies.ShipGear.Entities;

namespace app_4
{
    public class SqlCeInsertAdapterShipment
    {
        /// <summary>
        /// Returns foreign key ordinal in the type.
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Parent"></param>
        /// <returns></returns>
        private static int GetOrdinal(SqlCeUpdatableRecord DetailRecord, Type Type, Type Parent)
        {
            for (int i = 0; i < DetailRecord.FieldCount; i++)
			{
                string fieldName = DetailRecord.GetName(i).ToUpper();
                if (fieldName == (Parent.Name + "_Id").ToUpper()
                    || fieldName == (Parent.Name + "Id").ToUpper())
                {
                    return i;
                }
			} 
            
            throw new InvalidOperationException(string.Format("Ordinal for detail {0} was not found in {1}",
                Type.Name, Parent.Name));
        }

        private void FillInProperty(PropertyInfo property, int ordinal, SqlCeUpdatableRecord record, object value)
        {
            if (value.GetType() == typeof(bool)) record.SetBoolean(ordinal, (bool)value);
            else if (value.GetType() == typeof(Int16)) record.SetInt16(ordinal, (Int16)value);
            else if (value.GetType() == typeof(Int32)) record.SetInt32(ordinal, (Int32)value);
            else if (value.GetType() == typeof(Int16)) record.SetInt64(ordinal, (Int16)value);
        }
        
        private void FillInShipment(Shipment shipment, SqlCeUpdatableRecord record)
        {
            foreach (PropertyInfo item in shipment.GetType().GetProperties())
	        {
                /*item.SetValue(shipment, item.  null);
                FillInProperty<item.GetType()>
                item.Name */
	        } 
        }

        /// <summary>
        /// Returns generated Id.
        /// </summary>
        /// <returns></returns>
        public static Int32 Write_Shipment(Shipment shipment, SqlCeConnection conn)
        {
            Dictionary<string, Int32> idBag = new Dictionary<string, int>();

            // Shipment
            using (SqlCeCommand cmd = new SqlCeCommand(shipment.GetType().Name, conn))
            {
                cmd.CommandType = System.Data.CommandType.TableDirect;
                using (SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable))
                {
                    SqlCeUpdatableRecord r = rs.CreateRecord();

                    /*r.SetInt32(1, shipment.SourceId);
                    r.SetInt32(2, shipment.CompanyId);
                    r.SetInt32(3, shipment.NumberOfPackages);
                    r.SetBoolean(4, shipment.RateRuleApplied);
                    if (shipment.ShipDate.HasValue)
                        r.SetDateTime(5, shipment.ShipDate.Value);
                    r.SetString(6, shipment.Workstation); */

                    
                    rs.Insert(r, DbInsertOptions.PositionOnInsertedRow);
                    idBag[shipment.GetType().Name] = rs.GetInt32(0);
                }
            }

            /*foreach (var level2 in shipment.levels)                                         // Payload
            {
                // Level2
                using (SqlCeCommand cmd = new SqlCeCommand(level2.GetType().Name, conn))
                {
                    cmd.CommandType = System.Data.CommandType.TableDirect;
                    using (SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable))
                    {
                        SqlCeUpdatableRecord r = rs.CreateRecord();
                        r.SetInt32(GetOrdinal(r, level2.GetType(), shipment.GetType()), idBag[shipment.GetType().Name]);        // foreign key
                        r.SetString(1, level2.Value);                                                                       // payload
                        rs.Insert(r, DbInsertOptions.PositionOnInsertedRow);
                        idBag[level2.GetType().Name] = rs.GetInt32(0);
                    }
                }

                foreach (var level3 in level2.levels)
                {
                    using (SqlCeCommand cmd = new SqlCeCommand(level3.GetType().Name, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.TableDirect;
                        using (SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable))
                        {
                            SqlCeUpdatableRecord r = rs.CreateRecord();
                            r.SetInt32(GetOrdinal(r, level3.GetType(), level2.GetType()), idBag[level2.GetType().Name]);
                            r.SetString(1, level3.Value);
                            rs.Insert(r, DbInsertOptions.PositionOnInsertedRow);
                            idBag[level3.GetType().Name] = rs.GetInt32(0);
                        }
                    }

                    foreach (var level4 in level3.levels)
                    {
                        using (SqlCeCommand cmd = new SqlCeCommand(level4.GetType().Name, conn))
                        {
                            cmd.CommandType = System.Data.CommandType.TableDirect;
                            using (SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable))
                            {
                                SqlCeUpdatableRecord r = rs.CreateRecord();
                                r.SetInt32(GetOrdinal(r, level4.GetType(), level3.GetType()), idBag[level3.GetType().Name]);
                                r.SetString(1, level4.Value);
                                rs.Insert(r, DbInsertOptions.PositionOnInsertedRow);
                                idBag[level4.GetType().Name] = rs.GetInt32(0);
                            }
                        }

                        foreach (var level5 in level4.levels)
                        {
                            using (SqlCeCommand cmd = new SqlCeCommand(level5.GetType().Name, conn))
                            {
                                cmd.CommandType = System.Data.CommandType.TableDirect;
                                using (SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable))
                                {
                                    SqlCeUpdatableRecord r = rs.CreateRecord();
                                    r.SetInt32(GetOrdinal(r, typeof(Level5), typeof(Level4)), idBag[level4.GetType().Name]);
                                    r.SetString(1, level5.Value);
                                    rs.Insert(r, DbInsertOptions.PositionOnInsertedRow);
                                }
                            }
                        }
                    }
                }
            } */
            
            return idBag[shipment.GetType().Name];
        }
    }
}
