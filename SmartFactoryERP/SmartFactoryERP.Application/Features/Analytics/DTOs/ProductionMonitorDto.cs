using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Analytics.DTOs
{
    public class ProductionMonitorDto
    {
        public int PresentWorkersCount { get; set; } // ✅ تم تفعيله الآن
        public List<ActiveOrderDto> ActiveOrders { get; set; }
    }
}

