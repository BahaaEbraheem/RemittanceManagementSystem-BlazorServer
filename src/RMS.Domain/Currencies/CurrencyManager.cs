using JetBrains.Annotations;
using RMS.Remittances;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace RMS.Currencies
{
    public class CurrencyManager : DomainService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IRemittanceRepository _remittanceRepository;

        public CurrencyManager(ICurrencyRepository currencyRepository,
            IRemittanceRepository remittanceRepository)
        {
            _currencyRepository = currencyRepository;
            _remittanceRepository = remittanceRepository;
        }

        
        public Task IsCurrencyUsedBeforInRemittance(Guid id)
        {
            Check.NotNull(id, nameof(id));
            var remittancequeryable = _remittanceRepository.GetQueryableAsync().Result.ToList();
            var remittance = remittancequeryable.Where(a => a.CurrencyId == id && a.IsDeleted == false).FirstOrDefault();
            if (remittance != null)
            {
                string remittanceSerial = remittance.SerialNumber;
                throw new CurrencyAlreadyUsedInRemittanceException(remittanceSerial);
            }
            return Task.CompletedTask;
        }
    }
}
