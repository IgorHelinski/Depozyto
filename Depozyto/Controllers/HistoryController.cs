using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Depozyto.Controllers
{
    public class HistoryController : Controller
    {
        public static IList<HistoryModel> history = new List<HistoryModel>()
        {
            //new AccountModel(){ num = "dfasj", money = "fjands", ownerEmail = "fdnjbask"}
        };

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public HistoryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        [Authorize]
        public IActionResult History() //Strona historii tranzakcji
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
                    //FromEmail = dr["FromEmail"].ToString(),
                    //ToEmail = dr["ToEmail"].ToString(),
                    Amount = dr["Amount"].ToString(),
                    Title = dr["Title"].ToString(),
                    Date = dr["Date"].ToString()
                });

                while (dr.Read())
                {

                    history.Add(new HistoryModel
                    {
                        //FromEmail = dr["FromEmail"].ToString(),
                        //ToEmail = dr["ToEmail"].ToString(),
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
    }
}
