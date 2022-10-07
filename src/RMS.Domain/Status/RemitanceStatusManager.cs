using JetBrains.Annotations;
using Microsoft.Extensions.Configuration.UserSecrets;
using RMS.Remittances;
using RMS.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;
using static RMS.Enums.Enums;

namespace RMS.Status
{
    public class RemitanceStatusManager : DomainService
    {
        private readonly IRemittanceRepository _remittanceRepository;
        private readonly IRemittanceStatusRepository _remittanceStatusRepository;
        private readonly ICurrentUser _currentUser;

        public RemitanceStatusManager(IRemittanceRepository remittanceRepository, ICurrentUser currentUser)
        {
            _remittanceRepository = remittanceRepository;
            _currentUser = currentUser;
        }




        public async Task<RemittanceStatus> CreateAsync(Guid remittanceId, Remittance_Status State)
        {

            return new RemittanceStatus(
                 GuidGenerator.Create(),
                 remittanceId, State

            );
           
        }


        public async Task<RemittanceStatus> UpdateAsync(Guid remittanceId,Remittance_Status newState)
        {
            Check.NotNullOrWhiteSpace(remittanceId.ToString(), nameof(remittanceId));

            var ExistRemitanceStatus =  _remittanceStatusRepository.FindByRemitanceIdAndStateAsync(remittanceId).Result;
            if (ExistRemitanceStatus!=null)
            {
                throw new ArgumentNullException();
            }
            return  new RemittanceStatus(
                 GuidGenerator.Create(),
                 remittanceId, newState

            );

        }

    }
}