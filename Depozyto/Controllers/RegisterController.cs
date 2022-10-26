using Depozyto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace Depozyto.Controllers
{
    public class RegisterController : Controller
    {

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        bool canRegister;

        private IConfiguration Configuration;
        public RegisterController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        //ta funkcja szyfruje haslo
        //string EncodePasswordToBase64(RegisterModel reg)
        //{
        //    try
        //    {
        //        byte[] encData_byte = new byte[reg.DePassword.Length];
        //        encData_byte = System.Text.Encoding.UTF8.GetBytes(reg.DePassword);
        //        reg.Password = Convert.ToBase64String(encData_byte);
        //        return reg.Password;
        //    }
        //    catch (Exception ex)
        //    {
        //       throw new Exception("Error in base64Encode" + ex.Message);
        //    }
        //}

        //Strona rejestracji
        public ActionResult Register()
        {
            return View();
        }

        //sprawdza czy mozemy zapisac uzytkownika w bazie danych (czy nie ma tos takiego loginu itp)
        void CheckRegister(RegisterModel reg)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Clients where Login='" + reg.Login + "' or Email ='" + reg.Email + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                //jest taki urzytkownik (nie mozna zarejestrowac)
                canRegister = false;
            }
            else
            {
                con.Close();
                //nie ma takiego urzttkownika (mozna zarejestrowac)
                canRegister = true;
            }
        }

        //Tutaj zapisujemy dane na bazie danych
        [HttpPost]
        public ActionResult CreateAccount(RegisterModel reg) //Rejestracja
        {
            //sprawdzamy czy mozemy zarejestrowac nowego klienta
            //EncodePasswordToBase64(reg);
            CheckRegister(reg);

            if (canRegister)
            {
                connectionString();
                SqlCommand com = new SqlCommand("SP_Client", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Login", reg.Login);
                com.Parameters.AddWithValue("@Haslo", EncriptController.Encrypt(reg.Password));
                com.Parameters.AddWithValue("@Imie", reg.Name);
                com.Parameters.AddWithValue("@Nazwisko", reg.LastName);
                com.Parameters.AddWithValue("@Email", reg.Email);
                com.Parameters.AddWithValue("@Role", "User");
                com.Parameters.AddWithValue("@Blocked", 0);
                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {




                    //Success
                    return View();
                }
                else
                {
                    //Failed
                    return View("Register");
                }
            }
            else
            {
                //nie moze zarejestrowac
                ViewData["LoginDouble"] = "Login lub Email jest już zajęty przez innego użytkownika";
                return View("Register");
            }

        }
    }
}
