using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using Depozyto.Models;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Depozyto.Controllers
{
    public class LoginController : Controller
    {

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public LoginController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        //ta funkcja szyfruje haslo
        //string Encode2PasswordToBase64(LoginModel log)
        //{
        //    try
        //    {
        //        byte[] encData_byte = new byte[log.EnPassword.Length];
        //        encData_byte = System.Text.Encoding.UTF8.GetBytes(log.EnPassword);
        //       log.Password = Convert.ToBase64String(encData_byte);
        //        return log.Password;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error in base64Encode" + ex.Message);
        //    }
        //}

        [HttpGet]
        public IActionResult Index(UserModel usr) //Strona z logowaniem
        {
            return View();
        }

        //Logowanie
        public async Task<IActionResult> Index(LoginModel log, UserModel usr)
        {
            log.Password = EncriptController.Encrypt(log.Password);
            //sprawdza czy w bazie danych jest taki urzytkownik
            //Encode2PasswordToBase64(log);
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Clients where Login='" + log.Login + "' and Haslo='" + log.Password + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                usr.Login = dr["Login"].ToString();
                usr.Password = dr["Haslo"].ToString();
                usr.Name = dr["Imie"].ToString();
                usr.LastName = dr["Nazwisko"].ToString();
                usr.Email = dr["Email"].ToString();
                usr.Role = dr["Role"].ToString();
                
                //znalazl urzytkownika

                log.loggedIn = true;
                usr.loggedIn = true;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,usr.Email),
                    new Claim(ClaimTypes.Name,usr.Name),
                    new Claim(ClaimTypes.Surname,usr.LastName),
                    new Claim(ClaimTypes.Role,usr.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new System.Security.Claims.ClaimsPrincipal(claimsIdentity));

                if (dr["Blocked"].ToString() == "True")
                {
                    con.Close();
                    Response.Cookies.Delete("sesja");
                    ViewData["UserBlocked"] = "Użytkownik został zablokowany";
                    return View();
                }

                con.Close();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //nie znalazl urzytkownika
                con.Close();
                log.loggedIn = false;
                ViewData["LoginFlag"] = "Nieprawidłowy login lub hasło!!!";
                return View();
            }
        }

        public IActionResult Logout() //Wylogowanie
        {
            Response.Cookies.Delete("sesja");

            return RedirectToAction("Index", "Home");
        }
    }
}
