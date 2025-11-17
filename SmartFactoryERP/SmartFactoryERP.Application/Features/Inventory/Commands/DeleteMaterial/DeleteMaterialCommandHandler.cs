using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.DeleteMaterial
{
    public class DeleteMaterialCommandHandler : IRequestHandler<DeleteMaterialCommand, Unit>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMaterialCommandHandler(IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork)
        {
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب الكيان
            var materialToDelete = await _inventoryRepository.GetMaterialByIdAsync(request.Id, cancellationToken);

            // 2. التحقق من وجوده
            if (materialToDelete == null)
            {
                // throw new NotFoundException(nameof(Material), request.Id);
                throw new Exception($"Material with Id {request.Id} was not found."); // مؤقتاً
            }

            // 3. استدعاء "سلوك" الحذف من الـ Domain
            materialToDelete.Delete();

            // 4. حفظ التغييرات (EF Core سيقوم بـ UPDATE IsDeleted = true)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
