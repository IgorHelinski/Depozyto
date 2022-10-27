using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace Depozyto.Controllers.Admin
{
    public class EditController : Controller
    {
        public static IList<HistoryModel> history = new List<HistoryModel>()
        {
            //new AccountModel(){ num = "dfasj", money = "fjands", ownerEmail = "fdnjbask"}
        };
        public static IList<AccountModel> accounts = new List<AccountModel>()
        {
            //new AccountModel(){ num = "dfasj", money = "fjands", ownerEmail = "fdnjbask"}
        };
        public static IList<UserModel> users = new List<UserModel>()
        {
            //new AccountModel(){ num = "dfasj", money = "fjands", ownerEmail = "fdnjbask"}
        };
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        private IConfiguration Configuration;
        public EditController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = Configuration.GetConnectionString("ConString");
        }
        public IActionResult DeleteUser(string id)
        {
            TempData["DeleteUserId"] = id;
            return View();
        }
        public IActionResult Edit(string Id)
        {
            //string EnPassword;
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Select * from dbo.Clients where Email = '" + Id + "';";

            dr = com.ExecuteReader();
            if (dr.Read())
            {
                ViewData["Login"] = dr["Login"].ToString();
                ViewData["Imie"] = dr["Imie"].ToString();
                ViewData["Nazwisko"] = dr["Nazwisko"].ToString();
                // EnPassword = dr["Haslo"].ToString();

            }
            else
            {
                // EnPassword = "byleco";
            }

            //string Password = EncriptController.Decrypt(EnPassword);

            //ViewData["Haslo"] = Password;
            TempData["EditID"] = Id;
            return View();
        }
        public IActionResult EditUser(AdminModel adm)
        {
            string changeEmail;
            changeEmail = TempData["EditID"].ToString();
            // adm.password = EncriptController.Encrypt(adm.password);
            connectionString();
            con.Open();
            com.Connection = con;

            com.CommandText = "UPDATE Clients SET Imie = '" + adm.name + "' WHERE Email = '" + changeEmail + "';";
            com.ExecuteNonQuery();
            com.CommandText = "UPDATE Clients SET Nazwisko = '" + adm.surname + "' WHERE Email = '" + changeEmail + "';";
            com.ExecuteNonQuery();
            com.CommandText = "UPDATE Clients SET Login = '" + adm.login + "' WHERE Email = '" + changeEmail + "';";
            com.ExecuteNonQuery();
            //com.CommandText = "UPDATE Clients SET Haslo = '" + adm.password + "' WHERE Email = '" + changeEmail + "';";
            //com.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("Index", "Admin");
        }
    }
}
