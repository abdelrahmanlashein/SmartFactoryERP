using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        // هذه هي "الترانزكشن"
        // هو المسئول عن تجميع كل التغييرات وحفظها مرة واحدة
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
