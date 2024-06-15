using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Queries;
using MediatR;

namespace api.Handlers
{
    public class GetAllStocksHandler : IRequestHandler<GetAllStocksQuery, List<StockDto>>
    {
        private readonly IStockRepo _stockRepo;
        public GetAllStocksHandler(IStockRepo stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public async Task<List<StockDto>> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _stockRepo.GetAllAsync(request.QueryObject);
            return stocks.Select(s => s.ToStockDto()).ToList();
        }
    }
}