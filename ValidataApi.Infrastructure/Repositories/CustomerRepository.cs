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
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.Include(c => c.Orders).ToListAsync();
        }

        public void Add(Customer entity) => _context.Customers.Add(entity);
        public void Update(Customer entity) => _context.Customers.Update(entity);
        public void Delete(Customer entity) => _context.Customers.Remove(entity);
    }
}
