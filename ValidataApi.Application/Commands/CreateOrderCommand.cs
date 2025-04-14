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
    public class CreateOrderCommand : IRequest<int>
    {
        [Range(1, int.MaxValue, ErrorMessage = "Customer ID must be valid.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Items are required.")]
        [MinLength(1, ErrorMessage = "At least one item is required.")]
        public List<ItemDto> Items { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateOrderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating order for CustomerId {CustomerId}", command.CustomerId);
            var customer = await _unitOfWork.Customers.GetByIdAsync(command.CustomerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found: {CustomerId}", command.CustomerId);
                throw new KeyNotFoundException("Customer not found.");
            }

            var items = command.Items.Select(dto => new Item(new Product(dto.ProductName, dto.ProductPrice), dto.Quantity)).ToList();
            var order = new Order(command.OrderDate, items);
            customer.AddOrder(order);
            _unitOfWork.Orders.Add(order);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Order created with Id {OrderId}", order.Id);
            return order.Id;
        }
    }
}
