using Moq;
using Xunit;
using FluentAssertions;
using NetDevloperTask.Controllers;
using NetDevloperTask.Models;
using NetDevloperTask.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.IO;

public class BusinessCardControllerTests
{
    private readonly Mock<IBusinessCardService> _mockService;
    private readonly BusinessCardController _controller;

    public BusinessCardControllerTests()
    {
        _mockService = new Mock<IBusinessCardService>();
        _controller = new BusinessCardController(_mockService.Object);
    }

    // Test for CreateBusinessCard
    [Fact]
    public async Task CreateBusinessCard_ShouldReturnCreatedAtAction_WhenValidModel()
    {
        // Arrange
        var card = new BusinessCard { Id = 1, Name = "John Doe", Gender = "Male", DateOfBirth = new DateTime(1985, 2, 10), Email = "john@example.com", Phone = "1234567890", Address = "Address" };
        _mockService.Setup(s => s.CreateBusinessCardAsync(It.IsAny<BusinessCard>())).ReturnsAsync(card);

        // Act
        var result = await _controller.Create(card);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult.Value.Should().Be(card);
        createdResult.ActionName.Should().Be(nameof(BusinessCardController.GetById));
    }

    // Test for ImportBusinessCardFromFile - Null or empty file
    [Fact]
    public async Task ImportBusinessCardFromFile_ShouldReturnBadRequest_WhenFileIsNull()
    {
        // Arrange
        IFormFile file = null;

        // Act
        var result = await _controller.Import(file);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("No file uploaded");
    }

    [Fact]
    public async Task ImportBusinessCardFromFile_ShouldReturnBadRequest_WhenFileFormatIsUnsupported()
    {
        // Arrange
        var invalidFileName = "invalid.txt";

        // Mocking the IFormFile with some content and a wrong file extension
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns(invalidFileName);
        fileMock.Setup(f => f.Length).Returns(100); // Simulate a non-empty file

        // Act
        var result = await _controller.Import(fileMock.Object);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Unsupported file format");
    }

    [Fact]
    public async Task ImportBusinessCardFromFile_ShouldReturnOk_WhenFileIsCsvAndImportSucceeds()
    {
        // Arrange
        var validFileName = "valid.csv";
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns(validFileName);
        fileMock.Setup(f => f.Length).Returns(100); // Simulate a non-empty file

        var businessCards = new List<BusinessCard>
    {
        new BusinessCard { Id = 0, Name = "John Doe", Gender = "Male", DateOfBirth = new DateTime(1985, 2, 10), Email = "john@example.com", Phone = "1234567890", Address = "Address 1" },
        new BusinessCard { Id = 0, Name = "Jane Doe", Gender = "Female", DateOfBirth = new DateTime(1990, 5, 12), Email = "jane@example.com", Phone = "9876543210", Address = "Address 2" }
    };

        // Mocking the service method for CSV import
        _mockService.Setup(s => s.ImportBusinessCardsFromCsvAsync(It.IsAny<Stream>())).ReturnsAsync(businessCards);

        // Act
        var result = await _controller.Import(fileMock.Object);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedCards = okResult.Value as IEnumerable<BusinessCard>;
        returnedCards.Should().BeEquivalentTo(businessCards);
    }

    [Fact]
    public async Task ImportBusinessCardFromFile_ShouldReturnOk_WhenFileIsXmlAndImportSucceeds()
    {
        // Arrange
        var validFileName = "valid.xml";
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns(validFileName);
        fileMock.Setup(f => f.Length).Returns(100); // Simulate a non-empty file

        var businessCards = new List<BusinessCard>
    {
        new BusinessCard { Id = 0, Name = "John Doe", Gender = "Male", DateOfBirth = new DateTime(1985, 2, 10), Email = "john@example.com", Phone = "1234567890", Address = "Address 1" },
        new BusinessCard { Id = 0, Name = "Jane Doe", Gender = "Female", DateOfBirth = new DateTime(1990, 5, 12), Email = "jane@example.com", Phone = "9876543210", Address = "Address 2" }
    };

        // Mocking the service method for XML import
        _mockService.Setup(s => s.ImportBusinessCardsFromXmlAsync(It.IsAny<Stream>())).ReturnsAsync(businessCards);

        // Act
        var result = await _controller.Import(fileMock.Object);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedCards = okResult.Value as IEnumerable<BusinessCard>;
        returnedCards.Should().BeEquivalentTo(businessCards);
    }


    // Test for GetBusinessCardById
    [Fact]
    public async Task GetBusinessCardById_ShouldReturnNotFound_WhenCardDoesNotExist()
    {
        // Arrange
        int cardId = 1;
        _mockService.Setup(s => s.GetBusinessCardByIdAsync(cardId)).ReturnsAsync((BusinessCard)null);

        // Act
        var result = await _controller.GetById(cardId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetBusinessCardById_ShouldReturnOk_WhenCardExists()
    {
        // Arrange
        int cardId = 1;
        var card = new BusinessCard { Id = cardId, Name = "John Doe" };
        _mockService.Setup(s => s.GetBusinessCardByIdAsync(cardId)).ReturnsAsync(card);

        // Act
        var result = await _controller.GetById(cardId);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(card);
    }

    // Test for GetAllBusinessCards
    [Fact]
    public async Task GetAllBusinessCards_ShouldReturnOk_WhenCardsExist()
    {
        // Arrange
        var cards = new List<BusinessCard>
        {
            new BusinessCard { Id = 1, Name = "John Doe" },
            new BusinessCard { Id = 2, Name = "Jane Doe" }
        };
        _mockService.Setup(s => s.GetAllBusinessCardsAsync()).ReturnsAsync(cards);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(cards);
    }

    // Test for DeleteBusinessCard
    [Fact]
    public async Task DeleteBusinessCard_ShouldReturnNoContent_WhenCardIsDeleted()
    {
        // Arrange
        int cardId = 1;
        _mockService.Setup(s => s.DeleteBusinessCardAsync(cardId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(cardId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteBusinessCard_ShouldReturnNotFound_WhenCardDoesNotExist()
    {
        // Arrange
        int cardId = 1;
        _mockService.Setup(s => s.DeleteBusinessCardAsync(cardId)).Throws<KeyNotFoundException>();

        // Act
        var result = await _controller.Delete(cardId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    // Test for ExportBusinessCardsToXml
    [Fact]
    public async Task ExportBusinessCardsToXml_ShouldReturnXmlFile_WhenCardsExist()
    {
        // Arrange
        var xmlData = "<BusinessCards><BusinessCard><Id>1</Id><Name>John Doe</Name></BusinessCard></BusinessCards>";
        _mockService.Setup(s => s.ExportBusinessCardsToXmlAsync()).ReturnsAsync(xmlData);

        // Act
        var result = await _controller.Export("xml");

        // Assert
        var fileResult = result as FileContentResult;
        fileResult.Should().NotBeNull();
        fileResult!.FileContents.Should().NotBeEmpty();
        fileResult.ContentType.Should().Be("application/xml");
        fileResult.FileDownloadName.Should().Be("BusinessCards.xml");
    }

    [Fact]
    public async Task ExportBusinessCardsToCsv_ShouldReturnCsvFile_WhenCardsExist()
    {
        // Arrange
        var csvData = "Id,Name\n1,John Doe\n";
        _mockService.Setup(s => s.ExportBusinessCardsToCsvAsync()).ReturnsAsync(csvData);

        // Act
        var result = await _controller.Export("csv");

        // Assert
        var fileResult = result as FileContentResult;
        fileResult.Should().NotBeNull();
        fileResult!.FileContents.Should().NotBeEmpty();
        fileResult.ContentType.Should().Be("text/csv");
        fileResult.FileDownloadName.Should().Be("BusinessCards.csv");
    }

}
