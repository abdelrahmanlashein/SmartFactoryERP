using SmartFactoryERP.Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Purchasing
{
    public class Supplier : BaseAuditableEntity, IAggregateRoot
    {
        // BaseEntity بيوفرلك Id (بدل SupplierID)

        public string SupplierCode { get; private set; }
        public string SupplierName { get; private set; }
        public string ContactPerson { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public bool IsActive { get; private set; }

        // Constructor خاص لـ EF Core
        private Supplier() { }

        // Factory Method: الطريقة الوحيدة لإنشاء مورد
        public static Supplier Create(string supplierCode , string name, string contactPerson, string email, string phone, string address)
        {
            // 1. قواعد البيزنس (Validation)
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Supplier Name is required."); // استخدم DomainException مستقبلاً

            if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email))
                throw new Exception("At least one contact method (Phone or Email) is required.");

            // 2. إنشاء الكيان
            return new Supplier
            {
                SupplierCode = supplierCode, //  
                SupplierName = name,
                ContactPerson = contactPerson,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                IsActive = true, // المورد الجديد مفعل تلقائياً
                CreatedDate = DateTime.UtcNow
            };
        }

        // Method لتحديث البيانات لاحقاً
        public void UpdateDetails(string name, string contactPerson, string email, string phone, string address)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Name is required");

            SupplierName = name;
            ContactPerson = contactPerson;
            Email = email;
            PhoneNumber = phone;
            Address = address;
        }

        // Method للتعطيل (بدل الحذف)
        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
