using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Configuration;

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

        private IConfiguration Configuration;
        public AccountsController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        [Authorize] 
        public IActionResult Index(AccountModel acc) //Strona główna
        {
            //Wczytuje konta z bady danych i wstawia je do tabeli aby je wyswietlic

            accounts.Clear();

            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Accounts where ownerEmail='" + User.FindFirst(ClaimTypes.Email).Value + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                accounts.Add(new AccountModel
                {
                    num = dr["num"].ToString(),
                    money = dr["money"].ToString(),
                    ownerEmail = dr["ownerEmail"].ToString(),
                    Currency = dr["Currency"].ToString()
                });

                while (dr.Read())
                {

                    accounts.Add(new AccountModel
                    {
                        num = dr["num"].ToString(),
                        money = dr["money"].ToString(),
                        ownerEmail = dr["ownerEmail"].ToString(),
                        Currency = dr["Currency"].ToString()
                    });
                }

                con.Close();
                return View(accounts);
            }
            else
            {
                con.Close();
                return View(accounts);
            }
        }

        [Authorize]
        public IActionResult AddAccount(CurrenciesModel curr) //Strona dodawania nowego konta
        {
            List<SelectListItem> currencies = new List<SelectListItem>();

            currencies.Clear();

            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Currencies;";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                currencies.Add(new SelectListItem
                {
                    Text = dr["Currency"].ToString()
                });

                while (dr.Read())
                {

                    currencies.Add(new SelectListItem
                    {
                        Text = dr["Currency"].ToString()
                    });
                }

                con.Close();
                ViewBag.Currency = currencies;
                return View(new AccountModel());
            }
            else
            {
                con.Close();
                ViewBag.Currency = currencies;
                return View(new AccountModel());
            }
        }

        [Authorize]
        public IActionResult Add(AccountModel acc) //Dodanie nowego konta
        {
            connectionString();
            SqlCommand com = new SqlCommand("SP_Account", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@num", acc.num);
            com.Parameters.AddWithValue("@money", 0f);
            com.Parameters.AddWithValue("@ownerEmail", User.FindFirst(ClaimTypes.Email).Value);
            com.Parameters.AddWithValue("@Currency", acc.Currency);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                accounts.Add(new AccountModel
                {
                    num = acc.num,
                    money = "0",
                    ownerEmail = User.FindFirst(ClaimTypes.Email).Value,
                    Currency = acc.Currency
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
