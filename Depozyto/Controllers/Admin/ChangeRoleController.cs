using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace Depozyto.Controllers.Admin
{
    public class ChangeRoleController : Controller
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
        public ChangeRoleController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = Configuration.GetConnectionString("ConString");
        }
        public IActionResult ChangeRole(string id)
        {
            TempData["Route"] = id;
            return View();
        }

        public IActionResult Change(AdminModel adm)
        {
            string changeEmail;
            changeEmail = TempData["Route"].ToString();

            connectionString();
            con.Open();
            com.Connection = con;

            com.CommandText = "UPDATE Clients SET Role = '" + adm.role + "' WHERE Email = '" + changeEmail + "';";
            com.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("Index", "Admin");
        }
    }
}
