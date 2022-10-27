using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace Depozyto.Controllers.Admin
{

    public class UserController : Controller
    {
        public static IList<HistoryModel> history = new List<HistoryModel>()
        {
            //new AccountModel(){ num = "dfasj", money = "fjands", ownerEmail = "fdnjbask"}
        };
        public static IList<AccountModel> accounts = new List<AccountModel>()
        {
            //new AccountModel(){ num = "dfasj", money = "fjands", ownerEmail = "fdnjbask"}
        };
        public static IList<UserModel> users = new List<UserModel>()
        {
            //new AccountModel(){ num = "dfasj", money = "fjands", ownerEmail = "fdnjbask"}
        };
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        private IConfiguration Configuration;
        public UserController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = Configuration.GetConnectionString("ConString");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            string sample = "True";
            bool bruh;

            users.Clear();



            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Clients;";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                if (dr["Blocked"].ToString() == sample)
                {
                    bruh = true;
                }
                else
                {
                    bruh = false;
                }

                users.Add(new UserModel
                {
                    Login = dr["Login"].ToString(),
                    Password = dr["Haslo"].ToString(),
                    Name = dr["Imie"].ToString(),
                    LastName = dr["Nazwisko"].ToString(),
                    Email = dr["Email"].ToString(),
                    Blocked = bruh,
                    Role = dr["Role"].ToString()
                });

                while (dr.Read())
                {

                    if (dr["Blocked"].ToString() == sample)
                    {
                        bruh = true;
                    }
                    else
                    {
                        bruh = false;
                    }

                    users.Add(new UserModel
                    {
                        Login = dr["Login"].ToString(),
                        Password = dr["Haslo"].ToString(),
                        Name = dr["Imie"].ToString(),
                        LastName = dr["Nazwisko"].ToString(),
                        Email = dr["Email"].ToString(),
                        Blocked = bruh,
                        Role = dr["Role"].ToString()
                    });
                }

                con.Close();
                return View(users);

            }
            else
            {
                return View(users);
            }

        }
    }
}
