

using System.Linq;
using System.Threading.Tasks;
using RMS.Customers;
using Blazorise.DataGrid;
using Volo.Abp.Application.Dtos;
using System.Xml.Linq;
using RMS.Customers.Dtos;
using RMS.Remittances.Dtos;
using RMS.Remittances;
using Microsoft.CodeAnalysis;
using System;
using Volo.Abp.BlazoriseUI;

namespace RMS.Blazor.Pages.Customers
{
    public partial class Customers 
    {
        protected override Task UpdateGetListInputAsync() 
        {  
            if 
                (GetListInput is ISortedResultRequest sortedResultRequestInput) 
            {  
                sortedResultRequestInput.Sorting = CurrentSorting;
            } 
            if (GetListInput is IPagedResultRequest pagedResultRequestInput)
            { 
                pagedResultRequestInput.SkipCount = (CurrentPage - 1) * PageSize;
            }

            if (GetListInput is ILimitedResultRequest limitedResultRequestInput) 
            { 
                limitedResultRequestInput.MaxResultCount = PageSize;
            }
            return Task.CompletedTask;
        }
        protected override Task OnDataGridReadAsync(DataGridReadDataEventArgs<CustomerDto> e) 
        {


            var firstName = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "FirstName");
            if (firstName != null) this. GetListInput.FirstName = firstName.SearchValue.ToString();
            var lastName = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "LastName");
            if (lastName != null) this.GetListInput.LastName = lastName.SearchValue.ToString();
            var fatherName = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "FatherName");
            if (fatherName != null) this.GetListInput.FatherName = fatherName.SearchValue.ToString();
            var motherName = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "MotherName");
            if (motherName != null) this.GetListInput.MotherName = motherName.SearchValue.ToString();
            return base.OnDataGridReadAsync(e);
        }



    }
}
