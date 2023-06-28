namespace Banking.Cqrs.Core.Events
{
    public class FundsWithdrawEvent : BaseEvent
    {
        public double Amount { get; set; }

        public FundsWithdrawEvent(string id) : base(id)
        {
        }
    }
}
