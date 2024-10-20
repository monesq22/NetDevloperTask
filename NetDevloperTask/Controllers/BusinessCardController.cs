using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDevloperTask.Models;
using NetDevloperTask.Services.interfaces;
using System.Text;

namespace NetDevloperTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardController : ControllerBase
    {
        private readonly IBusinessCardService _businessCardService;

        public BusinessCardController(IBusinessCardService businessCardService)
        {
            _businessCardService = businessCardService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessCard businessCard)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCard = await _businessCardService.CreateBusinessCardAsync(businessCard);
            return CreatedAtAction(nameof(GetById), new { id = createdCard.Id }, createdCard);
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("No file uploaded");

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension != ".xml" && fileExtension != ".csv")
                return BadRequest("Unsupported file format");

            try
            {
                var importedCards = fileExtension == ".xml"
                    ? await _businessCardService.ImportBusinessCardsFromXmlAsync(file.OpenReadStream())
                    : await _businessCardService.ImportBusinessCardsFromCsvAsync(file.OpenReadStream());

                return Ok(importedCards);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var card = await _businessCardService.GetBusinessCardByIdAsync(id);
            if (card is null)
                return NotFound();

            return Ok(card);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cards = await _businessCardService.GetAllBusinessCardsAsync();
            return Ok(cards);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _businessCardService.DeleteBusinessCardAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export([FromQuery] string fileType)
        {
            {
                fileType = fileType.ToLower();
                switch (fileType)
                {
                    case "xml":
                        var xmlData = await _businessCardService.ExportBusinessCardsToXmlAsync();
                        return File(Encoding.UTF8.GetBytes(xmlData), "application/xml", "BusinessCards.xml");

                    case "csv":
                        var csvData = await _businessCardService.ExportBusinessCardsToCsvAsync();
                        return File(Encoding.UTF8.GetBytes(csvData), "text/csv", "BusinessCards.csv");

                    default:
                        return BadRequest();
                }
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] string? name, [FromQuery] DateTime? dob, [FromQuery] string? phone, [FromQuery] string? gender, [FromQuery] string? email)
        {
            var cards = await _businessCardService.GetFilteredBusinessCardsAsync(name, dob, phone, gender, email);
            return Ok(cards);
        }
    } 
}
