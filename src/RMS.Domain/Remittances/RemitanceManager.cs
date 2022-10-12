using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using RMS.Currencies;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;
using static RMS.Enums.Enums;
using Volo.Abp.Validation;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using Volo.Abp.ObjectMapping;
using RMS.Status;
using Volo.Abp.Users;
using RMS.Customers;

namespace RMS.Remittances
{
    public class RemittanceManager : DomainService
    {

        private readonly IRemittanceRepository _remittanceRepository;
        private readonly ICurrentUser _currentUser;



        public RemittanceManager(IRemittanceRepository remittanceRepository, ICurrentUser currentUser)
        {
            _remittanceRepository = remittanceRepository;
            _currentUser = currentUser;
        }



        public async Task<Remittance> CreateAsync(double amount, RemittanceType type,
            string receiverFullName ,DateTime CreationTime,Guid currencyId,Guid senderBy)
        {
            Check.NotNullOrWhiteSpace(amount.ToString(), nameof(amount));
            Check.NotNullOrWhiteSpace(receiverFullName, nameof(receiverFullName));

            if (string.IsNullOrEmpty(Enum.GetName(type)))
            {
                    type = RemittanceType.Internal;
            }
            //هل تم انشاء حوالة مسودة من قبل
            var existingDraftRemittance = await _remittanceRepository.FindRemittance_StillDraftAsync(amount, receiverFullName);
            if (existingDraftRemittance != null)
            {
                throw new ArgumentException(receiverFullName, "The Remittance Alredy Exist");
            }
            return new Remittance(
                 GuidGenerator.Create(),
                 amount,
                 _currentUser.Id,
                 type,
                receiverFullName,
                currencyId, senderBy
            );

           
        }

        public async Task<Remittance> UpdateAsync(Remittance remittance ,double amount, RemittanceType type,
            string receiverFullName,[NotNull] Guid currencyId)
        {
            // هل الحوالة تمت الموافقة عليها أم لا
            bool approvedRemittance = await _remittanceRepository.IsApprovedRemittanceAsync(remittance);

            if (!approvedRemittance)
            {
                throw new RemittanceAlreadyApprovedException();
            }
            Check.NotNullOrWhiteSpace(amount.ToString(), nameof(amount));
            Check.NotNullOrWhiteSpace(receiverFullName, nameof(receiverFullName));
            Check.NotNullOrWhiteSpace(currencyId.ToString(), nameof(currencyId));
           
            return remittance;
        }


    }
}