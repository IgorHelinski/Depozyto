﻿using Depozyto.Models;
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
    public class TransactionController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public TransactionController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        [Authorize]
        public IActionResult Transaction() //Strona wykonywania tranzakcji
        {
            List<SelectListItem> accounts = new List<SelectListItem>();
            List<SelectListItem> contractors = new List<SelectListItem>();

            accounts.Clear();
            contractors.Clear();

            //znajduje i wyświetla konta zalogowanego użytkownika
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
                ViewBag.Accounts = accounts;
            }
            else
            {
                con.Close();
                ViewBag.Accounts = accounts;
            }

            //znajduje i wyświetla kontrahentów przypisanych do zalogowanego użytkownika
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
        public IActionResult Payment(TransactionModel trans) //Przelewanie pieniędzy
        {
            //Sprawdzamy ile mamy kasy na koncie
            float b = 0;  
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * FROM Accounts WHERE ownerEmail = '" + User.FindFirst(ClaimTypes.Email).Value + "' AND num = '" + trans.Accounts + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                trans.FromAccountId = Convert.ToInt32(dr["Id"]);
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
            com.CommandText = "SELECT * FROM Accounts WHERE ownerEmail = '" + email + "' AND num = '" + numerKonta + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                trans.ToAccountId = Convert.ToInt32(dr["Id"]);
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
            comm.Parameters.AddWithValue("@FromAccountId", trans.FromAccountId);
            comm.Parameters.AddWithValue("@ToAccountId", trans.ToAccountId);
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

    }
}