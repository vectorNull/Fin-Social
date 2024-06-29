using api.Models;

namespace api.Interfaces
{
    public interface IFinancialModelingPrepService
    {
        Task<Stock?> FindStockBySymbolAsync(string symbol);
    }
}