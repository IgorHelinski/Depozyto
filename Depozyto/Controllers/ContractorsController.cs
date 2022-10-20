using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using Depozyto.Models;
using System.Security.Claims;

namespace Depozyto.Controllers
{
    public class ContractorsController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        public static IList<AddContractorsModel> contractors = new List<AddContractorsModel>()
        {
            //new AccountModel(){ num = "dfasj", money = "fjands", ownerEmail = "fdnjbask"}

        };

        

        [Authorize]
        public IActionResult Index()
        {
            contractors.Clear();

            //get all users accounts and put them in list
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Contractors where ownerEmail='" + User.FindFirst(ClaimTypes.Email).Value + "';";
            dr = com.ExecuteReader();
            if (dr.Read())
            {

                contractors.Add(new AddContractorsModel
                {
                    Name = dr["Name"].ToString(),
                    Surname = dr["Surname"].ToString(),
                    Accountnumber = dr["Accountnumber"].ToString(),
                    Email = dr["Email"].ToString(),
                    OwnerEmail = dr["ownerEmail"].ToString()
                });

                while (dr.Read())
                {

                    contractors.Add(new AddContractorsModel
                    {
                        Name = dr["Name"].ToString(),
                        Surname = dr["Surname"].ToString(),
                        Accountnumber = dr["Accountnumber"].ToString(),
                        Email = dr["Email"].ToString(),
                        OwnerEmail = dr["ownerEmail"].ToString()
                    });
                }

                con.Close();

                return View(contractors);

            }
            else
            {
                return View(contractors);
            }
            
        }

        [Authorize]
        public IActionResult AddContractor()
        {
            return View();
        }
        
        void connectionString()
        {

            //połączenia serwera
            con.ConnectionString = "Server=localhost\\SQLEXPRESS;Initial Catalog=epicDataBase;Persist Security Info=True;User ID=sa;Password=haslo";
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateContractor(AddContractorsModel addContractors)
        {
            connectionString();
            con.Open();
            SqlCommand com = new SqlCommand("SP_Contractors", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Name", addContractors.Name);
            com.Parameters.AddWithValue("@Surname", addContractors.Surname);
            com.Parameters.AddWithValue("@Accountnumber", addContractors.Accountnumber);
            com.Parameters.AddWithValue("@Email", addContractors.Email);
            com.Parameters.AddWithValue("@ownerEmail", User.FindFirst(ClaimTypes.Email).Value);
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                //Success
                return RedirectToAction("Index", "Contractors", contractors);
            }
            else
            {
                //Failed
                return View("Register");
            }
        }
    }
}
