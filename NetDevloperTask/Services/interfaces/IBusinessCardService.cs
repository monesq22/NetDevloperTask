using NetDevloperTask.Models;

namespace NetDevloperTask.Services.interfaces
{
    public interface IBusinessCardService
    {
        Task<BusinessCard> CreateBusinessCardAsync(BusinessCard businessCard);
        //Task<BusinessCard> CreateBusinessCardFromQrCodeAsync(string qrCodeData);
        Task<BusinessCard> CreateBusinessCardFromFileAsync(string fileData, string fileType);
        Task<BusinessCard> GetBusinessCardByIdAsync(int id);
        Task<IEnumerable<BusinessCard>> GetAllBusinessCardsAsync();
        Task DeleteBusinessCardAsync(int id);
        Task<string> ExportBusinessCardsToXmlAsync();
        Task<string> ExportBusinessCardsToCsvAsync(); 
        Task<IEnumerable<BusinessCard>> GetFilteredBusinessCardsAsync(string? name, DateTime? dob, string? phone, string? gender, string? email);
    }
}
