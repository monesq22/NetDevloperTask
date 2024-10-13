using CsvHelper;
using NetDevloperTask.Models;
using NetDevloperTask.Repositories.interfaces;
using NetDevloperTask.Services.interfaces;
using System.Formats.Asn1;
using System.Globalization;
using System.Xml.Serialization;
using ZXing;

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
            return await _repository.CreateBusinessCardAsync(businessCard);
        }

        //public async Task<BusinessCard> CreateBusinessCardFromQrCodeAsync(string qrCodeData)
        //{
        //    var reader = new BarcodeReader();
        //    using (var ms = new MemoryStream(System.Convert.FromBase64String(qrCodeData)))
        //    {
        //        var result = reader.Decode(ms);
        //        if (result != null)
        //        {
        //            // QR code assumed to contain business card data delimited by semicolons
        //            var data = result.Text.Split(';');
        //            var businessCard = new BusinessCard
        //            {
        //                Name = data[0],
        //                Gender = data[1],
        //                DateOfBirth = DateTime.Parse(data[2]),
        //                Email = data[3],
        //                Phone = data[4],
        //                Address = data[5]
        //            };
        //            return await _repository.CreateBusinessCardAsync(businessCard);
        //        }
        //    }
        //    return null;
        //}

        public async Task<BusinessCard> CreateBusinessCardFromFileAsync(string fileData, string fileType)
        {
            BusinessCard businessCard = null;

            if (fileType == "XML")
            {
                var serializer = new XmlSerializer(typeof(BusinessCard));
                using (var reader = new StringReader(fileData))
                {
                    businessCard = (BusinessCard)serializer.Deserialize(reader);
                }
            }
            else if (fileType == "CSV")
            {
                using (var reader = new StringReader(fileData))
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    businessCard = csv.GetRecord<BusinessCard>();
                }
            }

            if (businessCard != null)
            {
                return await _repository.CreateBusinessCardAsync(businessCard);
            }

            return null;
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

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, businessCards);
                return stringWriter.ToString();
            }
        }

        public async Task<string> ExportBusinessCardsToCsvAsync()
        {
            var businessCards = await _repository.GetAllBusinessCardsAsync();

            using (var stringWriter = new StringWriter())
            using (var csvWriter = new CsvWriter(stringWriter, CultureInfo.InvariantCulture))
            {
                await csvWriter.WriteRecordsAsync(businessCards);
                return stringWriter.ToString();
            }
        }
        public async Task<IEnumerable<BusinessCard>> GetFilteredBusinessCardsAsync(string? name, DateTime? dob, string? phone, string? gender, string? email)
        {
            return await _repository.GetFilteredBusinessCardsAsync(name, dob, phone, gender, email);
        }
    }
}
