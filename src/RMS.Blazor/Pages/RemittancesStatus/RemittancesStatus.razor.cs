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
using RMS.Customers.Dtos;
using RMS.Permissions;

namespace RMS.Blazor.Pages.RemittancesStatus
{
    public partial class RemittancesStatus
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
        protected override async Task OnInitializedAsync()
        {
            await GetRemittancesAsync();
            currencyList = (await RemittanceAppService.GetCurrencyLookupAsync()).Items;
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

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<RemittanceDto> e)
        {
            CurrentSorting = e.Columns
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;
            await GetRemittancesAsync();
            await InvokeAsync(StateHasChanged);
        }
        private async Task GetRemittancesAsync()
        {
            var result = await RemittanceAppService.GetListRemittancesStatusAsync(
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
    }
}
