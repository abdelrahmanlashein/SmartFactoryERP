using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.IoT_Integration
{
    public enum MachineDataStatus
    {
        Running,
        Idle,
        Stopped
    }
    public class MachineData
    {
        public long DataID { get; set; } // (PK) - استخدمت long لأنه سيتراكم بسرعة
        public int MachineID { get; set; } // (FK)
        public DateTime Timestamp { get; set; }
        public MachineDataStatus Status { get; set; }

        // (سأستخدم decimal للدقة)
        public decimal Speed { get; set; }
        public decimal Temperature { get; set; }
        public decimal Pressure { get; set; }
        public int ProductionCount { get; set; }

        // Navigation Property
        public virtual Machine Machine { get; set; } 
    }
}
