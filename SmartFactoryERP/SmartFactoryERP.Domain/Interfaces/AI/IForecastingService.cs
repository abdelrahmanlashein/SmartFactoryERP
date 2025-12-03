using SmartFactoryERP.Domain.Models.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.AI
{
    public interface IForecastingService
    {
        // الدالة تأخذ تاريخ المبيعات وترجع التوقع
        ForecastResult PredictNextMonth(List<SalesHistoryRecord> history);
    }
}
