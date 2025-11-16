using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.IoT_Integration
{
    public enum MachineAlertType
    {
        Error,
        Warning,
        Critical
    }
    public class MachineAlert
    {
        public int AlertID { get; set; } // (PK)
        public int MachineID { get; set; } // (FK)
        public MachineAlertType AlertType { get; set; }
        public string AlertMessage { get; set; }
        public DateTime AlertTime { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedTime { get; set; } // Nullable
        public string ResolvedBy { get; set; } // (مؤقتاً string)

        // Navigation Property
        public virtual Machine Machine { get; set; }
    }
}
