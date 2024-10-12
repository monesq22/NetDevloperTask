using Microsoft.EntityFrameworkCore;
using NetDevloperTask.Data;
using NetDevloperTask.Models;
using NetDevloperTask.Repositories.interfaces;

namespace NetDevloperTask.Repositories
{
    public class BusinessCardRepository : IBusinessCardRepository
    {
        private readonly BusinessCardDbContext _context;

        public BusinessCardRepository(BusinessCardDbContext context)
        {
            _context = context;
        }
        public async Task<BusinessCard> CreateBusinessCardAsync(BusinessCard businessCard)
        {
            await _context.BusinessCards.AddAsync(businessCard);
            await _context.SaveChangesAsync();  // Commit the changes directly here
            return businessCard;
        }
        public async Task<BusinessCard> GetBusinessCardByIdAsync(int id)
        {
            return await _context.BusinessCards.FindAsync(id);
        }
        public async Task<IEnumerable<BusinessCard>> GetAllBusinessCardsAsync()
        {
            return await _context.BusinessCards.ToListAsync();
        }
        public async Task DeleteBusinessCardAsync(BusinessCard businessCard)
        {
            _context.BusinessCards.Remove(businessCard);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<BusinessCard>> GetFilteredBusinessCardsAsync(string? name, DateTime? dob, string? phone, string? gender, string? email)
        {
            return await _context.BusinessCards
                .Where(b => (string.IsNullOrEmpty(name) || b.Name.Contains(name)) &&
                            (!dob.HasValue || b.DateOfBirth == dob.Value) &&
                            (string.IsNullOrEmpty(phone) || b.Phone.Contains(phone)) &&
                            (string.IsNullOrEmpty(gender) || b.Gender == gender) &&
                            (string.IsNullOrEmpty(email) || b.Email.Contains(email)))
                .ToListAsync();
        }
    }
}
