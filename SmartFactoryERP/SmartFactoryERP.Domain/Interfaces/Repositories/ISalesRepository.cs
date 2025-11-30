using SmartFactoryERP.Domain.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    public interface ISalesRepository
    {
        // Customer Methods
        Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer> GetCustomerByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken);
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
        Task AddSalesOrderAsync(SalesOrder order, CancellationToken cancellationToken);
        Task<SalesOrder> GetSalesOrderWithItemsAsync(int id, CancellationToken cancellationToken);
        Task AddInvoiceAsync(Invoice invoice, CancellationToken cancellationToken);
        Task<Invoice> GetInvoiceByIdAsync(int id, CancellationToken token);
        // Future: Sales Orders
    }
}
