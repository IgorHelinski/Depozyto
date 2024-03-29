﻿namespace Depozyto.Models
{
    public class TransactionModel
    {
        public string Accounts { get; set; }
        public string Contractors { get; set; }
        public int cash { get; set; }
        public string title { get; set; }
        public string date { get; set;}
        public string currency { get; set; }

        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }

        public int FromClientId { get; set; }
        public int ToClientId { get; set; }
    }
}
