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

        void connectionString()
        {
            //połączenia serwera
            con.ConnectionString = "Server=localhost\\SQLEXPRESS;Initial Catalog=epicDataBase;Persist Security Info=True;User ID=sa;Password=haslo";
        }

        [Authorize]
        public IActionResult Index()
        {
            history.Clear();

            //get all users accounts and put them in list
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.TransactionLogs where FromEmail='" + User.FindFirst(ClaimTypes.Email).Value + "'or ToEmail = '" + User.FindFirst(ClaimTypes.Email).Value + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                history.Add(new HistoryModel
                {
                    FromEmail = dr["FromEmail"].ToString(),
                    ToEmail = dr["ToEmail"].ToString(),
                    Amount = dr["Amount"].ToString(),
                    Title = dr["Title"].ToString(),
                    Date = dr["Date"].ToString()
                });

                while (dr.Read())
                {

                    history.Add(new HistoryModel
                    {
                        FromEmail = dr["FromEmail"].ToString(),
                        ToEmail = dr["ToEmail"].ToString(),
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
                return View(history);
            }




        }



        //Dodawanie kaski
        [Authorize]
        public IActionResult Deposit(IList<AccountModel> ac)
        {
            

            return View(ac);
        }

        [Authorize]
        public IActionResult Wplata(UserModel usr)
        {
            float b = 0;

            //kod do wplacania kaski na konto
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT Money FROM Clients WHERE Email = '" + User.FindFirst(ClaimTypes.Email).Value + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                b = Convert.ToSingle((double)dr["Money"]);
            }
            con.Close();
            connectionString();
            con.Open();
            com.Connection = con;
            float amount = b + usr.Money; 

                com.CommandText = "UPDATE Clients SET Money = '"+ amount +"' WHERE Email = '"+ User.FindFirst(ClaimTypes.Email).Value  +"';";
            com.ExecuteNonQueryAsync();

            con.Close();
            return View("Succes");
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
        //Historia tranzakcji
        [Authorize]
        public IActionResult History()
        {
            return View();
        }
    }
}
