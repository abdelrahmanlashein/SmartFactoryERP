using MediatR;
using SmartFactoryERP.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Analytics.DTOs
{
    public class GetProductionMonitorQuery : IRequest<ProductionMonitorDto> { }

    public class GetProductionMonitorQueryHandler : IRequestHandler<GetProductionMonitorQuery, ProductionMonitorDto>
    {
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IProductionRepository _productionRepo; // تأكد أن لديك هذا الريبو أو استخدم DbContext مباشرة للسرعة

        public GetProductionMonitorQueryHandler(IAttendanceRepository attendanceRepo, IProductionRepository productionRepo)
        {
            _attendanceRepo = attendanceRepo;
            _productionRepo = productionRepo;
        }

        public async Task<ProductionMonitorDto> Handle(GetProductionMonitorQuery request, CancellationToken token)
        {
            // 1. عدد العمال الحاضرين (من موديول HR اللي لسه مخلصينه)
            var presentCount = await _attendanceRepo.GetCurrentPresentCountAsync(DateTime.UtcNow.Date, token);

            // 2. الأوامر النشطة (Started)
            // Fix: Use GetAllProductionOrdersAsync and filter by status
            var allOrders = await _productionRepo.GetAllProductionOrdersAsync(token);
            var activeOrders = allOrders.Where(o => o.Status.ToString() == "Started").ToList();

            return new ProductionMonitorDto
            {
                PresentWorkersCount = presentCount,
                ActiveOrders = activeOrders.Select(o => new ActiveOrderDto
                {
                    OrderId = o.Id,
                    ProductName = o.Product?.MaterialName ?? "Unknown", // تأكد من الـ Includes
                    Quantity = o.Quantity,
                    StartDate = o.StartDate,
                    EstimatedEndDate = o.EndDate, // تأكد إنك ضفت EndDate في الانتيتي
                    Status = o.Status.ToString()
                }).ToList()
            };
        }
    }
}
