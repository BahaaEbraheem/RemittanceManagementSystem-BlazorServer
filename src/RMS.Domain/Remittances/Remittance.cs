using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
using RMS.Status;
using RMS.Remittances;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using static RMS.Enums.Enums;

namespace RMS.Remittances
{
   public class Remittance : FullAuditedAggregateRoot<Guid>
    {
        public string SerialNumber { get; private set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }

        [Display(Name = "Remittance Type")]
        public RemittanceType Type { get; set; } = RemittanceType.Internal;
        public Guid? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public Guid? ReleasedBy { get; set; }
        public DateTime? ReleasedDate { get; set; }

        public Guid SenderBy { get; set; }
        public Guid? ReceiverBy { get; set; }
        public string ReceiverFullName { get; set; }
        public Guid CurrencyId { get; set; }

        public ICollection<RemittanceStatus> Status { get; set; }

        private Remittance() { }
        public Remittance(Guid id, [NotNull] double amount,Guid? creatorId, RemittanceType type,string receiverFullName
            ,[NotNull]Guid currencyId,Guid senderBy) : base(id)
        {
            Id = id;
            SerialNumber= SetSerialNum(); 
            Amount=amount;
            CreatorId = creatorId;
            TotalAmount = amount+((amount * 5) / 100);
            Type = type;
            ReceiverFullName = receiverFullName;
            CurrencyId = currencyId;
            SenderBy = senderBy;
            Status = new Collection<RemittanceStatus>();
        }

        private string SetSerialNum()
        {
            Random rng = new Random();
            int number = rng.Next(1, RemittanceConsts.MaxLength);
            string serial = number.ToString("000000000");
           return SerialNumber = serial;
        }

        //public void AddState(Guid stateId)
        //{
        //    Check.NotNull(stateId, nameof(stateId));

        //    if (IsInState(stateId))
        //    {
        //        return;
        //    }
        //    Status.Add(new Remittance_Status(remittanceId: Id, stateId));
        //}
        //private bool IsInState(Guid stateId) 
        //{
        //    return
        //        Status.Any(x => x.StateId == stateId); 
        //}

    }
}



