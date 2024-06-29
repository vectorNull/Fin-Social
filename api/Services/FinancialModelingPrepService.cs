using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using System.Text.Json;

namespace api.Services
{
    public class FinancialModelingPrepService : IFinancialModelingPrepService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public FinancialModelingPrepService(HttpClient httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<Stock?> FindStockBySymbolAsync(string symbol) 
        {
            try
            {
                var apiKey = _configuration["FinancialModelingPrep"];
                var url = $"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                
                if (response.IsSuccessStatusCode)
                {
                    // var content = await response.Content.ReadAsStringAsync();
                    var task = JsonSerializer.Deserialize<FMPStock[]>(response.Content.ReadAsStringAsync().Result);
                    var fmpStock = task?[0];

                    if (fmpStock == null)
                    {
                        return null;
                    }

                    return fmpStock.ToStockFromFMPStockDto();
                }

                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}