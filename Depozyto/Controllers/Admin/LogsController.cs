using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace Depozyto.Controllers.Admin
{
    public class LogsController : Controller
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
        public LogsController(IConfiguration _configuration)
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
        public IActionResult Logs()
        {
            history.Clear();

            //get all users accounts and put them in list
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.TransactionLogs;";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                history.Add(new HistoryModel
                {
                    FromEmail = dr["FromEmail"].ToString(),
                    ToEmail = dr["ToEmail"].ToString(),
                    Amount = dr["Amount"].ToString(),
                    Title = dr["Title"].ToString(),
                    Date = dr["Date"].ToString(),
                    FromAccountId = Convert.ToInt32(dr["FromAccountId"]),
                    ToAccountId = Convert.ToInt32(dr["ToAccountId"])
                });

                while (dr.Read())
                {

                    history.Add(new HistoryModel
                    {
                        FromEmail = dr["FromEmail"].ToString(),
                        ToEmail = dr["ToEmail"].ToString(),
                        Amount = dr["Amount"].ToString(),
                        Title = dr["Title"].ToString(),
                        Date = dr["Date"].ToString(),
                        FromAccountId = Convert.ToInt32(dr["FromAccountId"]),
                        ToAccountId = Convert.ToInt32(dr["ToAccountId"])
                    });
                }

                con.Close();
                return View(history);
            }
            else
            {
                return View(history);
            }

        }
    }
}
