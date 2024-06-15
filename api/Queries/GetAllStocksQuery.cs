using api.DTOs.Stock;
using api.Helpers;
using api.Models;
using MediatR;

namespace api.Queries
{
    public class GetAllStocksQuery : IRequest<List<StockDto>>
    {
        public QueryObject QueryObject { get; }

        public GetAllStocksQuery(QueryObject queryObject)
        {
            QueryObject = queryObject;
        }
    }
}