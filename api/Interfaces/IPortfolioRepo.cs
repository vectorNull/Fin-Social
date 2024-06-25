using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepo
    {
        Task<List<StockDto>> GetUserPortfolioAsync(AppUser user);
    }
}