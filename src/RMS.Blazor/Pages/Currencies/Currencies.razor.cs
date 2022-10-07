﻿

using System.Linq;
using System.Threading.Tasks;
using RMS.Currencies;
using Blazorise.DataGrid;
using Volo.Abp.Application.Dtos;
using System.Xml.Linq;
using RMS.Currencies.Dtos;
using RMS.Remittances.Dtos;
using RMS.Remittances;
using Microsoft.CodeAnalysis;

namespace RMS.Blazor.Pages.Currencies
{
    public partial class Currencies
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
        protected override Task OnDataGridReadAsync(DataGridReadDataEventArgs<CurrencyDto> e) 
        {

            //var id = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "Id");
            //if (id != null) this.GetListInput.Id = id.SearchValue.ToString();
            var name = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "Name");
            if (name != null) this. GetListInput.Name = name.SearchValue.ToString();
            var Symbol = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "Symbol");
            if (Symbol != null) this.GetListInput.Symbol = Symbol.SearchValue.ToString();
            var Code = e.Columns.FirstOrDefault(c => c.SearchValue != null && c.Field == "Code");
            if (Code != null) this.GetListInput.Code = Code.SearchValue.ToString();
            return base.OnDataGridReadAsync(e);
        }



    }
}