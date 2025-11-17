using MediatR;
using SmartFactoryERP.Domain.Entities.Inventory;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Domain.Enums;
using SmartFactoryERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Inventory.Commands.CreateMaterial
{
    public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, int>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork; // لحفظ التغييرات

        // 1. حقن (Inject) الـ Interfaces اللي من الـ Domain
        public CreateMaterialCommandHandler(IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork)
        {
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateMaterialCommand command, CancellationToken cancellationToken)
        {
            // 2. التحقق من قاعدة بيزنس (التكرار) باستخدام الـ Repository
            var existingMaterial = await _inventoryRepository.GetMaterialByCodeAsync(command.MaterialCode, cancellationToken);
            if (existingMaterial != null)
            {
                // ارمي ValidationException (من الهيكل)
                throw new Exception($"Material with code {command.MaterialCode} already exists.");
            }

            // 3. استخدام "الكيان الذكي" (Smart Entity) من الـ Domain لإنشاء الكيان
            var material = Material.CreateNew(
                command.MaterialCode,
                command.MaterialName,
                command.MaterialType,
                command.UnitOfMeasure,
                command.UnitPrice,
                command.MinimumStockLevel
            );

            // 4. إضافة الكيان عن طريق الـ Repository
            await _inventoryRepository.AddMaterialAsync(material, cancellationToken);

            // 5. حفظ كل التغييرات في "ترانزكشن" واحدة عن طريق الـ Unit of Work
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 6. إرجاع الـ ID الجديد
            return material.Id;
        }
    }
}
