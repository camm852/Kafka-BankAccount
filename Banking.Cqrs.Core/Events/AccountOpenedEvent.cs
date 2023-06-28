namespace Banking.Cqrs.Core.Events
{
    public class AccountOpenedEvent : BaseEvent
    {
        public string AccountHolder { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public double OpeningBalance { get; set; }

        public AccountOpenedEvent(string id, 
            string accoundHolder, 
            string accountType, 
            DateTime createdDate, 
            double openingBalance) : base(id)
        {
            AccountHolder = accoundHolder;
            AccountType = accountType;
            CreatedDate = createdDate;
            OpeningBalance = openingBalance;
        }
    }
}
