using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Application.Features.Production.Commands.CreateBOM
{
    // الطلب الرئيسي: إنشاء وصفة تصنيع كاملة لمنتج معين
    public class CreateBillOfMaterialCommand : IRequest<int>
    {
        public int ProductId { get; set; }      // المنتج النهائي (مثلاً: ترابيزة)
        
        // 👇 التغيير: قائمة المكونات بدلاً من مكون واحد
        public List<BomComponentDto> Components { get; set; } = new();
    }

    // كلاس مساعد: يمثل مكون واحد في الوصفة
    public class BomComponentDto
    {
        public int ComponentId { get; set; }    // المادة الخام (مثلاً: خشب، مسامير)
        public decimal Quantity { get; set; }   // الكمية المطلوبة (مثلاً: 4 أرجل)
    }
}
