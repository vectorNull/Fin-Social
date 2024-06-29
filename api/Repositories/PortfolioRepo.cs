using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Services;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class PortfolioRepo : IPortfolioRepo
    {
        private readonly AppDbContext _context;
        private readonly IStockRepo _stockRepo;
        private readonly IFinancialModelingPrepService _financialModelingPrepService;

        public PortfolioRepo(AppDbContext context, IStockRepo stockRepo, IFinancialModelingPrepService financialModelingPrepService)
        {
            _context = context;
            _stockRepo = stockRepo;
            _financialModelingPrepService = financialModelingPrepService;
        }

        public async Task<List<StockDto>> GetUserPortfolioAsync(AppUser user)
        {
            return await _context.Portfolios.Where(x => x.AppUserId == user.Id)
                .Select(stock => stock.Stock.ToStockDto())
                .ToListAsync();
        }

        public async Task<StockDto?> AddStockToPortfolioAsync(AppUser user, string symbol)
        {
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

             if (stock is null)
            {
                stock = await _financialModelingPrepService.FindStockBySymbolAsync(symbol);
                
                if (stock is null)
                {
                    return null;
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }

            var portfolio = new Portfolio
            {
                AppUserId = user.Id,
                StockId = stock.Id
            };

            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();

            return stock.ToStockDto();
        }

        public async Task<bool> RemoveStockFromPortfolioAsync(AppUser user, string symbol)
        {
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                return false;
            }

            var portfolio = await _context.Portfolios.FirstOrDefaultAsync(x => 
                x.AppUserId == user.Id && x.StockId == stock.Id);

            if (portfolio == null)
            {
                return false;
            }

            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}