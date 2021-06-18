using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace library_ms.librarian
{
    public partial class display_all_books : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Krina Darji\source\repos\WebApplication1\WebApplication1\App_Data\lms.mdf;Integrated Security=True");
        protected void Page_Load(object sender, EventArgs e)
        {

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            if (Session["librarian"] == null)
            {
                Response.Redirect("login.aspx");
            }

            int i = 0;
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from books";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            r1.DataSource = dt;
            r1.DataBind();

        }
        public string checkpdf(object myvalue1, object id1)
        {
            if (myvalue1 == "")
            {
                return myvalue1.ToString();
            }
            else
            {
                return "<a href = 'delete_files.aspx?id1=" + id1 + " ' style = 'color:red'>delete pdf</a>";
            }
        }
    }
}