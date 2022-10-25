namespace Depozyto.Models
{
    public class TransactionModel
    {
        public string Accounts { get; set; }
        public string Contractors { get; set; }
        public int cash { get; set; }
        public string title { get; set; }
        public string date { get; set;}

        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
    }
}
