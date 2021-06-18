using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net;
using System.IO;


namespace WebApplication1.student
{
    public partial class student_registration : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\lms.mdf;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
        }
        public class Class1
        {

            public static string GetRandomPassword(int length)
            {
                char[] chars = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                string password = string.Empty;
                Random random = new Random();

                for (int i = 0; i < length; i++)
                {
                    int x = random.Next(1, chars.Length);
                    //For avoiding Repetation of Characters
                    if (!password.Contains(chars.GetValue(x).ToString()))
                        password += chars.GetValue(x);
                    else
                        i = i - 1;
                }
                return password;
            }

        }

        protected void b1_Click(object sender, EventArgs e)
        {
            int count = 0;
            int count2 = 0;
            if (IsReCaptchValid())
            {
                SqlCommand cmd1 = con.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "select * from student_registration where enrollment_no = '" + enrollmentno.Text + "'";
                cmd1.ExecuteNonQuery();
                DataTable dt1 = new DataTable();
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(dt1);
                count = Convert.ToInt32(dt1.Rows.Count.ToString());

                if (count > 0)
                {
                    Response.Write("<script>alert('this enrollment no is already registered'); </script>");

                }
                else
                {
                    //this is for checking username unique
                    SqlCommand cmd2 = con.CreateCommand();
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandText = "select * from student_registration where username = '" + username.Text + "'";
                    cmd2.ExecuteNonQuery();
                    DataTable dt2 = new DataTable();
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    da2.Fill(dt2);
                    count = Convert.ToInt32(dt1.Rows.Count.ToString());

                    if (count2 > 0)
                    {
                        Response.Write("<script>alert('this username is already registered'); </script>");
                    }
                    else
                    {

                        //end here username checking 
                        string randomno = Class1.GetRandomPassword(10) + ".jpg";
                        string Path = "";
                        f1.SaveAs(Request.PhysicalApplicationPath+"/student/student_img/"+randomno.ToString());
                        Path = "/student/student_img/"+randomno.ToString();

                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into student_registration  values('" + firstname.Text + "','" + lastname.Text + "','" + enrollmentno.Text + "','" + username.Text + "','" + password.Text + "','" + email.Text + "','" + contact.Text + "','" +Path.ToString()+ "','no')";
                        cmd.ExecuteNonQuery();

                        Response.Write("<script>alert('record inserted successfully'); </script>");
                    }
                }
            }
            else
            {
                lblMessage1.Text = "this is invalid";
            }
        }

        public bool IsReCaptchValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6Lc_OngaAAAAAN5UMpYfrbJpRT6qWYCs0BIG5DZm";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }
    }
}