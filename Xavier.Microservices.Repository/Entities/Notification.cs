using System;

namespace Xavier.Microservices.Repository.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string SourceAccount { get; set; }
        public string DestinationAccount { get; set; }
        public Int64 TransactionAmount { get; set; }
        public DateTime RetrieveDateTime { get; set; }
    }
}