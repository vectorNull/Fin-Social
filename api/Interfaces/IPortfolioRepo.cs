using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepo
    {
        Task<List<Stock>> GetUserPortfolioAsync(AppUser user);
    }
}