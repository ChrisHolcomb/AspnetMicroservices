using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;

    public UpdateOrderCommandHandler(IOrdersRepository ordersRepository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
    {
        _ordersRepository = ordersRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _ordersRepository.GetByIdAsync(request.Id);
        if (orderToUpdate == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }

        _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));

        await _ordersRepository.UpdateAsync(orderToUpdate);
        
        _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");
        
        return Unit.Value;
    }
}