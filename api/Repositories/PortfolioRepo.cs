using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PortfolioRepo : IPortfolioRepo
    {
        private readonly AppDbContext _context;

        public PortfolioRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Stock>> GetUserPortfolioAsync(AppUser user)
        {
            return await _context.Portfolios.Where(x => x.AppUser.Id == user.Id)
                .Select(stock => new Stock
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap,
                    Comments = stock.Stock.Comments,
                }).ToListAsync();
        }
    }
}