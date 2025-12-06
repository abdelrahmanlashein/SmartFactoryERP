using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Interfaces.Identity
{
    public interface ICurrentUserService
    {
        string UserId { get; }      // الـ GUID بتاع حساب الدخول
        int? EmployeeId { get; }    // رقم الموظف المرتبط بالحساب
    }
}

