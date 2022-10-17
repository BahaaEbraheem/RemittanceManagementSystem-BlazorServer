using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMS.Remittances.Dtos;
using RMS.Remittances;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Authorization;
using Volo.Abp.AspNetCore.Authentication;
using AutoMapper.Internal.Mappers;
using IdentityServer4.Models;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using RMS.Customers.Dtos;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.BlazoriseUI.Components;
using static RMS.Enums.Enums;
using RMS.Customers;
using RMS.Permissions;
using static RMS.Permissions.RMSPermissions;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic;

namespace RMS.Blazor.Pages.Remittances
{
    public partial class ApprovedRemittances
    {

        [Inject]
        private IReadOnlyList<RemittanceDto> RemittanceList { get; set; }
        [Inject]
        private IReadOnlyList<CustomerDto> CustomerList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        [Parameter]
        public EventCallback<string> OnSearchChanged { get; set; }
        public string SearchTerm { get; set; }
        CustomerPagedAndSortedResultRequestDto customerPagedAndSortedResultRequestDto = new CustomerPagedAndSortedResultRequestDto();
        IReadOnlyList<CurrencyLookupDto> currencyList = Array.Empty<CurrencyLookupDto>();
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }
        private string CurrentSortingCustomer { get; set; }
        private int CurrentPageCustomer { get; set; }
        private int TotalCount { get; set; }
        private string SelectedCurrency { get; set; }

        private CreateRemittanceDto NewRemittance { get; set; }
        private Guid EditingRemittanceId { get; set; }
        private UpdateRemittanceDto EditingRemittance { get; set; }
        private CreateUpdateCustomerDto NewCustomer { get; set; }
        private Modal CreateSearchCustomerModal { get; set; }
        private Modal ReleaseRemittanceModal { get; set; }
        private Modal CreateCustomerModal { get; set; }
        private Modal CreateRemittanceModal { get; set; }
        private Modal EditRemittanceModal { get; set; }
        private Validations CreateCustomerValidationsRef { get; set; }
        private Validations UpdateCustomerValidationsRef { get; set; }
        private Validations CreateValidationsRef { get; set; }

        private Validations EditValidationsRef;
        private bool CanCreateRemittance { get; set; }
        private bool CanEditRemittance { get; set; }
        private bool CanDeleteRemittance { get; set; }
        private bool CanApprovedRemittance { get; set; }
        private bool CanReleaseRemittance { get; set; }
        private bool CanReadyRemittance { get; set; }
        public ApprovedRemittances()
        {
            NewCustomer = new CreateUpdateCustomerDto();
            NewRemittance = new CreateRemittanceDto();
            EditingRemittance = new UpdateRemittanceDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetRemittancesAsync();
            currencyList = (await RemittanceAppService.GetCurrencyLookupAsync()).Items;
            await GetCustomersAsync(customerPagedAndSortedResultRequestDto);

            await SetPermissionsAsync();
        }
        private void OpenCreateCustomerModal()
        {
            CreateCustomerValidationsRef.ClearAll();

            NewCustomer = new CreateUpdateCustomerDto();
            CreateCustomerModal.Show();
            CreateSearchCustomerModal.Hide();

        }
        private async Task PassCustomer(CustomerDto customerDto, CreateRemittanceDto newRemittance, UpdateRemittanceDto editingRemittance)
        {
            //await CreateValidationsRef.ClearAll();
            await EditValidationsRef.ClearAll();
            var checkAge = DateTime.Now.Year - customerDto.BirthDate.Year;
            //Check If Pass CreateRemittanceModal Or UpdateRemittanceModal Or ReleaseRemittanceModal
                editingRemittance.ReceiverBy = customerDto.Id;
                editingRemittance.ReceiverFullName = customerDto.FirstName + " " + customerDto.FatherName
                + " " + customerDto.LastName;
                editingRemittance.ReceiverName = customerDto.FirstName + " " + customerDto.FatherName
                + " " + customerDto.LastName;
                EditingRemittance = editingRemittance;
                await CreateSearchCustomerModal.Hide();
                await ReleaseRemittanceModal.Show();
        }

        private async Task OnDataGridCustomersReadAsync(DataGridReadDataEventArgs<CustomerDto> e_Customer)
        {

            customerPagedAndSortedResultRequestDto = new CustomerPagedAndSortedResultRequestDto();
            CurrentSortingCustomer = e_Customer.Columns
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
            CurrentPageCustomer = e_Customer.Page - 1;

            var firstName = e_Customer.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "FirstName");
            if (firstName != null)
                customerPagedAndSortedResultRequestDto.FirstName = firstName.SearchValue.ToString();
            var lastName = e_Customer.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "LastName");
            if (lastName != null)
                customerPagedAndSortedResultRequestDto.LastName = lastName.SearchValue.ToString();

            var fatherName = e_Customer.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "FatherName");
            if (fatherName != null)
                customerPagedAndSortedResultRequestDto.FatherName = fatherName.SearchValue.ToString();
            var motherName = e_Customer.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "MotherName");
            if (motherName != null)
                customerPagedAndSortedResultRequestDto.MotherName = motherName.SearchValue.ToString();



            await GetCustomersAsync(customerPagedAndSortedResultRequestDto);
            await InvokeAsync(StateHasChanged);
        }
        private async Task SetPermissionsAsync()
        {
            CanCreateRemittance = await AuthorizationService
                .IsGrantedAsync(RMSPermissions.Remittances.Create);

            CanEditRemittance = await AuthorizationService
                .IsGrantedAsync(RMSPermissions.Remittances.Edit);

            CanDeleteRemittance = await AuthorizationService
                .IsGrantedAsync(RMSPermissions.Remittances.Delete);


            CanApprovedRemittance = await AuthorizationService
                .IsGrantedAsync(RMSPermissions.Status.Approved);

            CanReleaseRemittance = await AuthorizationService
                .IsGrantedAsync(RMSPermissions.Status.Released);
            CanReadyRemittance = await AuthorizationService
             .IsGrantedAsync(RMSPermissions.Status.Ready);

        }


        private async Task GetRemittancesAsync()
        {
            var result = await RemittanceAppService.GetListRemittancesForReleaser(
                new GetRemittanceListDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );
            RemittanceList = result.Items;
            TotalCount = (int)result.TotalCount;
        }
        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<RemittanceDto> e)
        {
            CurrentSorting = e.Columns
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;
            await GetRemittancesAsync();
            await InvokeAsync(StateHasChanged);
        }


        private void CloseCreateSearchCustomerModal()
        {
            CreateSearchCustomerModal.Hide();
        }

        private void CloseCreateCustomerModal()
        {
            CreateCustomerModal.Hide();
        }

        private async Task CreateCustomerAsync()
        {
            if (await CreateCustomerValidationsRef.ValidateAll())
            {

                await CustomerAppService.CreateAsync(NewCustomer);

                await CreateCustomerModal.Hide();
                await GetCustomersAsync(customerPagedAndSortedResultRequestDto);

                await CreateSearchCustomerModal.Show();
            }
        }
        private async Task GetCustomersAsync(CustomerPagedAndSortedResultRequestDto customerPagedAndSortedResultRequestDto)
        {

            PagedResultDto<CustomerDto> result = new PagedResultDto<CustomerDto>();
            if (customerPagedAndSortedResultRequestDto == null)
            {

                result = await CustomerAppService.GetListAsync(
               new CustomerPagedAndSortedResultRequestDto
               {

                   MaxResultCount = PageSize,
                   SkipCount = CurrentPageCustomer * PageSize,
                   Sorting = CurrentSortingCustomer
               }
           );
            }
            else
            {
                result = await CustomerAppService.GetListAsync(
             new CustomerPagedAndSortedResultRequestDto
             {
                 FirstName = customerPagedAndSortedResultRequestDto.FirstName,
                 LastName = customerPagedAndSortedResultRequestDto.LastName,
                 FatherName = customerPagedAndSortedResultRequestDto.FatherName,
                 MotherName = customerPagedAndSortedResultRequestDto.MotherName,
                 MaxResultCount = PageSize,
                 SkipCount = CurrentPageCustomer * PageSize,
                 Sorting = CurrentSortingCustomer
             }
             );

            }


            CustomerList = result.Items;
            TotalCount = (int)result.TotalCount;
        }
        private void OpenCreateSearchCustomerModalForReleaser()
        {
            //CreateCustomerValidationsRef.ClearAll();
            NewCustomer = new CreateUpdateCustomerDto();
            if (CanReleaseRemittance)
            {
                CreateSearchCustomerModal.Show();
                ReleaseRemittanceModal.Hide();
            }
        }
        private void CloseReleaseRemittanceModal()
        {
            ReleaseRemittanceModal.Hide();
        }
        private void OpenReleaseRemittanceModal(RemittanceDto Remittance)
        {
            EditValidationsRef.ClearAll();
            EditingRemittanceId = Remittance.Id;
            EditingRemittance = ObjectMapper.Map<RemittanceDto, UpdateRemittanceDto>(Remittance);
            EditingRemittance.ReceiverName = null;
            ReleaseRemittanceModal.Show();
        }
        private async Task UpdateRemittanceToReleaseAsync(Guid editingRemittanceId, UpdateRemittanceDto updateRemittance)
        {
            RemittanceDto remittance = ObjectMapper.Map<UpdateRemittanceDto, RemittanceDto>(updateRemittance);
            if (editingRemittanceId.Equals(null))
                throw new ArgumentNullException(nameof(editingRemittanceId));
            remittance.Id = editingRemittanceId;
            await RemittanceAppService.SetRelease(remittance);
            await GetRemittancesAsync();
            await ReleaseRemittanceModal.Hide();


        }
    }
}
