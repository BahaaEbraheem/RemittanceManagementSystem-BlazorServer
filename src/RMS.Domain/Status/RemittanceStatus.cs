using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using static RMS.Enums.Enums;

namespace RMS.Status
{
    public class RemittanceStatus : FullAuditedEntity<Guid>
    {
        public Guid RemittanceId { get; protected set; }
        public Remittance_Status State { get; set; }
        private RemittanceStatus() { }
        public RemittanceStatus(Guid id,Guid remittanceId, Remittance_Status state)
        {
            Id = id;
            RemittanceId = remittanceId;
            State = state;
        }
        public override object[] GetKeys()
        {
            return new object[]{RemittanceId};
        }
    }
}
