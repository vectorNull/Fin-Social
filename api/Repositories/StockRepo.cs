using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepo : IStockRepo
    {
        private readonly AppDbContext _context;

        public StockRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (stockModel is null)
            {
                return null;
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stocks
                .Include(c => c.Comments)
                .AsQueryable();

            stocks = FilterStocks(stocks, query);
            stocks = SortStocks(stocks, query);
            stocks = PaginateStocks(stocks, query);

            return await stocks.ToListAsync();
        }

        private IQueryable<Stock> SortStocks(IQueryable<Stock> stocks, QueryObject query)
        {
            if (string.IsNullOrWhiteSpace(query.SortBy)) return stocks;

            return query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)
                ? (query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol))
                : stocks;
        }

        private IQueryable<Stock> FilterStocks(IQueryable<Stock> stocks, QueryObject query)
        {
            return stocks
                .Where(s => string.IsNullOrWhiteSpace(query.CompanyName) || s.CompanyName.Contains(query.CompanyName))
                .Where(s => string.IsNullOrWhiteSpace(query.Symbol) || s.Symbol.Contains(query.Symbol));
        }

        private IQueryable<Stock> PaginateStocks(IQueryable<Stock> stocks, QueryObject query)
        {
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return stocks.Skip(skipNumber).Take(query.PageSize);
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            var stockModel = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);

            if (stockModel is null)
            {
                return null;
            }

            return stockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stock is null)
            {
                return null;
            }

            stock.Symbol = stockDto.Symbol;
            stock.CompanyName = stockDto.CompanyName;
            stock.Purchase = stockDto.Purchase;
            stock.LastDiv = stockDto.LastDiv;
            stock.Industry = stockDto.Industry;
            stock.MarketCap = stockDto.MarketCap;

            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<bool> StockExists(int stockId)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == stockId);
        }
    }
}
