using api.DTOs.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol.ToLower(),
                CompanyName = stockDto.CompanyName.ToLower(),
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry.ToLower(),
                MarketCap = stockDto.MarketCap
            };
        }

        public static Stock ToStockFromFMPStockDto(this FMPStock fmpStock)
        {
            return new Stock
            {
                Symbol = fmpStock.symbol!.ToLower(),
                CompanyName = fmpStock.companyName!.ToLower(),
                Purchase = (decimal)fmpStock.price,
                LastDiv = (decimal)fmpStock.lastDiv,
                Industry = fmpStock.industry!.ToLower(),
                MarketCap = fmpStock.mktCap
            };
        }


    }
}