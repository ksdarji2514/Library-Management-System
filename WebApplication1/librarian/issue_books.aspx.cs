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
    public partial class issue_books : System.Web.UI.Page
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
            if (IsPostBack) return;
            enrno.Items.Clear();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select enrollment_no from student_registration";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                enrno.Items.Add(dr["enrollment_no"].ToString());
            }

            isbn.Items.Clear();
            isbn.Items.Add("Select");
            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "select books_isbn from books";
            cmd2.ExecuteNonQuery();
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);
            foreach(DataRow dr2 in dt2.Rows)
            {
                isbn.Items.Add(dr2["books_isbn"].ToString());
            }


        }

        protected void b1_Click(object sender, EventArgs e)
        {
            if (isbn.SelectedItem.ToString() == "Select") {
                Response.Write("<script>alert('please select the books');windows.location.href=windows.location.href;</script>");
  
            }
            else
            {
                int found = 0;
                SqlCommand cmd0 = con.CreateCommand();
                cmd0.CommandType = CommandType.Text;
                cmd0.CommandText = "select * from issue_books where student_enrollment_no='" + enrno.SelectedItem.ToString() + "'and books_isbn='" + isbn.SelectedItem.ToString() + "'and is_books_return='no'";
                cmd0.ExecuteNonQuery();
                DataTable dt0 = new DataTable();
                SqlDataAdapter da0 = new SqlDataAdapter(cmd0);
                da0.Fill(dt0);
                found = Convert.ToInt32(dt0.Rows.Count.ToString());
                if (found > 0)
                {
                    Response.Write("<script>alert('this book is already available with this student');</script>");

                }
                else
                {

                    if (instock.Text == "0")
                    {
                        Response.Write("<script>alert('this book is not available in stock');</script>");

                    }
                    else
                    {
                        string books_issues_date = DateTime.Now.ToString("yyyy/MM/dd");
                        string approx_return_date = DateTime.Now.AddDays(10).ToString("yyyy/MM/dd");
                        string username = "";
                        SqlCommand cmd2 = con.CreateCommand();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = "select * from student_registration where enrollment_no='" + enrno.SelectedItem.ToString() + "'";
                        cmd2.ExecuteNonQuery();
                        DataTable dt2 = new DataTable();
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        da2.Fill(dt2);
                        foreach (DataRow dr2 in dt2.Rows)
                        {
                            username = dr2["username"].ToString();
                        }
                        SqlCommand cmd3 = con.CreateCommand();
                        cmd3.CommandType = CommandType.Text;
                        cmd3.CommandText = "insert into issue_books values('" + enrno.SelectedItem.ToString() + "','" + isbn.SelectedItem.ToString() + "','" + books_issues_date.ToString() + "','" + approx_return_date.ToString() + "','" + username.ToString() + "','no','no')";
                        cmd3.ExecuteNonQuery();

                        SqlCommand cmd4 = con.CreateCommand();
                        cmd4.CommandType = CommandType.Text;
                        cmd4.CommandText = "update books set available_qty = available_qty-1 where books_isbn = '" + isbn.SelectedItem.ToString() + "'";
                        cmd4.ExecuteNonQuery();

                        Response.Write("<script>alert('books issues successfully');windows.location.href=windows.location.href;</script>");
                    }
                }
            }
        }
            protected void isbn_SelectedIndexChanged(object sender, EventArgs e)
        {
            i1.ImageUrl = "";
            booksname.Text = "";
            instock.Text = "";

            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "select * from books where books_isbn='"+isbn.SelectedItem.ToString()+"'";
            cmd2.ExecuteNonQuery();
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);
            foreach (DataRow dr2 in dt2.Rows)
            {
                i1.ImageUrl = dr2["books_image"].ToString();
                booksname.Text = dr2["books_title"].ToString();
                instock.Text = dr2["available_qty"].ToString();
            }

        }
    }
}