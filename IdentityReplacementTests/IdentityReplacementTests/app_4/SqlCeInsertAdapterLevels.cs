using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Reflection;

namespace app_4
{
    public class SqlCeInsertAdapterLevels
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

        /// <summary>
        /// Returns generated Id.
        /// </summary>
        /// <returns></returns>
        public static Int32 Write_Level1(Level1 level1, SqlCeConnection conn)
        {
            Dictionary<string, Int32> idBag = new Dictionary<string, int>();

            // Level1
            using (SqlCeCommand cmd = new SqlCeCommand(level1.GetType().Name, conn))
            {
                cmd.CommandType = System.Data.CommandType.TableDirect;
                using (SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable))
                {
                    SqlCeUpdatableRecord r = rs.CreateRecord();
                    r.SetString(1, level1.Value);                                         // Payload
                    rs.Insert(r, DbInsertOptions.PositionOnInsertedRow);
                    idBag[level1.GetType().Name] = rs.GetInt32(0);
                }
            }

            foreach (var level2 in level1.levels)                                         // Payload
            {
                // Level2
                using (SqlCeCommand cmd = new SqlCeCommand(level2.GetType().Name, conn))
                {
                    cmd.CommandType = System.Data.CommandType.TableDirect;
                    using (SqlCeResultSet rs = cmd.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable))
                    {
                        SqlCeUpdatableRecord r = rs.CreateRecord();
                        r.SetInt32(GetOrdinal(r, level2.GetType(), level1.GetType()), idBag[level1.GetType().Name]);        // foreign key
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
            }
            
            return idBag[level1.GetType().Name];
        }
    }
}
