using IMS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMS.WebMvc.Models
{
    public class DashboardModel
    {
        public List<ExpiringPoliciesModel> ExpiringPolicies { get; set; }
        public List<OutstandingInvoicesModel> OutstandingInvoices { get; set; }
        public List<SoaModel> Soas { get; set; }
    }

    public class ExpiringPoliciesModel
    {
        public int Id { get; set; }
        public int InsuranceProviderId { get; set; }
        public string CompanyName { get; set; }
        public int PolicyTypeId { get; set; }
        public string PolicyTypeName { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int ClientId { get; set; }
        public bool IsOrganization { get; set; }
        public string OrganizationName { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
    }

    public class OutstandingInvoicesModel
    {
        public DateTime IssueDate { get; set; }
        public string InvoiceNumber { get; set; }
        public int ClientId { get; set; }
        public bool IsOrganization { get; set; }
        public string OrganizationName { get; set; }
        public string ClientName { get; set; }
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumInsured { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalAmountDue { get; set; }

        public string StatusName { get; set; }
        public string Remarks { get; set; }

        public DateTime DueDate { get; set; }
    }
}