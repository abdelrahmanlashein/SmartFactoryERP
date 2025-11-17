using SmartFactoryERP.Domain.Interfaces.Repositories;
using SmartFactoryERP.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            // ببساطة، هو ينادي SaveChanges() على الـ DbContext
            return await _context.SaveChangesAsync(cancellationToken);
        }

        // ملاحظة: الـ IUnitOfWork الحقيقي قد يحتوي على Repositories
        // لكن لفصل الاهتمامات (SoC)، سنكتفي بهذا ونقوم بحقن (Inject)
        // الـ Repositories منفصلة في الـ Handler.
    }
}
