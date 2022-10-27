using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace Depozyto.Models
{
    public class AccountModel
    {
        public string numer { get; set; }
        public int kaska { get; set; }

        public string num { get; set; }
        public string money { get; set; }
        public string ownerEmail { get; set; }
        public string Currency { get; set; }
        public int ClientId { get; set; }

    }
}
