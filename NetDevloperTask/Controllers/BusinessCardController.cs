using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDevloperTask.Data;
using NetDevloperTask.Models;
using NetDevloperTask.Services.interfaces;
using System.Text;

namespace NetDevloperTask.Controllers;

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
    public async Task<IActionResult> CreateBusinessCard([FromBody] BusinessCard businessCard)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdCard = await _businessCardService.CreateBusinessCardAsync(businessCard);
        return CreatedAtAction(nameof(GetBusinessCardById), new { id = createdCard.Id }, createdCard);
    }

    [HttpPost("import/file")]
    public async Task<IActionResult> ImportBusinessCardFromFile([FromBody] ImportFileDto importFile, [FromQuery] string fileType)
    {
        var createdCard = await _businessCardService.CreateBusinessCardFromFileAsync(importFile.FileData, fileType);
        if (createdCard == null)
            return BadRequest("Invalid File Data");

        return CreatedAtAction(nameof(GetBusinessCardById), new { id = createdCard.Id }, createdCard);
    }


    //[HttpPost("import/qr")]
    //public async Task<IActionResult> ImportBusinessCardFromQrCode([FromBody] string qrCodeData)
    //{
    //    var createdCard = await _businessCardService.CreateBusinessCardFromQrCodeAsync(qrCodeData);
    //    if (createdCard == null)
    //        return BadRequest("Invalid QR Code");

    //    return CreatedAtAction(nameof(GetBusinessCardById), new { id = createdCard.Id }, createdCard);
    //}

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBusinessCardById(int id)
    {
        var card = await _businessCardService.GetBusinessCardByIdAsync(id);
        if (card == null)
            return NotFound();

        return Ok(card);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBusinessCards()
    {
        var cards = await _businessCardService.GetAllBusinessCardsAsync();
        return Ok(cards);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBusinessCard(int id)
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

    // Export to XML
    [HttpGet("export/xml")]
    public async Task<IActionResult> ExportBusinessCardsToXml()
    {
        var xmlData = await _businessCardService.ExportBusinessCardsToXmlAsync();
        return File(Encoding.UTF8.GetBytes(xmlData), "application/xml", "BusinessCards.xml");
    }

    // Export to CSV
    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportBusinessCardsToCsv()
    {
        var csvData = await _businessCardService.ExportBusinessCardsToCsvAsync();
        return File(Encoding.UTF8.GetBytes(csvData), "text/csv", "BusinessCards.csv");
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilteredBusinessCards([FromQuery] string? name, [FromQuery] DateTime? dob, [FromQuery] string? phone, [FromQuery] string? gender, [FromQuery] string? email)
    {
        var cards = await _businessCardService.GetFilteredBusinessCardsAsync(name, dob, phone, gender, email);
        return Ok(cards);
    }

}
