using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Commands
{
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteOrderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting order {OrderId}", command.Id);
            var order = await _unitOfWork.Orders.GetByIdAsync(command.Id);
            if (order == null)
            {
                _logger.LogWarning("Order not found: {OrderId}", command.Id);
                throw new KeyNotFoundException("Order not found.");
            }

            _unitOfWork.Orders.Delete(order);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} deleted", command.Id);
        }
    }
}
