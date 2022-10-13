using Microsoft.AspNetCore.Authorization;
using RMS.Currencies;
using RMS.Permissions;
using RMS.Remittances.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
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
using Volo.Abp.Users;
using RMS.Status;
using RMS.Status.Dtos;
using static RMS.Permissions.RMSPermissions;
using RMS.Customers;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using Nito.Disposables.Internals;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Concurrent;

namespace RMS.Remittances
{
    public class RemittanceAppService : RMSAppService, IRemittanceAppService, ITransientDependency
    {
        private readonly IRemittanceRepository _remittanceRepository;
        private readonly RemitanceStatusManager _remittanceStatusManager;
        private readonly IRemittanceStatusAppService _remittanceStatusAppService;
        private readonly RemittanceManager _remittanceManager;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRepository<RemittanceStatus, Guid> _remittanceStatusRepository;
        private readonly IRepository<IdentityUser, Guid> _userRepository;
        private readonly IdentityUserManager _identityUserManager;

        public RemittanceAppService(
            IdentityUserManager identityUserManager,
            ICustomerRepository customerRepository,
            IRepository<RemittanceStatus, Guid> remittanceStatusRepository,
            IRemittanceRepository remittanceRepository,
            IRemittanceStatusAppService remittanceStatusAppService,
            RemittanceManager remittanceManager,
            ICurrencyRepository currencyRepository,
            RemitanceStatusManager remittanceStatusManager,
           IRepository<IdentityUser, Guid> userRepository)
        {
            _remittanceRepository = remittanceRepository;
            _customerRepository = customerRepository;
            _remittanceManager = remittanceManager;
            _currencyRepository = currencyRepository;
            _userRepository = userRepository;
            _remittanceStatusRepository = remittanceStatusRepository;
            _remittanceStatusAppService = remittanceStatusAppService;
            _remittanceStatusManager = remittanceStatusManager;
            _identityUserManager = identityUserManager;

            //GetPolicyName = RMSPermissions.Remittances.Default;
            //GetListPolicyName = RMSPermissions.Remittances.Default;
            //CreatePolicyName = RMSPermissions.Remittances.Create;
            //UpdatePolicyName = RMSPermissions.Remittances.Edit;
            //DeletePolicyName = RMSPermissions.Remittances.Create;
        }
        //[Authorize(RMSPermissions.Remittances.Create)]

        public async Task<RemittanceDto> CreateAsync(CreateRemittanceDto input)
        {
        
            var remittance = await _remittanceManager.CreateAsync(
                input.Amount, input.Type,
                input.ReceiverFullName,
                input.CreationTime,
                input.CurrencyId,
                input.SenderBy
            );
            await _remittanceRepository.InsertAsync(remittance);

            var remittanceStatus = await _remittanceStatusManager.CreateAsync(remittance.Id, Remittance_Status.Draft);
            await _remittanceStatusRepository.InsertAsync(remittanceStatus);
            return ObjectMapper.Map<Remittance, RemittanceDto>(remittance);
        }

        [Authorize(RMSPermissions.Remittances.Delete)]

        public async Task DeleteAsync(Guid id)
        {
            await _remittanceRepository.DeleteAsync(id);
        }

        public async Task<RemittanceDto> GetAsync(Guid id)
        {
            //var remittance = await _remittanceRepository.GetAsync(id);
            //return ObjectMapper.Map<Remittance, RemittanceDto>(remittance);


            //Get the IQueryable<Remittance> from the repository
            var queryable = await _remittanceRepository.GetQueryableAsync();

            //Prepare a query to join remittances and currencies
            var query = from remittance in queryable
                        join currency in await _currencyRepository.GetQueryableAsync() on remittance.CurrencyId equals currency.Id
                        where remittance.Id == id
                        select new { remittance, currency };

            //Execute the query and get the remittance with currency
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Remittance), id);
            }

            var remittanceDto = ObjectMapper.Map<Remittance, RemittanceDto>(queryResult.remittance);
            remittanceDto.CurrencyName = queryResult.currency.Name;
            return remittanceDto;







        }

        public async Task<PagedResultDto<RemittanceDto>> GetListAsync(GetRemittanceListDto input)
        {
            IdentityUser identityUser = await _identityUserManager.GetByIdAsync(CurrentUser.GetId());
            if (identityUser.Equals(null))
            {
                throw new ArgumentNullException("Who Are You Please SignIn");
            }
            var roles = await _identityUserManager.GetRolesAsync(identityUser);
            //Get the IQueryable<remittance> from the repository
            var remittancequeryable =  _remittanceRepository.GetQueryableAsync().Result.ToList();
            var currencyequeryable =  _currencyRepository.GetQueryableAsync().Result.ToList();
            var userequeryable =  _userRepository.GetQueryableAsync().Result.ToList();
            var remittance_Statusqueryable =  _remittanceStatusRepository.GetQueryableAsync().Result.ToList();
            var customerqueryable =  _customerRepository.GetQueryableAsync().Result.ToList();

            var remittanceStatusQyery = from remittance_Status in remittance_Statusqueryable
                                        group remittance_Status by remittance_Status.RemittanceId into remittance_Status
                                        select remittance_Status.OrderByDescending(t => t.CreationTime).FirstOrDefault();

            var query = from remittance in remittancequeryable
                        join currency in currencyequeryable
                        on remittance.CurrencyId equals currency.Id
                        join senderCustomer in customerqueryable
                        on remittance.SenderBy equals senderCustomer.Id
                        join receiverCustomer in customerqueryable
                       on remittance.SenderBy equals receiverCustomer.Id
                        join remittanceStatus in remittanceStatusQyery
                        on remittance.Id equals remittanceStatus.RemittanceId
                         where (remittanceStatus.State == Remittance_Status.Draft && roles.Contains("Creator")) ||
                         (remittanceStatus.State == Remittance_Status.Ready && roles.Contains("Supervisor")) ||
                         (remittanceStatus.State == Remittance_Status.Approved && roles.Contains("Releaser"))
                        select new { remittance, currency, remittanceStatus, senderCustomer, receiverCustomer };

            //Paging
            query = query
                .OrderBy(x => x.remittance.ReceiverFullName).ThenBy(x => x.remittanceStatus.State)
                //.OrderBy(NormalizeSorting(input.Sorting))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            //Execute the query and get a list
            //var queryResult = await AsyncExecuter.ToListAsync(query);


            //Convert the query result to a list of RemittanceDto objects
            var remittanceDtos = query.Select(x =>
            {
                var remittanceDto = ObjectMapper.Map<Remittance, RemittanceDto>(x.remittance);

                remittanceDto.CurrencyName = _currencyRepository.GetAsync(x.remittance.CurrencyId).Result.Name;
                //remittanceDto.CreatrorName = x.user.UserName;
                remittanceDto.State = x.remittanceStatus.State;
                remittanceDto.SenderName = x.senderCustomer.FirstName+" " +x.senderCustomer.FatherName+ " " + x.senderCustomer.LastName;
               remittanceDto.ReceiverName = x.receiverCustomer.FirstName + " " + x.receiverCustomer.FatherName + " " + x.receiverCustomer.LastName;
                return remittanceDto;
            }).ToList();

            //Get the total count with another query
            var totalCount = await _remittanceRepository.GetCountAsync();

            return new PagedResultDto<RemittanceDto>(
                totalCount,
                remittanceDtos
            );

        }

        //[Authorize(RMSPermissions.Remittances.Edit)]
        public async Task UpdateAsync(Guid id, UpdateRemittanceDto input)
        {
            var remittance = await _remittanceRepository.GetAsync(id);

            var CheckRemittanceIfApproved = await _remittanceManager.UpdateAsync(remittance,
                input.Amount, input.Type,
                input.ReceiverFullName,input.CurrencyId);
                remittance.Amount = input.Amount;
                remittance.Type = input.Type;
                remittance.ReceiverFullName = input.ReceiverFullName;
                remittance.CurrencyId = input.CurrencyId;
                remittance.SenderBy=input.SenderBy;

            await _remittanceRepository.UpdateAsync(remittance);

            //var remittanceStatus = await _remittanceStatusManager.UpdateAsync(remittance.Id, Remittance_Status.Ready);
            //await _remittanceStatusRepository.UpdateAsync(remittanceStatus);

        }

        public async Task SetReady(RemittanceDto input)
        {
           var remittanceStatus = await _remittanceStatusManager.UpdateAsync(input.Id);
            var remittance = await _remittanceRepository.GetAsync(input.Id);

            switch (remittanceStatus.State)
            {
                case Remittance_Status.Draft:
                    remittanceStatus.State = Remittance_Status.Ready;
                    remittance.LastModifierId=CurrentUser.Id;
                    remittance.LastModificationTime = DateTime.Now;

                    break;

                case Remittance_Status.Ready:
                    remittanceStatus.State = Remittance_Status.Approved;
                    remittance.ApprovedBy = CurrentUser.Id;
                    remittance.ApprovedDate = DateTime.Now;
                    break;

                case Remittance_Status.Approved:
                    remittanceStatus.State = Remittance_Status.Release;
                    remittance.ReleasedBy = CurrentUser.Id;
                    remittance.ReleasedDate = DateTime.Now;
                    remittance.ReceiverBy = input.ReceiverBy;
                    remittance.ReceiverFullName = input.ReceiverFullName;

                    break;

                default:
                    remittanceStatus.State = Remittance_Status.Draft;
                    break;
            }

            //var remittanc = ObjectMapper.Map<RemittanceDto, Remittance>(input);
            await _remittanceRepository.UpdateAsync(remittance);

            await _remittanceStatusRepository.InsertAsync(remittanceStatus);

        }

        public async Task<ListResultDto<CurrencyLookupDto>> GetCurrencyLookupAsync()
        {
            var currencies = await _currencyRepository.GetListAsync();

            return new ListResultDto<CurrencyLookupDto>(
                ObjectMapper.Map<List<Currency>, List<CurrencyLookupDto>>(currencies)
            );
        }

        private static string NormalizeSorting(string sorting)
        {
            if (sorting.IsNullOrEmpty())
            {
                return $"remittance.{nameof(Remittance.ReceiverFullName)}";
            }

            if (sorting.Contains("receiverFullName", StringComparison.OrdinalIgnoreCase))
            {
                return sorting.Replace(
                    "receiverFullName",
                    "remittance.receiverFullName",
                    StringComparison.OrdinalIgnoreCase
                );
            }

            return $"remittance.{sorting}";
        }

        public async Task<ListResultDto<UserLookupDto>> GetUserLookupAsync()
        {
            var users = await _userRepository.GetListAsync();

            return new ListResultDto<UserLookupDto>(
                ObjectMapper.Map<List<IdentityUser>, List<UserLookupDto>>(users)
            );
        }

        public async Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync()
        {
            var customers = await _customerRepository.GetListAsync();

            return new ListResultDto<CustomerLookupDto>(
                ObjectMapper.Map<List<Customer>, List<CustomerLookupDto>>(customers)
            );
        }

        public async Task<PagedResultDto<RemittanceDto>> GetListRemittancesStatusAsync(GetRemittanceListDto input)
        {
            IdentityUser identityUser = await _identityUserManager.GetByIdAsync(CurrentUser.GetId());
            if (identityUser.Equals(null))
            {
                throw new ArgumentNullException("Who Are You Please SignIn");
            }
            var roles = await _identityUserManager.GetRolesAsync(identityUser);
            //Get the IQueryable<remittance> from the repository
            var remittancequeryable = _remittanceRepository.GetQueryableAsync().Result.ToList();
            var currencyequeryable = _currencyRepository.GetQueryableAsync().Result.ToList();
            var userequeryable = _userRepository.GetQueryableAsync().Result.ToList();
            var remittance_Statusqueryable = _remittanceStatusRepository.GetQueryableAsync().Result.ToList();
            var customerqueryable = _customerRepository.GetQueryableAsync().Result.ToList();

            var remittanceStatusQyery = from remittance_Status in remittance_Statusqueryable
                                        group remittance_Status by remittance_Status.RemittanceId into remittance_Status
                                        select remittance_Status.OrderByDescending(t => t.CreationTime).FirstOrDefault();

            var query = from remittance in remittancequeryable
                        join currency in currencyequeryable
                        on remittance.CurrencyId equals currency.Id
                        join senderCustomer in customerqueryable
                        on remittance.SenderBy equals senderCustomer.Id
                        join receiverCustomer in customerqueryable
                       on remittance.SenderBy equals receiverCustomer.Id
                        join remittanceStatus in remittanceStatusQyery
                        on remittance.Id equals remittanceStatus.RemittanceId
                        //where (remittanceStatus.State == Remittance_Status.Draft && roles.Contains("Creator")) ||
                        //(remittanceStatus.State == Remittance_Status.Ready && roles.Contains("Supervisor")) ||
                        //(remittanceStatus.State == Remittance_Status.Approved && roles.Contains("Releaser"))
                        select new { remittance, currency, remittanceStatus, senderCustomer, receiverCustomer };

            //Paging
            query = query
                .OrderBy(x => x.remittance.ReceiverFullName).ThenBy(x => x.remittanceStatus.State)
                //.OrderBy(NormalizeSorting(input.Sorting))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            //Execute the query and get a list
            //var queryResult = await AsyncExecuter.ToListAsync(query);


            //Convert the query result to a list of RemittanceDto objects
            var remittanceDtos = query.Select(x =>
            {
                var remittanceDto = ObjectMapper.Map<Remittance, RemittanceDto>(x.remittance);

                remittanceDto.CurrencyName = _currencyRepository.GetAsync(x.remittance.CurrencyId).Result.Name;
                //remittanceDto.CreatrorName = x.user.UserName;
                remittanceDto.State = x.remittanceStatus.State;
                remittanceDto.SenderName = x.senderCustomer.FirstName + " " + x.senderCustomer.FatherName + " " + x.senderCustomer.LastName;
                remittanceDto.ReceiverName = x.receiverCustomer.FirstName + " " + x.receiverCustomer.FatherName + " " + x.receiverCustomer.LastName;
                return remittanceDto;
            }).ToList();

            //Get the total count with another query
            var totalCount = await _remittanceRepository.GetCountAsync();

            return new PagedResultDto<RemittanceDto>(
                totalCount,
                remittanceDtos
            );

        }
    }



}

