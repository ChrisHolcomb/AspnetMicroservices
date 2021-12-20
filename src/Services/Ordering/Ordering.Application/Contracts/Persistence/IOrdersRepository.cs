using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persistence;

public interface IOrdersRepository : IAsyncRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
}