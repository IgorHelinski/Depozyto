using Depozyto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace Depozyto.Controllers
{
    public class AdminController : Controller
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
        public AdminController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            return View();
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

        public IActionResult ChangeRole(string id)
        {
            TempData["Route"] = id;
            return View();
        }

        public IActionResult Change(AdminModel adm)
        {
            string changeEmail;
            changeEmail = TempData["Route"].ToString();
            
            connectionString();
            con.Open();
            com.Connection = con;

            com.CommandText = "UPDATE Clients SET Role = '" + adm.role + "' WHERE Email = '" + changeEmail + "';";
            com.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult DeleteUser(string id)
        {
            TempData["DeleteUserId"] = id;
            return View();
        }
        public IActionResult Delete()
        {
            string deleteId;
            deleteId = TempData["DeleteUserId"].ToString();

            connectionString();
            con.Open();
            com.Connection = con;

            com.CommandText = "UPDATE Clients SET Blocked = '" + true + "' WHERE Email = '" + deleteId + "';";
            com.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("Index", "Admin");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Accounts()
        {
            accounts.Clear();

            //get all users accounts and put them in list
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Accounts;";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                accounts.Add(new AccountModel
                {
                    num = dr["num"].ToString(),
                    money = dr["money"].ToString(),
                    ownerEmail = dr["ownerEmail"].ToString()
                });

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
                return View(accounts);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Logs()
        {
            history.Clear();

            //get all users accounts and put them in list
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.TransactionLogs;";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                history.Add(new HistoryModel
                {
                    FromEmail = dr["FromEmail"].ToString(),
                    ToEmail = dr["ToEmail"].ToString(),
                    Amount = dr["Amount"].ToString(),
                    Title = dr["Title"].ToString(),
                    Date = dr["Date"].ToString(),
                    FromAccountId = Convert.ToInt32(dr["FromAccountId"]),
                    ToAccountId = Convert.ToInt32(dr["ToAccountId"])
                }) ;

                while (dr.Read())
                {

                    history.Add(new HistoryModel
                    {
                        FromEmail = dr["FromEmail"].ToString(),
                        ToEmail = dr["ToEmail"].ToString(),
                        Amount = dr["Amount"].ToString(),
                        Title = dr["Title"].ToString(),
                        Date = dr["Date"].ToString(),
                        FromAccountId = Convert.ToInt32(dr["FromAccountId"]),
                        ToAccountId = Convert.ToInt32(dr["ToAccountId"])
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
        public IActionResult Edit(string Id)
        {
            //string EnPassword;
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "Select * from dbo.Clients where Email = '" + Id + "';";

            dr = com.ExecuteReader();
            if (dr.Read())
            {
                ViewData["Login"] = dr["Login"].ToString();
                ViewData["Imie"] = dr["Imie"].ToString();
                ViewData["Nazwisko"] = dr["Nazwisko"].ToString();
                // EnPassword = dr["Haslo"].ToString();

            }
            else
            {
                // EnPassword = "byleco";
            }

            //string Password = EncriptController.Decrypt(EnPassword);

            //ViewData["Haslo"] = Password;
            TempData["EditID"] = Id;
            return View();
        }
        public IActionResult EditUser(AdminModel adm)
        {
            string changeEmail;
            changeEmail = TempData["EditID"].ToString();
            // adm.password = EncriptController.Encrypt(adm.password);
            connectionString();
            con.Open();
            com.Connection = con;

            com.CommandText = "UPDATE Clients SET Imie = '" + adm.name + "' WHERE Email = '" + changeEmail + "';";
            com.ExecuteNonQuery();
            com.CommandText = "UPDATE Clients SET Nazwisko = '" + adm.surname + "' WHERE Email = '" + changeEmail + "';";
            com.ExecuteNonQuery();
            com.CommandText = "UPDATE Clients SET Login = '" + adm.login + "' WHERE Email = '" + changeEmail + "';";
            com.ExecuteNonQuery();
            //com.CommandText = "UPDATE Clients SET Haslo = '" + adm.password + "' WHERE Email = '" + changeEmail + "';";
            //com.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("Index", "Admin");
        }
    }
}
