using Microsoft.AspNetCore.Mvc;
using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Dynamic;

namespace Depozyto.Controllers
{
    public class DashBoardController : Controller
    {
        public static IList<HistoryModel> history = new List<HistoryModel>()
        {
            //new AccountModel(){ num = "dfasj", money = "fjands", ownerEmail = "fdnjbask"}
        };

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public DashBoardController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        [Authorize]
        public IActionResult Index() //Strona główna
        {
            //znajduje logi tranzakcji z bazy danych i wstawia je do tabeli
            history.Clear();
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Transactions where FromClientId='" + User.FindFirst(ClaimTypes.SerialNumber).Value + "'or ToClientId = '" + User.FindFirst(ClaimTypes.SerialNumber).Value + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                history.Add(new HistoryModel
                {
                    FromEmail = dr["FromClientId"].ToString(),
                    ToEmail = dr["ToClientId"].ToString(),
                    Amount = dr["Amount"].ToString(),
                    Title = dr["Title"].ToString(),
                    Date = dr["Date"].ToString()
                });

                while (dr.Read())
                {

                    history.Add(new HistoryModel
                    {
                        FromEmail = dr["FromClientId"].ToString(),
                        ToEmail = dr["ToClientId"].ToString(),
                        Amount = dr["Amount"].ToString(),
                        Title = dr["Title"].ToString(),
                        Date = dr["Date"].ToString()
                    });
                }

                con.Close();
                return View(history);
            }
            else
            {
                con.Close();
                return View(history);
            }
        }

        //Sukces
        [Authorize]
        public IActionResult Succes()
        {
            return View();
        }

        //Anulowanie
        [Authorize]
        public IActionResult Cancel()
        {
            return View();
        }
    }
}
