using NetDevloperTask.Models;

namespace NetDevloperTask.Services.interfaces
{
    public interface IBusinessCardService
    {
        Task<BusinessCard> CreateBusinessCardAsync(BusinessCard businessCard);
        //Task<BusinessCard> CreateBusinessCardFromQrCodeAsync(string qrCodeData);
        Task<BusinessCard> CreateBusinessCardFromFileAsync(string fileData, string fileType);
        Task<BusinessCard> GetBusinessCardByIdAsync(int id); // Add this method
        Task<IEnumerable<BusinessCard>> GetAllBusinessCardsAsync();  // New method to fetch all
        Task DeleteBusinessCardAsync(int id);  // No return value for delete
        Task<string> ExportBusinessCardsToXmlAsync();  // For XML export
        Task<string> ExportBusinessCardsToCsvAsync();  // For CSV export
        Task<IEnumerable<BusinessCard>> GetFilteredBusinessCardsAsync(string? name, DateTime? dob, string? phone, string? gender, string? email);
    }
}
