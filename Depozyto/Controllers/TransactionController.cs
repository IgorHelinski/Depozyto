using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Depozyto.Controllers
{
    public class TransactionController : Controller
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

            return View();
        }

        [Authorize]
        public IActionResult Transaction()
        {
            List<SelectListItem> accounts = new List<SelectListItem>();
            List<SelectListItem> contractors = new List<SelectListItem>();

            accounts.Clear();
            contractors.Clear();

            //get all users accounts and put them in list
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Accounts where ownerEmail='" + User.FindFirst(ClaimTypes.Email).Value + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                accounts.Add(new SelectListItem
                {
                    Text = dr["num"].ToString(),

                });

                while (dr.Read())
                {

                    accounts.Add(new SelectListItem
                    {
                        Text = dr["num"].ToString(),
                       
                    });
                }

                con.Close();
                //success
                ViewBag.Accounts = accounts;
                //return View();

            }
            else
            {
                con.Close();
                ViewBag.Accounts = accounts;
                
            }

            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Contractors where ownerEmail='" + User.FindFirst(ClaimTypes.Email).Value + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                contractors.Add(new SelectListItem
                {
                    Text = dr["Accountnumber"].ToString() + "," + dr["Email"].ToString()

                });

                while (dr.Read())
                {

                    contractors.Add(new SelectListItem
                    {
                        Text = dr["Accountnumber"].ToString() + "," + dr["Email"].ToString()

                    }) ;
                }

                con.Close();
                ViewBag.Contractors = contractors;
                return View();

            }
            else
            {
                con.Close();
                ViewBag.Contractors = contractors;
                return View();
            }


        }

        [Authorize]
        public IActionResult Przelew(TransactionModel trans)
        {
            //kod do przesylania pieniendzy między kontami

            //Sprawdzamy ile mamy kasy na koncie
            float b = 0;  
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT money FROM Accounts WHERE ownerEmail = '" + User.FindFirst(ClaimTypes.Email).Value + "' AND num = '" + trans.Accounts + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                b = Convert.ToSingle((double)dr["Money"]);
            }
            con.Close();
            // b = ilosc kaski na koncie

            //tutaj bendziemy odejmowac kaske z konta
            connectionString();
            con.Open();
            com.Connection = con;

            float amount = b - trans.cash;

            com.CommandText = "UPDATE Accounts SET money = '" + amount + "' WHERE ownerEmail = '" + User.FindFirst(ClaimTypes.Email).Value + "'AND num = '" + trans.Accounts + "';";
            com.ExecuteNonQuery();
            con.Close();

            //Odseparowujemy numer konta od adresu email
            string[] lista;
            
            lista = trans.Contractors.Split(",");
            string numerKonta = lista[0];
            string email = lista[1];

            //tu bendziemy sprawdzac ile kontrahent ma kaski na koncie
            float a = 0;
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT money FROM Accounts WHERE ownerEmail = '" + email + "' AND num = '" + numerKonta + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                a = Convert.ToSingle((double)dr["Money"]);
            }
            con.Close();
            // a = ilosc kaski na koncie kontrahenta

            //tutaj dodamy kaske na konto kontrachenta
            connectionString();
            con.Open();
            com.Connection = con;

            float amountToAdd = a + trans.cash;

            com.CommandText = "UPDATE Accounts SET money = '" + amountToAdd + "' WHERE ownerEmail = '" + email + "'AND num = '" + numerKonta + "';";
            com.ExecuteNonQuery();
            con.Close();

            //tutaj zrobimy logi tranzakcji do histori

            connectionString();
            SqlCommand comm = new SqlCommand("SP_TransactionLog", con);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.AddWithValue("@FromEmail", User.FindFirst(ClaimTypes.Email).Value);
            comm.Parameters.AddWithValue("@ToEmail", email);
            comm.Parameters.AddWithValue("@Amount", trans.cash);
            comm.Parameters.AddWithValue("@Title", trans.title);
            comm.Parameters.AddWithValue("@Date", trans.date);
            con.Open();
            int i = comm.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                //Success
                return RedirectToAction("Succes", "Dashboard");
            }
            else
            {
                //Failed
                return RedirectToAction("Transaction", "Transaction");
            }
        }

        [Authorize]
        public IActionResult History()
        {
            history.Clear();

            //get all users accounts and put them in list
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.TransactionLogs where FromEmail='" + User.FindFirst(ClaimTypes.Email).Value + "'or ToEmail = '"+ User.FindFirst(ClaimTypes.Email).Value + "';";
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

    }
}
