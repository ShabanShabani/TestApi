using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Application.DTOs;
using ValidataApi.Domain.Entities;
using ValidataApi.Infrastructure.UnitOfWork;

namespace ValidataApi.Application.Commands
{
    public class UpdateOrderCommand : IRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Items are required.")]
        [MinLength(1, ErrorMessage = "At least one item is required.")]
        public List<ItemDto> Items { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateOrderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating order {OrderId}", command.Id);
            var order = await _unitOfWork.Orders.GetByIdAsync(command.Id);
            if (order == null)
            {
                _logger.LogWarning("Order not found: {OrderId}", command.Id);
                throw new KeyNotFoundException("Order not found.");
            }

            var items = command.Items.Select(dto => new Item(new Product(dto.ProductName, dto.ProductPrice), dto.Quantity)).ToList();
            order.Update(command.OrderDate, items);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} updated", command.Id);
        }
    }
}
