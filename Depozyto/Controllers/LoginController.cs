using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using Depozyto.Models;
using System.Data.SqlClient;
using System.Data;

namespace Depozyto.Controllers
{
    public class LoginController : Controller
    {
        
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        bool canRegister;

        // Login
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        void connectionString()
        {
            //połączenia serwera
            con.ConnectionString = "Server=localhost\\SQLEXPRESS;Initial Catalog=epicDataBase;Persist Security Info=True;User ID=sa;Password=haslo";
        }
        public IActionResult Index(LoginModel log)
        {
            //sprawdza czy w bazie danych jest taki urzytkownik
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Clients where Login='"+log.Login+"' and Haslo='"+log.Password+"'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                //znalazl urzytkownika
                con.Close();
                log.loggedIn = true;
                return View("Success", log);
            }
            else
            {
                //nie znalazl urzytkownika
                con.Close();
                log.loggedIn = false;
                ViewData["LoginFlag"] = "Nieprawidłowy login lub hasło!!!";
                return View();
            }
            
        }

        // Login/Register
        public ActionResult Register()
        {
            return View();
        }

        //sprawdza czy mozemy zapisac uzytkownika w bazie danych (czy nie ma tos takiego loginu itp)
        void CheckRegister(RegisterModel reg)
        {
            //sprawdza czy w bazie danych jest już taki urzytkownik
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Clients where Login='" + reg.Login + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                //jest taki urzytkownik (nie mozna zarejestrowac)
                canRegister = false;
            }
            else
            {
                con.Close();
                //nie ma takiego urzttkownika (mozna zarejestrowac)
                canRegister = true;
            }
        }

        //Tutaj zapisujemy dane na bazie danych
        [HttpPost]
        public ActionResult CreateAccount(RegisterModel reg)
        {
            //sprawdzamy czy mozemy zarejestrowac nowego klienta
            CheckRegister(reg);

            
            if (canRegister) 
            {
                connectionString();
                SqlCommand com = new SqlCommand("SP_Client", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Login", reg.Login);
                com.Parameters.AddWithValue("@Haslo", reg.Password);
                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {
                    //Success
                    return View();
                }
                else
                {
                    //Failed
                    return View("Register");
                }
            }
            else
            {
                //nie moze zarejestrowac
                ViewData["LoginDouble"] = "Login jest już zajęty przez innego urzytkownika";
                return View("Register");
            }
            
        }
    }
}
