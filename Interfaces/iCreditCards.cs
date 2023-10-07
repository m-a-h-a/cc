using QualifiApi.Models;

namespace QualifiApi.Interfaces
{
    public interface iCreditCards
    {
        Task<List<CreditCardsModel>> GetEligbleCreditCards(int salary, CancellationToken cancellationToken);
    }
}
