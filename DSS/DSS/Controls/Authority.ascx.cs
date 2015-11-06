using System;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSS.DSS.Controls
{
    public partial class Authority : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _BTN_Enter.Click += new EventHandler(_BTN_Enter_Click);
        }

        public static string GetHashedPassword(string Password)
        {
            byte[] bPassword = Encoding.UTF8.GetBytes(Password);
            MD5 md5 = new MD5CryptoServiceProvider();
            return Encoding.UTF8.GetString(md5.ComputeHash(bPassword));
        }

        void _BTN_Enter_Click(object sender, EventArgs e)
        {
            HttpContext Context = HttpContext.Current;

            // Лезет в базу, смотрит юзера по мейлу и паролю
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_person_Read", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("Login", _TB_Login.Text);
                Command.Parameters.AddWithValue("Password", GetHashedPassword(_TB_Password.Text));
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                // Если нашла, давабляет UserID, PersonID и TaskID в сессию
                if (Reader.Read())
                {
                    Context.Response.Cookies.Add(new HttpCookie("UserID", Reader["id"].ToString()));
                    Context.Response.Cookies.Add(new HttpCookie("PersonID", "-1"));
                    if (Reader["task_id"] != DBNull.Value)
                        Context.Response.Cookies.Add(new HttpCookie("TaskID", Reader["task_id"].ToString()));
                }
            }

            Response.Redirect("/DSS/Default.aspx");
        }
    }
}