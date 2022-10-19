using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Depozyto.Controllers
{
    public class TransactionController : Controller
    {



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

            return View();
        }

        [Authorize]
        public IActionResult Transaction()
        {
            List<AccountModel> accounts = new List<AccountModel>();

            accounts.Clear();

            //get all users accounts and put them in list
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Accounts where ownerEmail='" + User.FindFirst(ClaimTypes.Email).Value + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

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
                //success
                return View(accounts);

            }
            else
            {
                //failure
                return View();
            }

            
        }

    }
}
