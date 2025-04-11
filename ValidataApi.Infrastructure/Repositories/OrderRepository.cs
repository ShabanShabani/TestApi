using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Domain.Entities;
using ValidataApi.Domain.Interfaces;
using ValidataApi.Infrastructure.Data;

namespace ValidataApi.Infrastructure.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToListAsync();
        }

        public void Add(Order entity) => _context.Orders.Add(entity);
        public void Update(Order entity) => _context.Orders.Update(entity);
        public void Delete(Order entity) => _context.Orders.Remove(entity);
    }
}
