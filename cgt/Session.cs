using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace cgt
{
    public class SessionProcess
    {
        public static string ConnString = WebConfigurationManager.ConnectionStrings["cgt"].ToString();

        public static string GetUser(string SessionID, string Type)
        {
            string r = "";
            if (SessionID != null)
            {
                SqlConnection conn = new SqlConnection(ConnString);
                string sqlStr = "[dbo].[cgtUser_Get]";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.Parameters.Add("@Id", SqlDbType.VarChar);
                cmd.Parameters["@Id"].Value = SessionID;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar);
                cmd.Parameters["@Type"].Value = Type;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    r = (string)cmd.ExecuteScalar();

                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                    cmd.Dispose();
                }
            }
            return r;
        }

        public static string LoginUser(string User, string Password, string DepartmentID, string Computer, string IP, string MACAddress)
        {
            string _SessionID = "";
            SqlConnection conn = new SqlConnection(ConnString);
            string sqlStr = "[dbo].[cgtUser_Login]";
            using (SqlCommand cmd = new SqlCommand(sqlStr, conn))
            {
                cmd.Parameters.Add("@UserID", SqlDbType.VarChar);
                cmd.Parameters["@UserID"].Value = User;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar);
                cmd.Parameters["@Password"].Value = Password;
                cmd.Parameters.Add("@DepartmentID", SqlDbType.VarChar);
                cmd.Parameters["@DepartmentID"].Value = DepartmentID;
                cmd.Parameters.Add("@Computer", SqlDbType.VarChar);
                cmd.Parameters["@Computer"].Value = Computer;
                cmd.Parameters.Add("@IP", SqlDbType.VarChar);
                cmd.Parameters["@IP"].Value = IP;
                cmd.Parameters.Add("@MACAddress", SqlDbType.VarChar);
                cmd.Parameters["@MACAddress"].Value = MACAddress;

                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    _SessionID = (string)cmd.ExecuteScalar();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                    cmd.Dispose();
                }
            }
            return _SessionID;
        }

        public static string LogoutUser(string SessionID)
        {
            SessionID = null;
            return SessionID;
        }

        public static string GetRight(string ApplicationID, string PageAuthority, string SessionID, string Type)
        {
            string r = "0000";
            if (SessionID != null)
            {
                SqlConnection conn = new SqlConnection(ConnString);
                conn.Open();
                string sqlStr = "[dbo].[cgtUser_Right]";
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                cmd.Parameters.Add("@ApplicationID", SqlDbType.VarChar);
                cmd.Parameters["@ApplicationID"].Value = ApplicationID;
                cmd.Parameters.Add("@Page", SqlDbType.VarChar);
                cmd.Parameters["@Page"].Value = PageAuthority;
                cmd.Parameters.Add("@ID", SqlDbType.VarChar);
                cmd.Parameters["@ID"].Value = SessionID;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["UserID"].ToString() != "")
                    {
                        r = reader["Open"].ToString() + reader["Save"].ToString() + reader["Update"].ToString() + reader["Delete"].ToString();
                    }
                }
                reader.Dispose();
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
            }
            else
            {
                System.Web.HttpContext.Current.Server.Transfer("~\\Session\\Login.aspx");
            }
            return r;
        }

        public static bool isOpen(string appid, string page, string sessid)
        {
            bool r = false;
            int open = -1;
            string all = cgt.SessionProcess.GetRight(appid, page, sessid, "UNIFIED");
            try
            {
                open = int.Parse(all.Substring(0, 1));
            }
            catch(Exception)
            {
                open = -1;
            }
            if (open == 0)
            {
                System.Web.HttpContext.Current.Server.Transfer("~\\Session\\NoAccess.aspx");
            }
            else if (open==-1)
            {
                System.Web.HttpContext.Current.Server.Transfer("~\\Session\\Login.aspx");
            }
            return r;
        }
    }
}