using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace CaptchaManagementSystemWebservice
{
    /// <summary>
    /// Summary description for CaptchaWebservice
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CaptchaWebservice : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        [WebMethod]
        public List<Captcha> GetAllCaptchas()
        {
            List<Captcha> captchas = new List<Captcha>();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM Captcha", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    captchas.Add(new Captcha
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Type = reader["Type"].ToString()
                    });
                }
            }
            return captchas;
        }

        [WebMethod]
        public string AddCaptcha(Captcha captcha)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    string query = @"INSERT INTO Captcha
                                     VALUES(@Name, @Description, @Type)";
                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", captcha.Name);
                    cmd.Parameters.AddWithValue("@Description", captcha.Description);
                    cmd.Parameters.AddWithValue("@Type", captcha.Type);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return "Captcha added successfully.";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [WebMethod]
        public void UpdateCaptcha(Captcha captcha)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string query = @"UPDATE Captcha 
                                 SET Name=@Name, Description=@Description, Type=@Type 
                                 WHERE Id=@Id";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", captcha.Id);
                cmd.Parameters.AddWithValue("@Name", captcha.Name);
                cmd.Parameters.AddWithValue("@Description", captcha.Description);
                cmd.Parameters.AddWithValue("@Type", captcha.Type);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        [WebMethod]
        public void DeleteCaptcha(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                var cmd = new MySqlCommand("DELETE FROM Captcha WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        [WebMethod]
        public Captcha GetCaptchaById(int id)
        {
            Captcha captcha = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                var cmd = new MySqlCommand("SELECT * FROM Captcha WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    captcha = new Captcha
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Type = reader["Type"].ToString()
                    };
                }
            }
            return captcha;
        }
        public class Captcha
        {


            [Key]
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }

        }

    }
}
