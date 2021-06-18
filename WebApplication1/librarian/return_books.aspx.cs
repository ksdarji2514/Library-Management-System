using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.librarian
{
    public partial class return_books : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\lms.mdf;Integrated Security=True");

        string penalty = "0";
        double noofdays;
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


            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "select * from penalty";
            cmd2.ExecuteNonQuery();
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);

            foreach (DataRow dr2 in dt2.Rows)
            {
                penalty = dr2["penalty"].ToString();
            }

            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("student_enrollment_no");
            dt.Columns.Add("books_isbn");
            dt.Columns.Add("books_issue_date");
            dt.Columns.Add("books_approx_return_date");
            dt.Columns.Add("student_username");
            dt.Columns.Add("is_books_return");
            dt.Columns.Add("books_returned_date");
            dt.Columns.Add("lateday");
            dt.Columns.Add("penalty");

            SqlCommand cmd1 = con.CreateCommand();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from issue_books where is_books_return= 'no'";
            cmd1.ExecuteNonQuery();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            da1.Fill(dt1);

            foreach (DataRow dr1 in dt1.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = dr1["id"].ToString();
                dr["student_enrollment_no"] = dr1["student_enrollment_no"].ToString();
                dr["books_isbn"] = dr1["books_isbn"].ToString();
                dr["books_issue_date"] = dr1["books_issue_date"].ToString();
                dr["books_approx_return_date"] = dr1["books_approx_return_date"].ToString();
                dr["student_username"] = dr1["student_username"].ToString();
                dr["is_books_return"] = dr1["is_books_return"].ToString();
                dr["books_returned_date"] = dr1["books_returned_date"].ToString();
                
                //calculate late days
                DateTime d1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                DateTime d2 = Convert.ToDateTime(dr1["books_approx_return_date"].ToString());

                if (d1 > d2)
                {
                    TimeSpan t = d1 - d2;
                    noofdays = t.TotalDays;
                    dr["lateday"] = noofdays.ToString();

                }
                else
                {
                    dr["lateday"] = "0";
                }

                dr["penalty"] = Convert.ToString(Convert.ToDouble(noofdays) * Convert.ToDouble(penalty));
                dt.Rows.Add(dr);

            }
            d1.DataSource = dt;
            d1.DataBind();
        }
    }
}