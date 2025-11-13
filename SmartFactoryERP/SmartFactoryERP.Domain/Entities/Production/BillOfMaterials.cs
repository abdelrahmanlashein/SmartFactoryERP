using SmartFactoryERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.Production
{
    public class BillOfMaterials
    {
        public int BOMID { get; set; } // (PK)

        // (FK -> Material) - المنتج النهائي الذي يتم تصنيعه
        public int FinishedGoodID { get; set; }

        // (FK -> Material) - المادة الخام المستخدمة في تصنيعه
        public int RawMaterialID { get; set; }

        public decimal RequiredQuantity { get; set; } // الكمية المطلوبة من المادة الخام
        public bool IsActive { get; set; }

        // Navigation Properties
        public virtual Material FinishedGood { get; set; }
        public virtual Material RawMaterial { get; set; }
    }
}
