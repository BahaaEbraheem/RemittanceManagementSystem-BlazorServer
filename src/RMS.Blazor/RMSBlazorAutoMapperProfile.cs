using AutoMapper;
using RMS.Currencies.Dtos;
using RMS.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMS.Customers.Dtos;
using RMS.Customers;
using RMS.Remittances.Dtos;
using RMS.Remittances;
using RMS.Status.Dtos;
using static RMS.Enums.Enums;

namespace RMS.Blazor
{
    public class RMSBlazorAutoMapperProfile:Profile
    {
        public RMSBlazorAutoMapperProfile()
        {
            CreateMap<CurrencyDto, CreateCurrencyDto>();
            CreateMap<CurrencyDto, Currency>();
            CreateMap<Currency, CurrencyDto>();
            CreateMap<CurrencyDto, UpdateCurrencyDto>();
            CreateMap<CurrencyDto, CreateUpdateCurrencyDto>();


            CreateMap<CustomerDto, CreateCustomerDto>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, UpdateCustomerDto>();
            CreateMap<CustomerDto, CreateUpdateCustomerDto>();
            CreateMap<RemittanceStatusDto, CreateUpdateRemittanceStatusDto>();


            CreateMap<RemittanceDto, CreateRemittanceDto>();
            CreateMap<CreateRemittanceDto, RemittanceDto>();
            CreateMap<RemittanceDto, UpdateRemittanceDto>();




            CreateMap<RemittanceStatusDto, RemittanceDto>();
            CreateMap<Remittance_Status, RemittanceDto>();
            //CreateMap<Remittance, RemittanceDto>();
        }
    }
}
