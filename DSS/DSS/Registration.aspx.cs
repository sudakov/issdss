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

namespace DSS.DSS
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _BTN_Submit.Click += new EventHandler(_BTN_Submit_Click);
        }

        public static string GetHashedPassword(string Password)
        {
            byte[] bPassword = Encoding.UTF8.GetBytes(Password);
            MD5 md5 = new MD5CryptoServiceProvider();
            return Encoding.UTF8.GetString(md5.ComputeHash(bPassword));
        }

        void _BTN_Submit_Click(object sender, EventArgs e)
        {
            string UserID = null;
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DSSConnectionString"].ConnectionString))
            {
                SqlCommand Command = new SqlCommand("dbo.issdss_person_Insert", Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("Name", _TB_Name.Text);
                Command.Parameters.AddWithValue("Login", _TB_Login.Text);
                Command.Parameters.AddWithValue("Password", GetHashedPassword(_TB_Password.Text));
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    UserID = Reader["ID"].ToString();
                }
            }
            Context.Response.Cookies.Add(new HttpCookie("UserID", UserID));
            Response.Redirect("/DSS/Default.aspx");
        }
    }
}