using NetDevloperTask.Models;

namespace NetDevloperTask.Repositories.interfaces
{
    public interface IBusinessCardRepository
    {
        Task<BusinessCard> CreateBusinessCardAsync(BusinessCard businessCard);
        Task<BusinessCard> GetBusinessCardByIdAsync(int id);
        Task<IEnumerable<BusinessCard>> GetAllBusinessCardsAsync();
        Task DeleteBusinessCardAsync(BusinessCard businessCard);
        Task<IEnumerable<BusinessCard>> GetFilteredBusinessCardsAsync(string? name, DateTime? dob, string? phone, string? gender, string? email);
    }
}
