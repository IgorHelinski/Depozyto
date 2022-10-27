using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Depozyto.Controllers
{
	public class DepositController : Controller
	{
        public static IList<AccountModel> accounts = new List<AccountModel>()
        {
            //new AccountModel(){ num = "dfasj", money = "fjands", ownerEmail = "fdnjbask"}
        };

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public DepositController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        [Authorize]
        public IActionResult Deposit() //Strona wpłacania środków
        {
            //Wczytuje konta z bazy danych i wstawia do tabeli

            accounts.Clear();

            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Accounts where ClientId='" + User.FindFirst(ClaimTypes.SerialNumber).Value + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                accounts.Add(new AccountModel
                {
                    num = dr["num"].ToString(),
                    money = dr["money"].ToString(),
                    //ownerEmail = dr["ownerEmail"].ToString()
                });

                while (dr.Read())
                {


                    accounts.Add(new AccountModel
                    {
                        num = dr["num"].ToString(),
                        money = dr["money"].ToString(),
                        //ownerEmail = dr["ownerEmail"].ToString()
                    });
                }

                con.Close();
                return View(accounts);

            }

            con.Close();
            return View(accounts);
        }

        [Authorize]
        public IActionResult DepositMoney(AccountModel acc) //Dodawanie środków do konta
        {
            //Sprawdzamy ile pieniędzy znajduje się na koncie użytkownika

            float b = 0;
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT money FROM Accounts WHERE ClientId = '" + User.FindFirst(ClaimTypes.SerialNumber).Value + "' AND num = '" + acc.numer + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                b = Convert.ToSingle((double)dr["Money"]);
            }
            con.Close();

            //Wpłacamy środki na wybrane konto

            connectionString();
            con.Open();
            com.Connection = con;

            float amount = b + acc.kaska;

            com.CommandText = "UPDATE Accounts SET money = '" + amount + "' WHERE ClientId = '" + User.FindFirst(ClaimTypes.SerialNumber).Value + "'AND num = '" + acc.numer + "';";
            com.ExecuteNonQuery();

            con.Close();

            return RedirectToAction("Index", "Accounts", accounts);
        }
    }
}
