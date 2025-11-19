using SmartFactoryERP.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Sales
{
    public class Customer : BaseAuditableEntity, IAggregateRoot
    {
        public string CustomerName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Address { get; private set; }
        public string TaxNumber { get; private set; } // Important for invoices
        public decimal CreditLimit { get; private set; } // Maximum amount they can owe
        public bool IsActive { get; private set; }

        private Customer() { }

        public static Customer Create(string name, string email, string phone, string address, string taxNumber, decimal creditLimit)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Customer Name is required.");

            return new Customer
            {
                CustomerName = name,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                TaxNumber = taxNumber,
                CreditLimit = creditLimit,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };
        }

        public void UpdateDetails(string name, string email, string phone, string address, string taxNumber, decimal creditLimit)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Name is required");

            CustomerName = name;
            Email = email;
            PhoneNumber = phone;
            Address = address;
            TaxNumber = taxNumber;
            CreditLimit = creditLimit;
        }
    }
}
