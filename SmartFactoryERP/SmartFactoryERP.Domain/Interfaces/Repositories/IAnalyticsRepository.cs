using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    // We can't use the DTO here because DTOs are in Application layer.
    // So we either return a Tuple, a Domain ValueObject, or specific values.
    // For simplicity in this tutorial, let's return specific values or a generic dictionary, 
    // BUT the cleanest way is to move the DTO definition to a "Shared" folder or return primitive types.
    // Let's keep it simple: The Repository will return the DTO (We will cheat slightly and refer to the Application DTO 
    // OR better: we implement the logic in the handler using existing repositories? No, that's slow.)

    // CORRECTION: The best architectural approach here is to keep the Repository returning pure data.
    // Let's define a "StatsModel" in Domain, or just query count/sum in the implementation.

    
        public interface IAnalyticsRepository
        {
            Task<int> GetTotalMaterialsAsync(CancellationToken token);
            Task<int> GetLowStockCountAsync(CancellationToken token);
            Task<int> GetPendingSalesCountAsync(CancellationToken token);
            Task<decimal> GetPendingSalesRevenueAsync(CancellationToken token);
            Task<int> GetActiveProductionCountAsync(CancellationToken token);
        }
    }
