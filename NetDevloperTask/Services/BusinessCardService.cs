using CsvHelper;
using NetDevloperTask.Models;
using NetDevloperTask.Repositories.interfaces;
using NetDevloperTask.Services.interfaces;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Xml.Serialization;

namespace NetDevloperTask.Services
{
    public class BusinessCardService : IBusinessCardService
    {
        private readonly IBusinessCardRepository _repository;

        public BusinessCardService(IBusinessCardRepository repository)
        {
            _repository = repository;
        }

        public async Task<BusinessCard> CreateBusinessCardAsync(BusinessCard businessCard)
        {
            Validator.ValidateObject(businessCard, new ValidationContext(businessCard), validateAllProperties: true);
            return await _repository.CreateBusinessCardAsync(businessCard);
        }

        public async Task<BusinessCard> GetBusinessCardByIdAsync(int id)
        {
            return await _repository.GetBusinessCardByIdAsync(id);
        }

        public async Task<IEnumerable<BusinessCard>> GetAllBusinessCardsAsync()
        {
            return await _repository.GetAllBusinessCardsAsync();
        }

        public async Task DeleteBusinessCardAsync(int id)
        {
            var card = await _repository.GetBusinessCardByIdAsync(id);
            if (card == null)
            {
                throw new KeyNotFoundException("Business card not found");
            }

            await _repository.DeleteBusinessCardAsync(card);
        }

        public async Task<string> ExportBusinessCardsToXmlAsync()
        {
            var businessCards = await _repository.GetAllBusinessCardsAsync();

            var xmlSerializer = new XmlSerializer(typeof(List<BusinessCard>));

            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, businessCards.ToList());
            return stringWriter.ToString();
        }

        public async Task<string> ExportBusinessCardsToCsvAsync()
        {
            var businessCards = await _repository.GetAllBusinessCardsAsync();

            using var stringWriter = new StringWriter();
            using var csvWriter = new CsvWriter(stringWriter, CultureInfo.InvariantCulture);
            await csvWriter.WriteRecordsAsync(businessCards);
            return stringWriter.ToString();
        }

        public async Task<IEnumerable<BusinessCard>> GetFilteredBusinessCardsAsync(string? name, DateTime? dob, string? phone, string? gender, string? email)
        {
            return await _repository.GetFilteredBusinessCardsAsync(name, dob, phone, gender, email);
        }

        public async Task<List<BusinessCard>> ImportBusinessCardsFromXmlAsync(Stream xmlStream)
        {
            var serializer = new XmlSerializer(typeof(List<BusinessCard>));
            List<BusinessCard> businessCards;

            using (var reader = new StreamReader(xmlStream))
            {
                businessCards = (List<BusinessCard>)serializer.Deserialize(reader);
            }

            foreach (var card in businessCards)
            {
                if (card.Id != 0)
                    card.Id = 0; // Ensure Id is not set for new records
                // Validate business card entries before processing
                Validator.ValidateObject(card, new ValidationContext(card), validateAllProperties: true);
            }

            return businessCards;
        }

        public async Task<List<BusinessCard>> ImportBusinessCardsFromCsvAsync(Stream csvStream)
        {
            using var streamReader = new StreamReader(csvStream);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var records = await csvReader.GetRecordsAsync<BusinessCard>().ToListAsync();

            foreach (var card in records)
            {
                if (card.Id != 0)
                    card.Id = 0; // Reset Id for new records
                // Validate business card entries before processing
                Validator.ValidateObject(card, new ValidationContext(card), validateAllProperties: true);
            }

            return records;
        }
    }
}
