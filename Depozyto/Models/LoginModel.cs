using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Depozyto.Models
{
    public class LoginModel
    {
        //Dane logowania
        public string EnPassword { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool loggedIn { get; set; }
    }
}
