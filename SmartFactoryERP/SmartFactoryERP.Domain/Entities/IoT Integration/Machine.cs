using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactoryERP.Domain.Entities.IoT_Integration
{
    public enum MachineStatus
    {
        Active,
        Maintenance,
        Inactive
    }
    public class Machine
    {
        public int MachineID { get; set; } // (PK)
        public string MachineCode { get; set; }
        public string MachineName { get; set; }
        public string MachineType { get; set; }
        public string Location { get; set; }
        public DateTime InstallationDate { get; set; }
        public MachineStatus Status { get; set; }
    }
}
