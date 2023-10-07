namespace QualifiApi.Models
{
    public class CreditCardsModel
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public string? CardType { get; set; }
        public decimal Apr { get; set; }
        public int MinSalary { get; set; }

        public CreditCardsModel(int Id, string CardName, string? CardType, decimal Apr, int MinSalary)
        {
            this.Id = Id;
            this.CardName = CardName;
            this.CardType = CardType;
            this.Apr = Apr;
            this.MinSalary = MinSalary;
        }
    }
}
