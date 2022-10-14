using Microsoft.AspNetCore.Mvc;
using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Depozyto.Controllers
{
    public class DashBoardController : Controller
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
            return View("Index");
        }



        //Dodawanie kaski
        [Authorize]
        public IActionResult Deposit()
        {
            return View();
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


       

        //Zapłacenie kontrahentowi
        [Authorize]
        public IActionResult Transaction()
        {
            return View();
        }
        public IActionResult Zaplac()
        {
            return View();
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
