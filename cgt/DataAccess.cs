using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace cgt
{
    public class DataAccess
    {
        public static string serverid = ConfigurationManager.AppSettings["ServerID"];

        public static string ProsesData(string[] user, string[] type, string[] mkey, string[] dkey, string[] field, string[] data)
        {
            string r = "";
            string cstr = serverid+";Initial Catalog=" + type[0] + ";";
            using (SqlConnection conn = new SqlConnection(cstr))
            {
                conn.Open();
                string sqlStr = "Transaction_Process";
                using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameterCollection spc = cmd.Parameters;
                    spc.AddWithValue("@UserId", user[0]);
                    spc.AddWithValue("@DepartmentId", user[1]);
                    spc.AddWithValue("@Factory", user[2]);
                    spc.AddWithValue("@Table", type[1]);
                    spc.AddWithValue("@TableType", type[2]);
                    spc.AddWithValue("@ProcessType", type[3]);
                    spc.AddWithValue("@MasterKey", mkey[0]);
                    spc.AddWithValue("@MasterKey2", mkey[1]);
                    spc.AddWithValue("@DetailKey", dkey[0]);
                    spc.AddWithValue("@DetailKey2", dkey[1]);
                    spc.AddWithValue("@DetailKey3", dkey[2]);

                    int s = 0, e = 0;
                    e = field.Length;

                    while (e > s)
                    {
                        try
                        {
                            if (!(data[s].ToString() == "   ") && (data[s].ToString().Trim() == "" || data[s].Trim() == "__/__/____")) // set null for empty spaces
                                data[s] = null;
                            else
                                if (field[s] != null && field[s].Substring(0, 2) == "N_") // Numeric
                                    spc.AddWithValue(field[s].Substring(2, field[s].Length - 2), Tools.StringDecimalIDToUS(data[s]));
                                else if (field[s] != null && field[s].Substring(0, 2) == "D_") // DateTime
                                    spc.AddWithValue(field[s].Substring(2, field[s].Length - 2), Convert.ToDateTime(data[s]).ToString("yyyy/MM/dd"));
                                else if (field[s] != null)
                                {
                                    string str;
                                    str = data[s].Replace("'", "''");
                                    str = Regex.Replace(str, " +( |$)", "$1");
                                    spc.AddWithValue(field[s], str);
                                }
                        }
                        catch (Exception ex)
                        {
                            r = ex.Message.ToString();
                            if (r.IndexOf('\r') > 0)
                                r = r.Substring(0, r.IndexOf('\r'));
                            return r;
                        }
                        s += 1;
                    }
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException sqle)
                    {
                        r = sqle.Message.ToString();
                        if (r.IndexOf('\r') > 0)
                            r = r.Substring(0, r.IndexOf('\r'));
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return r.Replace("'", "");
        }

        public static DataTable RetriveData(string[] user, string[] tabel, string[] key)
        {
            DataTable table = new DataTable();
            string cstr = serverid + ";Initial Catalog=" + tabel[0] + ";";

            using (SqlConnection conn = new SqlConnection(cstr))
            {
                conn.Open();
                string sqlStr = "Transaction_Get";

                using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameterCollection spc = cmd.Parameters;
                    spc.AddWithValue("@UserID", user[0]);
                    spc.AddWithValue("@DepartmentID", user[1]);
                    spc.AddWithValue("@Factory", user[2]);
                    spc.AddWithValue("@Table", tabel[1]);
                    spc.AddWithValue("@Type", tabel[2]);
                    spc.AddWithValue("@Field", tabel[3]);

                    if (key[0] != null) spc.AddWithValue("@Key1", key[0]);
                    if (key[1] != null) spc.AddWithValue("@Key2", key[1]);
                    if (key[2] != null) spc.AddWithValue("@Key3", key[2]);
                    if (key[3] != null) spc.AddWithValue("@Key4", key[3]);
                    if (key[4] != null) spc.AddWithValue("@Key5", key[4]);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(table);
                    cmd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
            return table;
        }

        public static DataTable RetriveDataEx(string[] user, string[] tabel, string[] key)
        {
            DataTable table = new DataTable();
            string cstr = serverid + ";Initial Catalog=" + tabel[0] + ";";

            using (SqlConnection conn = new SqlConnection(cstr))
            {
                conn.Open();
                string sqlStr = "Transaction_Get";

                using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameterCollection spc = cmd.Parameters;
                    spc.AddWithValue("@UserID", user[0]);
                    spc.AddWithValue("@DepartmentID", user[1]);
                    spc.AddWithValue("@Factory", user[2]);
                    spc.AddWithValue("@Table", tabel[1]);
                    spc.AddWithValue("@Type", tabel[2]);
                    spc.AddWithValue("@Field", tabel[3]);

                    if (key[0] != null) spc.AddWithValue("@Key1", key[0]);
                    if (key[1] != null) spc.AddWithValue("@Key2", key[1]);
                    if (key[2] != null) spc.AddWithValue("@Key3", key[2]);
                    if (key[3] != null) spc.AddWithValue("@Key4", key[3]);
                    if (key[4] != null) spc.AddWithValue("@Key5", key[4]);
                    try { if (key[5] != null) spc.AddWithValue("@Date1", Convert.ToDateTime(key[5]).ToString("yyyy/MM/dd"));} catch (Exception) { };
                    try { if (key[6] != null) spc.AddWithValue("@Date2", Convert.ToDateTime(key[6]).ToString("yyyy/MM/dd"));} catch (Exception) { };
                    if (key[7] != null) spc.AddWithValue("@Int1", Tools.StringDecimalIDToUS(key[7]));
                    if (key[8] != null) spc.AddWithValue("@Int2", Tools.StringDecimalIDToUS(key[8]));
                    if (key[9] != null) spc.AddWithValue("@Int3", Tools.StringDecimalIDToUS(key[9]));


                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(table);
                    cmd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
            return table;
        }

        public static DataTable RetriveDataReport(string SPName, string[] key)
        {
            DataTable table = new DataTable();
            string cstr = serverid + ";Initial Catalog=SBR;";

            using (SqlConnection conn = new SqlConnection(cstr))
            {
                conn.Open();
                string sqlStr = SPName;

                using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameterCollection spc = cmd.Parameters;
                    if(key[0] != null)
                    spc.AddWithValue("@D1", Convert.ToDateTime(key[0]).ToString("yyyy/MM/dd"));
                    if(key[1] != null)
                    spc.AddWithValue("@D2", Convert.ToDateTime(key[1]).ToString("yyyy/MM/dd"));
                    if(key[2] != null)
                    spc.AddWithValue("@S1", key[2]);
                    if(key[3] != null)
                    spc.AddWithValue("@S2", key[3]);
                    if(key[4] != null)
                    spc.AddWithValue("@S3", key[4]);
                    if(key[5] != null)
                    spc.AddWithValue("@S4", key[5]);
                    if(key[6] != null)
                    spc.AddWithValue("@S5", key[6]);
                    if(key[7] != null)
                    spc.AddWithValue("@S6", key[7]);
                    if(key[8] != null)
                    spc.AddWithValue("@S7", key[8]);
                    if(key[9] != null)
                    spc.AddWithValue("@S8", key[9]);
                    if(key[10] != null)
                    spc.AddWithValue("@S9", key[10]);
                    
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(table);
                    cmd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
            return table;
        }

        public static DataTable LookData(string[] tabel, string[] key)
        {
            DataTable table = new DataTable();
            string cstr = serverid + ";Initial Catalog=" + tabel[0] + ";";

            using (SqlConnection conn = new SqlConnection(cstr))
            {
                conn.Open();
                string sqlStr = "Transaction_Look";
                using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameterCollection spc = cmd.Parameters;
                    spc.AddWithValue("@Table", tabel[1]);
                    if (key[0] != null) spc.AddWithValue("@Key1", key[0]);
                    if (key[1] != null) spc.AddWithValue("@Key2", key[1]);
                    if (key[2] != null) spc.AddWithValue("@Key3", key[2]);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(table);
                    cmd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
            return table;
        }

        public static string CheckData(string Table, string TableType, string key1, string key2, string key3, string key4, string key5, string key6)
        {
            string result = "";
            using (SqlConnection conn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["sbr"].ToString()))
            {
                conn.Open();
                string sqlStr = "Transaction_Check";
                using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameterCollection spc = cmd.Parameters;
                    spc.AddWithValue("@Table", Table);
                    spc.AddWithValue("@Type", TableType);
                    spc.AddWithValue("@Key1", key1);
                    spc.AddWithValue("@Key2", key2);
                    spc.AddWithValue("@Key3", key3);
                    spc.AddWithValue("@Key4", key4);
                    spc.AddWithValue("@Key5", key5);
                    spc.AddWithValue("@Key6", key6);

                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["Valid"].ToString() != "")
                                result = reader["Valid"].ToString();
                            else
                                result = "";
                        }
                        reader.Dispose();
                    }
                    catch (SqlException sqle)
                    {
                        result = sqle.Message.ToString();
                        if (result.IndexOf('\r') > 0)
                            result = (result.Substring(0, result.IndexOf('\r')));
                    }
                    cmd.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
            return result.Replace("'", "");
        }
    }
}