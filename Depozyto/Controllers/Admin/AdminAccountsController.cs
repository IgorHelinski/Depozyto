using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace Depozyto.Controllers.Admin
{
    public class AdminAccountsController : Controller
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
        public AdminAccountsController(IConfiguration _configuration)
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
        [Authorize(Roles = "Admin")]
        public IActionResult Accounts()
        {
            accounts.Clear();

            //get all users accounts and put them in list
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Accounts;";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                accounts.Add(new AccountModel
                {
                    num = dr["num"].ToString(),
                    money = dr["money"].ToString(),
                    ownerEmail = dr["ownerEmail"].ToString()
                });

                while (dr.Read())
                {

                    accounts.Add(new AccountModel
                    {
                        num = dr["num"].ToString(),
                        money = dr["money"].ToString(),
                        ownerEmail = dr["ownerEmail"].ToString()
                    });
                }

                con.Close();
                return View(accounts);

            }
            else
            {
                return View(accounts);
            }
        }
    }
}
