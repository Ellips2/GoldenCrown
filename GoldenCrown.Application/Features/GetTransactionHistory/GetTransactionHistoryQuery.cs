using GoldenCrown.Application.DTOs.Finance;
using MediatR;

namespace GoldenCrown.Application.Features.GetTransactionHistory
{
    public class GetTransactionHistoryQuery : IRequest<Result<IEnumerable<TransactionHistoryDto>>>
    {
        public int UserId;
        public DateTime? DateFrom;
        public DateTime? DateTo;
        public int Skip;
        public int Take;

        public GetTransactionHistoryQuery(int userId, DateTime? dateFrom, DateTime? dateTo, int skip, int take)
        {
            UserId = userId;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Skip = skip;
            Take = take;
        }
    }
}
