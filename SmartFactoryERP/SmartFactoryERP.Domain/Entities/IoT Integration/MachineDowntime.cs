using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.IoT_Integration
{
    public class MachineDowntime
    {
        public int DowntimeID { get; set; } // (PK)
        public int MachineID { get; set; } // (FK)
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DowntimeReason { get; set; }

        // (يمكن حساب المدة، ولكن سأضيفها كـ TimeSpan)
        public TimeSpan Duration { get; set; }

        public string ReportedBy { get; set; } // (مؤقتاً string)

        // Navigation Property
        public virtual Machine Machine { get; set; }
    }
}
