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

        private IConfiguration Configuration;
        public ContractorsController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        [Authorize]
        public IActionResult Index() //Strona główna
        {
            //Wczytuje kontrachentów z bazy danych i wstawia ich do listy

            contractors.Clear();
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
                con.Close();
                return View(contractors);
            }
        }

        [Authorize]
        public IActionResult AddContractor() //Strona dodawania kontrahenta
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateContractor(AddContractorsModel addContractors) //Dodawanie kontrahenta
        {
            //Zapisanie wpisanych danych do bazy danych
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
                return View("AddContractor");
            }
        }
    }
}
