namespace Depozyto.Models
{
    public class HistoryModel
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Amount { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public int FromAccountId {get; set;}
        public int ToAccountId { get; set; }

        public int FromClientId { get; set; }
        public int ToClientId { get; set; }
    }
}
