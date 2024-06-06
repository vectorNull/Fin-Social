using api.Controllers;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;

namespace test_finsocial_api;

public class StockControllerTests
{
    private readonly Mock<IStockRepo> _mockStockRepo;
    private readonly StockController _controller;
    public StockControllerTests()
    {
        _mockStockRepo = new Mock<IStockRepo>();
        _controller = new StockController(_mockStockRepo.Object);
    }

    [Fact]
    public async void GetAll_ReturnsOKResult_With_ListOfStockDtos()
    {
        //* Arrange
        var query = new QueryObject
        {
            Symbol = "AAPL",
            CompanyName = "Apple",
            SortBy = "Name",
            IsDescending = false,
            PageNumber = 1,
            PageSize = 20
        };

        var stockList = new List<Stock>
        {
            new Stock
            {
                Id = 1,
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.82m,
                Industry = "Technology",
                MarketCap = 2500000000,
                Comments = new List<Comment>()
            },
            new Stock
            {
                Id = 2,
                Symbol = "GOOGL",
                CompanyName = "Alphabet Inc.",
                Purchase = 2800.00m,
                LastDiv = 0.00m,
                Industry = "Technology",
                MarketCap = 1500000000,
                Comments = new List<Comment>()
            },
        };

        _mockStockRepo.Setup(repo => repo.GetAllAsync(query)).ReturnsAsync(stockList);
        
        //* Act
        var result = await _controller.GetAll(query);

        //* Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<StockDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
        Assert.Equal("Apple Inc.", returnValue.First().CompanyName);
        Assert.Equal("AAPL", returnValue.First().Symbol);
    }

    [Fact]
    public async void GetById_ReturnsOKResult_With_StockDto()
    {
        //* Arrange
        
        var stockId = 1;
        var stock = new Stock
        {
             Id = stockId,
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.82m,
                Industry = "Technology",
                MarketCap = 2500000000,
                Comments = new List<Comment>()
        };

        _mockStockRepo.Setup(repo => repo.GetByIdAsync(stockId)).ReturnsAsync(stock);
        
        //* Act
        var result = await _controller.GetById(stockId);

        //* Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<StockDto>(okResult.Value);
        Assert.Equal(stockId, returnValue.Id);
        Assert.Equal("AAPL", returnValue.Symbol);
    }
};