using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Depozyto.Models
{
	public class RegisterModel
	{
        //Dane rejestracji
        public string DePassword { get; set; }
		public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

    }
}
