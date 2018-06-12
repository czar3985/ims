using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IMS.Entities;
using IMS.WebMvc.Models;
using AutoMapper;

namespace IMS.WebMvc
{
    public static class MapConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(Configure);
        }

        private static void Configure(IConfiguration config)
        {
            ConfigureAccountsModels(config);
            ConfigureDashboardModels(config);
            ConfigureClientViewModels(config);
            ConfigureClientDetailModels(config);
            ConfigurePolicyViewModels(config);
            ConfigurePolicyDetailModel(config);
            ConfigureInvoiceViewModels(config);
            ConfigureClaimViewModels(config);
            ConfigureFormViewModels(config);
            ConfigureSoaModels(config);
            ConfigureReportModels(config);
            ConfigureDefaultRebateModels(config);
            ConfigureDirectoryModels(config);
        }

       

        private static void ConfigureAccountsModels(IConfiguration config)
        {
            Mapper.CreateMap<ApplicationUser, ApplicationUserModel>().ReverseMap();
        }

        private static void ConfigureDashboardModels(IConfiguration config)
        {
            Mapper.CreateMap<Policy, ExpiringPoliciesModel>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.InsuranceProvider.Name))
                .ForMember(dest => dest.PolicyTypeName, opt => opt.MapFrom(src => src.PolicyType.Name))
                .ForMember(dest => dest.IsOrganization, opt => opt.MapFrom(src => src.Client.IsOrganization))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Client.OrganizationName))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.LastName + ", " + src.Client.FirstName))
                .ForMember(dest => dest.ClientEmail, opt => opt.MapFrom(src => src.Client.Email));
            Mapper.CreateMap<Invoice, OutstandingInvoicesModel>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Policy.ClientId))
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.Policy.PolicyNumber))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.IsOrganization, opt => opt.MapFrom(src => src.Policy.Client.IsOrganization))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Policy.Client.OrganizationName))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Policy.Client.LastName + ", " + src.Policy.Client.FirstName))
                .ForMember(dest => dest.SumInsured, opt => opt.MapFrom(src => src.Policy.SumInsured));
        }

        private static void ConfigureClientViewModels(IConfiguration config)
        {
            Mapper.CreateMap<Client, ClientsListModel>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.LastName + ", " + src.FirstName))
                .ForMember(dest => dest.TotalBusiness, opt => opt.MapFrom(src => src.Policies.Sum(c => c.SumInsured)))
                .ForMember(dest => dest.TotalClaim,
                    opt => opt.MapFrom(src => src.Policies.SelectMany(p => p.Claims).Sum(c => c.ClaimAmount)))
                .ForMember(dest => dest.Balance,
                    opt => opt.MapFrom(src => src.Policies.SelectMany(p => p.Invoices)
                    .Where(i => i.Status.Name == "Unpaid").Sum(i => i.TotalAmountDue)));
            Mapper.CreateMap<Client, ClientSimple>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.LastName + ", " + src.FirstName));
            Mapper.CreateMap<Client, ClientModel>().ReverseMap();
        }

        private static void ConfigureClientDetailModels(IConfiguration config)
        {
            Mapper.CreateMap<Client, ClientDetailModel>()
                .ForMember(dest => dest.TotalBusiness, opt => opt.MapFrom(src => src.Policies.Sum(c => c.SumInsured)))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.LastName + ", " + src.FirstName))
                .ForMember(dest => dest.TotalClaim,
                    opt => opt.MapFrom(src => src.Policies.SelectMany(p => p.Claims).Sum(c => c.ClaimAmount)));
            Mapper.CreateMap<Policy, ClientPoliciesListModel>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.InsuranceProvider.Name))
                .ForMember(dest => dest.PolicyTypeName, opt => opt.MapFrom(src => src.PolicyType.Name))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.LatestEndorsementNumber, opt => opt.MapFrom(src =>
                        src.Endorsements.OrderByDescending(e => e.EffectiveDate).FirstOrDefault().
                        EndorsementNumber));
            Mapper.CreateMap<Claim, ClientClaimsListModel>()
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.Policy.PolicyNumber))
                .ForMember(dest => dest.PolicyTypeName, opt => opt.MapFrom(src => src.Policy.PolicyType.Name))
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.ClaimAttachments.FirstOrDefault().FileUrl));
            Mapper.CreateMap<Offer, ClientOffersListModel>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name));
        }

        private static void ConfigurePolicyViewModels(IConfiguration config)
        {
            Mapper.CreateMap<Policy, PolicyItemViewModel>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.InsuranceProvider.Name))
                .ForMember(dest => dest.PolicyTypeName, opt => opt.MapFrom(src => src.PolicyType.Name))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.IsOrganization, opt => opt.MapFrom(src => src.Client.IsOrganization))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Client.OrganizationName))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.LastName + ", " + src.Client.FirstName));

            Mapper.CreateMap<Risk, RiskModel>().ReverseMap();
        }

        private static void ConfigurePolicyDetailModel(IConfiguration config)
        {
            Mapper.CreateMap<Policy, PolicyModel>().ReverseMap();
            Mapper.CreateMap<Client, ExistingClientsViewModel>()
                .ForMember(dest => dest.ClientName,
                    opt => opt.MapFrom(src => src.LastName + ", " + src.FirstName));
            Mapper.CreateMap<Policy, PolicyDetailModel>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.InsuranceProvider.Name))
                .ForMember(dest => dest.PolicyTypeName, opt => opt.MapFrom(src => src.PolicyType.Name))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Agent.Name))
                .ForMember(dest => dest.IsOrganization, opt => opt.MapFrom(src => src.Client.IsOrganization))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Client.OrganizationName))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.LastName + ", " + src.Client.FirstName));
            Mapper.CreateMap<Invoice, PolicyInvoiceModel>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name));
            Mapper.CreateMap<Claim, PolicyClaimModel>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name));
            Mapper.CreateMap<PolicyAttachment, PolicyAttachmentModel>().ReverseMap();
            Mapper.CreateMap<Endorsement, EndorsementModel>().ReverseMap();
        }

        private static void ConfigureInvoiceViewModels(IConfiguration config)
        {
            Mapper.CreateMap<Invoice, InvoiceItemViewModel>()
                .ForMember(dest => dest.InsuranceProviderId, opt => opt.MapFrom(src => src.Policy.InsuranceProviderId))
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.Policy.PolicyNumber))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.SumInsured, opt => opt.MapFrom(src => src.Policy.SumInsured))
                .ForMember(dest => dest.IsOrganization, opt => opt.MapFrom(src => src.Policy.Client.IsOrganization))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Policy.Client.OrganizationName))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Policy.Client.AccountNumber))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Policy.InsuranceProvider.Name))
                .ForMember(dest => dest.Premium, opt => opt.MapFrom(src => src.Policy.Premium))
                //.ForMember(dest => dest.Rebate, opt => opt.MapFrom(src => src.Policy.Rebate))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Policy.Client.Id))
                .ForMember(dest => dest.ClientEmail, opt => opt.MapFrom(src => src.Policy.Client.Email))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Policy.Client.LastName + ", " + src.Policy.Client.FirstName))
                .ForMember(dest => dest.EndorsementNumber, opt => opt.MapFrom(src => src.Policy.Endorsements
                        .OrderByDescending(e => e.EffectiveDate).FirstOrDefault().EndorsementNumber));
            Mapper.CreateMap<Invoice, InvoiceModel>().ReverseMap();
            Mapper.CreateMap<Policy, ExistingPolicyModel>();
            Mapper.CreateMap<Particular, ParticularModel>().ReverseMap();
            Mapper.CreateMap<Particular, ParticularModel>()
                .ForMember(dest => dest.ParticularTypeName, opt => opt.MapFrom(src => src.ParticularType.Name));
        }

        private static void ConfigureClaimViewModels(IConfiguration config)
        {
            Mapper.CreateMap<Claim, ClaimItemViewModel>()
                .ForMember(dest => dest.InsuranceProviderId, opt => opt.MapFrom(src => src.Policy.InsuranceProviderId))
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.Policy.PolicyNumber))
                .ForMember(dest => dest.PolicyTypeName, opt => opt.MapFrom(src => src.Policy.PolicyType.Name))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Policy.InsuranceProvider.Name))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.Policy.ExpiryDate))
                .ForMember(dest => dest.SumInsured, opt => opt.MapFrom(src => src.Policy.SumInsured))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Policy.Client.Id))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Policy.Client.LastName + ", " + src.Policy.Client.FirstName))
                .ForMember(dest => dest.IsOrganization, opt => opt.MapFrom(src => src.Policy.Client.IsOrganization))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Policy.Client.OrganizationName))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name));
            Mapper.CreateMap<Claim, ClaimModel>().ReverseMap();
        }

        private static void ConfigureFormViewModels(IConfiguration config)
        {
            //Mapper.CreateMap<Form, FormsListModel>()
            //    .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.InsuranceProvider.Name))
            //    .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.DocumentType.Name));
            Mapper.CreateMap<Form, FormModel>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.InsuranceProvider.Name))
                .ForMember(dest => dest.DocumentTypeName, opt => opt.MapFrom(src => src.DocumentType.Name))
                .ReverseMap();
        }

        private static void ConfigureSoaModels(IConfiguration config)
        {
            Mapper.CreateMap<Soa, SoaModel>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.LastName + ", " + src.Client.FirstName))
                .ForMember(dest => dest.IsOrganization, opt => opt.MapFrom(src => src.Client.IsOrganization))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Client.OrganizationName))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Client.Address))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.InsuranceProvider.Name))
                .ReverseMap();
            Mapper.CreateMap<PolicyType, SoaGroupedByType>()
                .ForMember(dest => dest.PolicyTypeName, opt => opt.MapFrom(src => src.Name));
            Mapper.CreateMap<Invoice, SoaTableEntry>()
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.Policy.PolicyNumber))
                .ForMember(dest => dest.PolicyTypeId, opt => opt.MapFrom(src => src.Policy.PolicyTypeId))
                .ForMember(dest => dest.Premium, opt => opt.MapFrom(src => src.Policy.Premium))
                .ForMember(dest => dest.Tax, opt => opt.MapFrom(src =>
                    src.Particulars.Where(p => p.ParticularType.Name != "Premium")
                        .Sum(p => p.ParticularAmount)))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.TotalAmountDue));
        }

        private static void ConfigureReportModels(IConfiguration config)
        {
            Mapper.CreateMap<Invoice, ReportViewModel>()
                .ForMember(dest => dest.PaidDate, opt => opt.MapFrom(src => src.PaidDate))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Policy.Client.LastName + ", " + src.Policy.Client.FirstName))
                .ForMember(dest => dest.IsOrganization, opt => opt.MapFrom(src => src.Policy.Client.IsOrganization))
                .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Policy.Client.OrganizationName))
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.Policy.PolicyNumber))
                .ForMember(dest => dest.AgentName, opt => opt.MapFrom(src => src.Policy.Agent.Name))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Policy.ClientId))
                .ForMember(dest => dest.PolicyId, opt => opt.MapFrom(src => src.Policy.Id))
                .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AgentId, opt => opt.MapFrom(src => src.Policy.AgentId))
                .ForMember(dest => dest.GrandTotal, opt => opt.MapFrom(src => src.TotalAmountDue));
        }

        private static void ConfigureDefaultRebateModels(IConfiguration config)
        {
            Mapper.CreateMap<DefaultRebate, DefaultRebateModel>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.InsuranceProvider.Name))
                .ForMember(dest => dest.PolicyTypeName, opt => opt.MapFrom(src => src.PolicyType.Name))
                .ReverseMap();
        }

        private static void ConfigureDirectoryModels(IConfiguration config)
        {
            Mapper.CreateMap<Directory, DirectoryViewModel>().ReverseMap();
        }
    }
}