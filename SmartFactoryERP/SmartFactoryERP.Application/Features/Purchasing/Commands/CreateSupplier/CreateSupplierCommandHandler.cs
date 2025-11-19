using MediatR;
using SmartFactoryERP.Domain.Entities.Purchasing;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Purchasing.Commands.CreateSupplier
{
    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, int>
    {
        private readonly IPurchasingRepository _purchasingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSupplierCommandHandler(IPurchasingRepository purchasingRepository, IUnitOfWork unitOfWork)
        {
            _purchasingRepository = purchasingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            // 1. التحقق من عدم تكرار كود المورد
            var isDuplicate = await _purchasingRepository.IsSupplierCodeUniqueAsync(request.SupplierCode, cancellationToken);
            if (!isDuplicate) // لاحظ: الدالة بترجع true لو فريد (مش موجود)، فلو رجعت false يبقى مكرر (موجود)
            {
                // ملاحظة: اسم الدالة IsUnique، يعني لو رجعت false يبقى "مش فريد" (مكرر)
                // بس عشان اللخبطة، يفضل الدالة في الـ Repo تكون بترجع true لو الـ Code موجود
                // خلينا نفترض اننا هنظبطها في الـ implementation انها ترجع true لو الكود "متاح للاستخدام"
                // أو نغير اسمها لـ IsSupplierCodeExistsAsync عشان الوضوح.
                // *لنمشي مع العقد الحالي IsSupplierCodeUniqueAsync*:
                // لو رجعت false يبقى الكود ده مستخدم قبل كده.
                throw new Exception($"Supplier code '{request.SupplierCode}' already exists.");
            }

            // 2. إنشاء الكيان باستخدام Factory Method
            var supplier = Supplier.Create(
                request.SupplierCode,
                request.SupplierName,
                request.ContactPerson,
                request.PhoneNumber,
                request.Email,
                request.Address
            );

            // 3. الإضافة للـ Repository
            await _purchasingRepository.AddSupplierAsync(supplier, cancellationToken);

            // 4. الحفظ
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return supplier.Id;
        }
    }
}
