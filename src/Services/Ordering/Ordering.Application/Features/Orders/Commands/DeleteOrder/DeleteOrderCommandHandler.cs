using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrdersRepository ordersRepository, IMapper mapper,
        ILogger<DeleteOrderCommandHandler> logger)
    {
        _ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToDelete = await _ordersRepository.GetByIdAsync(request.Id);
        if (orderToDelete == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }

        await _ordersRepository.DeleteAsync(orderToDelete);
        _logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted.");
        
        return Unit.Value;
    }
}