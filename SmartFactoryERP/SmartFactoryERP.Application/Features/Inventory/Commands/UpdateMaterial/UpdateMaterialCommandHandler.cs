using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.UpdateMaterial
{
    public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand, Unit>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMaterialCommandHandler(IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork)
        {
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب الكيان من الداتابيز
            var materialToUpdate = await _inventoryRepository.GetMaterialByIdAsync(request.Id, cancellationToken);

            // 2. التحقق من وجوده
            if (materialToUpdate == null)
            {
                // throw new NotFoundException(nameof(Material), request.Id);
                throw new Exception($"Material with Id {request.Id} was not found."); // مؤقتاً
            }

            // 3. استدعاء "السلوك" (Method) من الـ Domain لتنفيذ التعديل
            // هنا الـ Application لا يعرف "كيف" يتم التعديل، هو فقط "يطلب" التعديل
            materialToUpdate.UpdateDetails(
                request.MaterialName,
                request.UnitOfMeasure,
                request.UnitPrice,
                request.MinimumStockLevel
            );

            // 4. حفظ التغييرات
            // (لا نحتاج لـ "Update" صريح، EF Core يتتبع التغييرات)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value; // إرجاع void
        }
    }
}
