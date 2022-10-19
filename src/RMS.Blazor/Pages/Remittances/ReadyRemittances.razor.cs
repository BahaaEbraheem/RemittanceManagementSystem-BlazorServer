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
    public partial class ReadyRemittances
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
        GetRemittanceListPagedAndSortedResultRequestDto getRemittanceListPagedAndSortedResultRequestDto
   = new GetRemittanceListPagedAndSortedResultRequestDto();
        private CreateRemittanceDto NewRemittance { get; set; }
        private Guid EditingRemittanceId { get; set; }
        private UpdateRemittanceDto EditingRemittance { get; set; }
        private CreateUpdateCustomerDto NewCustomer { get; set; }
        private Modal CreateSearchCustomerModal { get; set; }
        private Modal ReleaseRemittanceModal { get; set; }
        private Modal CreateCustomerModal { get; set; }
        private Modal CreateRemittanceModal { get; set; }
        private Modal EditRemittanceModal { get; set; }
        private Validations CreateCustomerValidationsRef;
        private Validations CreateValidationsRef;
        private Validations EditValidationsRef;
        private bool CanCreateRemittance { get; set; }
        private bool CanEditRemittance { get; set; }
        private bool CanDeleteRemittance { get; set; }
        private bool CanApprovedRemittance { get; set; }
        private bool CanReleaseRemittance { get; set; }
        private bool CanReadyRemittance { get; set; }
        public ReadyRemittances()
        {
            NewCustomer = new CreateUpdateCustomerDto();
            NewRemittance = new CreateRemittanceDto();
            EditingRemittance = new UpdateRemittanceDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetRemittancesAsync(getRemittanceListPagedAndSortedResultRequestDto);
            currencyList = (await RemittanceAppService.GetCurrencyLookupAsync()).Items;
            await SetPermissionsAsync();
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


        private async Task GetRemittancesAsync(GetRemittanceListPagedAndSortedResultRequestDto getRemittanceListPagedAndSortedResultRequestDto)
        {
            var result = await RemittanceAppService.GetListRemittancesForSupervisor(
                new GetRemittanceListPagedAndSortedResultRequestDto
                {
                    ReceiverFullName = getRemittanceListPagedAndSortedResultRequestDto.ReceiverFullName,
                    SenderName = getRemittanceListPagedAndSortedResultRequestDto.SenderName,
                    CurrencyName = getRemittanceListPagedAndSortedResultRequestDto.CurrencyName,
                    Amount = getRemittanceListPagedAndSortedResultRequestDto.Amount,
                    TotalAmount = getRemittanceListPagedAndSortedResultRequestDto.TotalAmount,
                    SerialNumber = getRemittanceListPagedAndSortedResultRequestDto.SerialNumber,
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
           
                GetRemittanceListPagedAndSortedResultRequestDto getRemittanceListPagedAndSortedResultRequestDto =
                    new GetRemittanceListPagedAndSortedResultRequestDto();
                CurrentSorting = e.Columns
                    .Where(c => c.SortDirection != SortDirection.Default)
                    .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                    .JoinAsString(",");
                CurrentPage = e.Page - 1;


                var receiverFullName = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "ReceiverFullName");
                if (receiverFullName != null)
                    getRemittanceListPagedAndSortedResultRequestDto.ReceiverFullName = receiverFullName.SearchValue.ToString();

                var currencyName = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "CurrencyName");
                if (currencyName != null)
                    getRemittanceListPagedAndSortedResultRequestDto.CurrencyName = currencyName.SearchValue.ToString();

                var senderName = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "SenderName");
                if (senderName != null)
                    getRemittanceListPagedAndSortedResultRequestDto.SenderName = senderName.SearchValue.ToString();


                var amount = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "Amount");
                if (amount != null)
                {
                    if (amount.SearchValue.ToString() != "")
                        getRemittanceListPagedAndSortedResultRequestDto.Amount = double.Parse((string)amount.SearchValue);
                }
                var totalAmount = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "TotalAmount");
                if (totalAmount != null)
                {
                    if (totalAmount.SearchValue.ToString() != "")
                        getRemittanceListPagedAndSortedResultRequestDto.TotalAmount = double.Parse((string)totalAmount.SearchValue);
                }

                var serialNumber = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "SerialNumber");
                if (serialNumber != null)
                    getRemittanceListPagedAndSortedResultRequestDto.SerialNumber = serialNumber.SearchValue.ToString();

                await GetRemittancesAsync(getRemittanceListPagedAndSortedResultRequestDto);
                await InvokeAsync(StateHasChanged);
            }
        private async Task UpdateRemittanceToReadyAsync(RemittanceDto Remittance)
        {
            await RemittanceAppService.SetApprove(Remittance);
            await GetRemittancesAsync(getRemittanceListPagedAndSortedResultRequestDto);

        }
    }
}
