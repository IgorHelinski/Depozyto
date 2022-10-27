using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
namespace Depozyto.Controllers.Admin
{
    public class DeleteUserController : Controller
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
            public DeleteUserController(IConfiguration _configuration)
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
            public IActionResult Delete()
            {
                string deleteId;
                deleteId = TempData["DeleteUserId"].ToString();

                connectionString();
                con.Open();
                com.Connection = con;

                com.CommandText = "UPDATE Clients SET Blocked = '" + true + "' WHERE Email = '" + deleteId + "';";
                com.ExecuteNonQuery();
                con.Close();

                return RedirectToAction("Index", "Admin");
            }
        }
    }
