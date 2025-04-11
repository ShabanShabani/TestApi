using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Domain.Entities;
using ValidataApi.Domain.Interfaces;
using ValidataApi.Infrastructure.Data;
using ValidataApi.Infrastructure.Repositories;

namespace ValidataApi.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IRepository<Customer> Customers { get; }
        public IRepository<Order> Orders { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Customers = new CustomerRepository(_context);
            Orders = new OrderRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        IRepository<Customer> Customers { get; }
        IRepository<Order> Orders { get; }
        Task<int> SaveChangesAsync();
    }
}
