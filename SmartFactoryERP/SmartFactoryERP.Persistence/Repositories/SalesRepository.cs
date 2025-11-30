using Microsoft.EntityFrameworkCore;
using SmartFactoryERP.Domain.Entities.Sales;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ApplicationDbContext _context;

        public SalesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
        }

        public async Task<List<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken)
        {
            return await _context.Customers.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Customer> GetCustomerByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Customers.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email)) return true;
            return !await _context.Customers.AnyAsync(c => c.Email == email, cancellationToken);
        }
        public async Task AddSalesOrderAsync(SalesOrder order, CancellationToken cancellationToken)
        {
            await _context.SalesOrders.AddAsync(order, cancellationToken);
        }
        public async Task<SalesOrder> GetSalesOrderWithItemsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.SalesOrders
                .Include(so => so.Items) // Load items eagerly
                .FirstOrDefaultAsync(so => so.Id == id, cancellationToken);
        }
        public async Task AddInvoiceAsync(Invoice invoice, CancellationToken cancellationToken)
        {
            await _context.Invoices.AddAsync(invoice, cancellationToken);
        }
        // SalesRepository.cs
        public async Task<Invoice> GetInvoiceByIdAsync(int id, CancellationToken token)
        {
            return await _context.Invoices
                .Include(i => i.SalesOrder)
                    .ThenInclude(so => so.Customer)
                .Include(i => i.SalesOrder)
                    .ThenInclude(so => so.Items) // Items loaded
                .FirstOrDefaultAsync(i => i.Id == id, token);
        }
    }
}
