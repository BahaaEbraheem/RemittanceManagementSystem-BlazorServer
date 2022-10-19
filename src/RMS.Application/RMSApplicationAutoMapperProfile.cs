using AutoMapper;
using RMS.Currencies;
using RMS.Currencies.Dtos;
using RMS.Remittances;
using RMS.Remittances.Dtos;
using Microsoft.AspNetCore.Components;
using RMS.Customers;
using RMS.Customers.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using Volo.Abp.AutoMapper;
using RMS.Status.Dtos;
using RMS.Status;

namespace RMS
{

    public class RMSApplicationAutoMapperProfile : Profile
    {
        public RMSApplicationAutoMapperProfile()
        {
            CreateMap<Currency, CurrencyDto>();
            CreateMap<CreateCurrencyDto, Currency>();
            CreateMap<UpdateCurrencyDto, Currency>();
            CreateMap<Remittance, RemittanceDto>();

            CreateMap<CreateUpdateCurrencyDto, Currency>();



            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();
            CreateMap<CreateUpdateCustomerDto, Customer>();


            //CreateMap<Remittance, RemittanceDto>();
            //CreateMap<CreateRemittanceDto, Remittance>();
            //CreateMap<UpdateRemittanceDto, Remittance>();

                //.ReverseMap()
                // .ForMember(a => a.Authors, a => a.Ignore());


            CreateMap<Currency, CurrencyLookupDto>();
            CreateMap<IdentityUser, UserLookupDto>().ForMember(des => des.Name, src => src.MapFrom<string>(r => r.UserName));
            CreateMap<UserLookupDto, IdentityUser>().ForMember(des => des.UserName, src => src.MapFrom<string>(r => r.Name))
                .Ignore(x => x.AccessFailedCount)
    .Ignore(x => x.LockoutEnabled)
    .Ignore(x => x.NormalizedUserName).Ignore(x => x.Email).Ignore(x => x.NormalizedEmail).Ignore(x => x.EmailConfirmed)
    .Ignore(x => x.SecurityStamp).Ignore(x => x.IsExternal).Ignore(x => x.PhoneNumberConfirmed).Ignore(x => x.IsActive)
    .Ignore(x => x.TwoFactorEnabled).Ignore(x => x.LockoutEnabled).Ignore(x => x.AccessFailedCount).Ignore(x => x.CreationTime)
    .Ignore(x => x.IsDeleted);
            
            CreateMap<RemittanceStatus, RemittanceStatusDto>();
            CreateMap<CreateUpdateRemittanceStatusDto, RemittanceStatus>();
            CreateMap<CreateUpdateRemittanceStatusDto, RemittanceStatusDto>();

            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */


            CreateMap<Customer, CustomerLookupDto>();

            CreateMap<CurrencyPagedAndSortedResultRequestDto, Currency>();
            CreateMap<CustomerPagedAndSortedResultRequestDto, Customer>();

            CreateMap<GetRemittanceListPagedAndSortedResultRequestDto, Remittance>();


        }
    }
}