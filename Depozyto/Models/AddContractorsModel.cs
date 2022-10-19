using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Depozyto.Models
{
    public class AddContractorsModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Accountnumber { get; set; }
        public string Email { get; set; }
        public string OwnerEmail { get; set; }
    }
}