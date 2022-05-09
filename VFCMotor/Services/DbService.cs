using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFCMotor.Models;

namespace VFCMotor.Services
{
   public class DbService
    {
       static string sFilePath = Environment.CurrentDirectory + "\\" + "capactor.sqlite";

        public static  void Connect()
        {
            if (!System.IO.File.Exists(sFilePath))
            {
              
                SQLiteConnection.CreateFile(sFilePath);

                using (var sqlite2 = new SQLiteConnection(@"Data Source=" + sFilePath))
                {
                    sqlite2.Open();
                    string sql = "create table capactor (" +
                        "C1_Set REAL, " +
                        "C2_Set REAL," +
                        "C1_Read REAL, " +
                        "C2_Read REAL, " +
                        "ADC0 REAL," +
                        "ADC1 REAL," +
                        "ADC2 REAL," +
                        "ADC3 REAL," +
                         "RegisterDate TEXT" +
                        ")";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite2);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void AddLog(Capacitance capacitance)
        {
            try
            {
                using (var m_dbConnection = new SQLiteConnection(@"Data Source=" + sFilePath))
                {
                    m_dbConnection.Open();
                    string sql = "insert into capactor (C1_Set, C2_Set,C1_Read,C2_Read,ADC0,ADC1,ADC2,ADC3,RegisterDate) " +
                        " values ('" + capacitance.C1_Set + "','" + capacitance.C2_Set + "','"
                        + capacitance.C1_Read + "','" + capacitance.C2_Read + "','" + capacitance.ADC0 + "','" + capacitance.ADC1 + "','"
                        + capacitance.ADC2 + "','" + capacitance.ADC3 + "','" + capacitance.RegisterDate + "')";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
            }
          
        }

        public static List<T> CreateList<T>(params T[] elements)
        {
            return new List<T>(elements);
        }
        public static List<Capacitance> GetLog(List<string> columns,DateTime date1, DateTime date2)
        {
            try
            {
                using (var m_dbConnection = new SQLiteConnection(@"Data Source=" + sFilePath))
                {
                    string sql = "SELECT ";
                    for (int i = 0; i < columns.Count; i++)
                        sql += columns[i] + ",";
                    sql = sql.TrimEnd(',');
                    sql += " FROM capactor WHERE RegisterDate BETWEEN @startDate AND @endDate";
                    List<Capacitance> capacitances = new List<Capacitance>();
                    m_dbConnection.Open();
                    SQLiteCommand cmd = m_dbConnection.CreateCommand();
                    cmd.CommandText = sql; //"SELECT C1_Set FROM capactor WHERE RegisterDate BETWEEN @startDate AND @endDate";
                    cmd.Parameters.AddWithValue("@startDate", date1.ToString("yyyy-MM-dd HH:hh:mm:ss"));
                    cmd.Parameters.AddWithValue("@endDate", date2.ToString("yyyy-MM-dd HH:hh:mm:ss"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            capacitances.Add(new Capacitance()
                            {
                                C1_Set = columns.IndexOf("C1_Set") > -1 ? (double)reader["C1_Set"] : 0,
                                C2_Set =columns.IndexOf("C2_Set") > -1? (double)reader["C2_Set"] : 0,
                                C1_Read= columns.IndexOf("C1_Read") > -1 ? (double)reader["C1_Read"] : 0,
                                C2_Read = columns.IndexOf("C2_Read") > -1 ? (double)reader["C2_Read"] : 0,
                                ADC0 = columns.IndexOf("ADC0") > -1 ? (double)reader["ADC0"] : 0,
                                ADC1 = columns.IndexOf("ADC1") > -1 ? (double)reader["ADC1"] : 0,
                                ADC2 = columns.IndexOf("ADC2") > -1 ? (double)reader["ADC2"] : 0,
                                ADC3 = columns.IndexOf("ADC3") > -1 ? (double)reader["ADC3"] : 0,
                                RegisterDate=(string)reader["RegisterDate"]
                            });

                            //for (int i = 0; i < reader.FieldCount; i++)
                            //{

                            //    //ReportData v = new ReportData() { name = reader.GetName(i), value = reader.GetValue(i).ToString() };
                            //    //capacitances.Add(v);
                            //}
                        }

                    }

                    return capacitances;
                }
            }
            catch (Exception ex)
            {

                LogService.LogMessage(ex.Message);
                return null;
            }
       
        }

        public static bool ClearDb()
        {
            using (SQLiteConnection sqlConn = new SQLiteConnection(@"Data Source=" + sFilePath))
            {
                sqlConn.Open();
                SQLiteCommand cmd = sqlConn.CreateCommand();
                cmd.CommandText = "DELETE FROM capactor ";
                int rows= cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }


    }
}
