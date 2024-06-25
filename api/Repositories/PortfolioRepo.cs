using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
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
        public async Task<List<StockDto>> GetUserPortfolioAsync(AppUser user)
        {
            return await _context.Portfolios.Where(x => x.AppUserId == user.Id)
                .Select(stock => stock.Stock.ToStockDto())
                .ToListAsync();
        }
    }
}