using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Depozyto.Controllers
{
    public class AccountsController : Controller
    {
        public static IList<AccountModel> accounts = new List<AccountModel>()
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
        public IActionResult Index(AccountModel acc)
        {
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
                return View(accounts);

            }
            else
            {
                return View();
            }


        }
         
        public IActionResult Deposit()
        {
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
                return View(accounts);

            }
           
            return View(accounts);
        }

        [Authorize]
        public IActionResult Wplata(AccountModel acc)
        {
            float b = 0;

            //kod do wplacania kaski na konto
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT money FROM Accounts WHERE ownerEmail = '" + User.FindFirst(ClaimTypes.Email).Value + "' AND num = '"+ acc.numer +"';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                b = Convert.ToSingle((double)dr["Money"]);
            }
            con.Close();
            connectionString();
            con.Open();
            com.Connection = con;
            float amount = b + acc.kaska;

            com.CommandText = "UPDATE Accounts SET money = '" + amount + "' WHERE ownerEmail = '" + User.FindFirst(ClaimTypes.Email).Value + "'AND num = '"+ acc.numer +"';";
            com.ExecuteNonQueryAsync();

            con.Close();
            return RedirectToAction("Index", "Accounts", accounts);
        }

        [Authorize]
        public IActionResult AddAccount()
        {
            return View(new AccountModel());
        }



        [Authorize]
        public IActionResult Add(AccountModel acc)
        {
            connectionString();
            SqlCommand com = new SqlCommand("SP_Account", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@num", acc.num);
            com.Parameters.AddWithValue("@money", 1f);
            com.Parameters.AddWithValue("@ownerEmail", User.FindFirst(ClaimTypes.Email).Value);
            
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                accounts.Add(new AccountModel
                {
                    num = acc.num,
                    money = "1",
                    ownerEmail = User.FindFirst(ClaimTypes.Email).Value
                }) ;



                //Success
                return RedirectToAction("Index", "Accounts", accounts);
            }
            else
            {
                //Failed
                return View();
            }
            
        }

    }
}
