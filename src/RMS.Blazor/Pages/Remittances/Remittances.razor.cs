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

namespace RMS.Blazor.Pages.Remittances
{
    public partial class Remittances 
    {


        [Inject]
        private IReadOnlyList<RemittanceDto> RemittanceList { get; set; }
        [Inject]
        private IReadOnlyList<CustomerDto> CustomerList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

        [Parameter]
        public EventCallback<string> OnSearchChanged { get; set; }

        public string SearchTerm { get; set; }




        IReadOnlyList<CurrencyLookupDto> currencyList = Array.Empty<CurrencyLookupDto>();

        IReadOnlyList<CustomerLookupDto> customerList = Array.Empty<CustomerLookupDto>();


        //IReadOnlyList<UserLookupDto> userList = Array.Empty<UserLookupDto>();

        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; }
        private string CurrentSortingCustomer { get; set; }
        private int TotalCount { get; set; }


     
        private CreateRemittanceDto NewRemittance { get; set; }

        private Guid EditingRemittanceId { get; set; }
        private UpdateRemittanceDto EditingRemittance { get; set; }


        private CreateUpdateCustomerDto NewCustomer { get; set; }
        private Modal CreateSearchCustomerModal { get; set; }



        private Modal CreateCustomerModal { get; set; }
        private Modal CreateRemittanceModal { get; set; }
        private Modal EditRemittanceModal { get; set; }





        private Validations CreateCustomerValidationsRef;

        private Validations CreateValidationsRef;

        private Validations EditValidationsRef;

        public Remittances()
        {
            NewCustomer = new CreateUpdateCustomerDto();
            NewRemittance = new CreateRemittanceDto();
            EditingRemittance = new UpdateRemittanceDto();
        }




        protected override async Task OnInitializedAsync()
        {
            await GetRemittancesAsync();
          await GetCustomersAsync();
            currencyList = (await RemittanceAppService.GetCurrencyLookupAsync()).Items;
            customerList = (await RemittanceAppService.GetCustomerLookupAsync()).Items;
            //userList = (await RemittanceAppService.GetUserLookupAsync()).Items;
        }

    
        private async Task GetRemittancesAsync()
        {
            var result = await RemittanceAppService.GetListAsync(
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

           await  GetRemittancesAsync();
           
            await InvokeAsync(StateHasChanged);
        }









        private async Task OnDataGridCustomersReadAsync(DataGridReadDataEventArgs<CustomerDto> e_Customer)
        {
            CurrentSortingCustomer = e_Customer.Columns
          .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
          .JoinAsString(",");
            CurrentPage = e_Customer.Page - 1;

            await GetCustomersAsync();

            await InvokeAsync(StateHasChanged);
        }




        private async Task GetCustomersAsync()
        {
        
            var result = await CustomerAppService.GetListAsync(
                 new CustomerPagedAndSortedResultRequestDto
                 {
                     MaxResultCount = PageSize,
                     SkipCount = CurrentPage * PageSize,
                     Sorting = CurrentSortingCustomer
                 }
             );

            CustomerList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task PassCustomer(CustomerDto customerDto, double amount,
            string receiverFullName,Guid currencyId,RemittanceType type)
            
        {
            NewRemittance = new CreateRemittanceDto() { SenderBy=customerDto.Id,
            SenderName=customerDto.FirstName+" "+ customerDto.FatherName
            +" "+ customerDto.LastName,Amount=amount,
                CurrencyId=currencyId,ReceiverFullName=receiverFullName
            };
           await CreateSearchCustomerModal.Hide();
            await CreateRemittanceModal.Show();
        }



        private void OpenCreateCustomerModal()
        {
            CreateValidationsRef.ClearAll();

            NewCustomer = new CreateUpdateCustomerDto();
            CreateCustomerModal.Show();
            CreateSearchCustomerModal.Hide();

        }

        private void OpenCreateSearchCustomerModal()
        {
            CreateCustomerValidationsRef.ClearAll();

            NewCustomer = new CreateUpdateCustomerDto();
            CreateSearchCustomerModal.Show();
             CreateRemittanceModal.Hide();

        }

        private void CloseCreateSearchCustomerModal()
        {
            CreateSearchCustomerModal.Hide();
            CreateRemittanceModal.Show();
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
                await GetCustomersAsync();

                await CreateSearchCustomerModal.Show();
            }
        }









        private void OpenCreateRemittanceModal()
        {
            CreateValidationsRef.ClearAll();

            NewRemittance = new CreateRemittanceDto();
            CreateRemittanceModal.Show();
        }


        private void CloseCreateRemittanceModal()
        {
            CreateRemittanceModal.Hide();
        }

        private void OpenEditRemittanceModal(RemittanceDto Remittance)
        {
            EditValidationsRef.ClearAll();

            EditingRemittanceId = Remittance.Id;
            EditingRemittance = ObjectMapper.Map<RemittanceDto, UpdateRemittanceDto>(Remittance);
            EditRemittanceModal.Show();
        }

        private async Task DeleteRemittanceAsync(RemittanceDto Remittance)
        {
            var confirmMessage = L["RemittanceDeletionConfirmationMessage", Remittance.Amount];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await RemittanceAppService.DeleteAsync(Remittance.Id);
            await GetRemittancesAsync();
        }

        private void CloseEditRemittanceModal()
        {
            EditRemittanceModal.Hide();
        }

        private async Task CreateRemittanceAsync()
        {
            if (await CreateValidationsRef.ValidateAll())
            {

                await RemittanceAppService.CreateAsync(NewRemittance);
                
                await GetRemittancesAsync();
               await CreateRemittanceModal.Hide();
            }
        }
        private async Task UpdateRemittanceToReadyAsync(RemittanceDto Remittance)
        {

            EditingRemittance = ObjectMapper.Map<RemittanceDto, UpdateRemittanceDto>(Remittance);
            EditingRemittanceId = Remittance.Id;
            await RemittanceAppService.SetReady(EditingRemittanceId, EditingRemittance);
            await GetRemittancesAsync();

        }
        private async Task UpdateRemittanceAsync()
        {
            if (await EditValidationsRef.ValidateAll())
            {
                await RemittanceAppService.UpdateAsync(EditingRemittanceId, EditingRemittance);
                await GetRemittancesAsync();
                await EditRemittanceModal.Hide();
            }
        }

      
    }
}