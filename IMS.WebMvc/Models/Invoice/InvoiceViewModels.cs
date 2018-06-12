using IMS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebMvc.Models
{
    public enum InvoiceActionEnum
    {
        Approve,
        Reject,
        Cancel,
        GenerateAndEmail,
        Paid,
        UpdatePostPaidProperties
    }

    public class InvoiceGroupedViewModel
    {
        public string CompanyName { get; set; }
        public List<InvoiceItemViewModel> InvoiceViewModelList { get; set; }
    }

    public class InvoiceItemViewModel : InvoiceModel
    {
        public InvoiceItemViewModel()
        {
            Policies = new List<ClientPoliciesListModel>();
        }

        public bool IsOrganization { get; set; }
        public string OrganizationName { get; set; }
        public string ClientName { get; set; }
        public int InsuranceProviderId { get; set; }

        [Display(Name = "Policy No.")]
        public string PolicyNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumInsured { get; set; }

        public string StatusName { get; set; }

        //For Edit
        [Display(Name = "Account No.")]
        public string AccountNumber { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Premium { get; set; }

        public List<ClientPoliciesListModel> Policies { get; set; }

        //For Create
        public List<ParticularType> ParticularTypesList { get; set; }
        public List<ClientSimple> Clients { get; set; }

        // Flags
        public bool IsFromPolicy { get; set; }
        public bool IsReadOnlyState { get; set; }

        // Email
        public string ClientEmail { get; set; }
    }

    public class InvoiceActionViewModel
    {
        public bool IsApprove { get; set; }
        public bool IsSelected { get; set; }
        public int InvoiceId { get; set; }
    }

    public class InvoiceModel
    {
        public InvoiceModel()
        {
            Particulars = new List<ParticularModel>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name ="Issue Date")]
        public DateTime IssueDate { get; set; }

        [Display(Name = "Paid Date")]
        public DateTime PaidDate { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Display(Name ="Invoice No.")]
        [Required]
        public string InvoiceNumber { get; set; }

        [Range(0.01,Double.MaxValue)]
        [Display(Name = "Total Amount Due")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalAmountDue { get; set; }

        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Insured")]
        [Range(1, int.MaxValue, ErrorMessage = "Insured field is required")]
        public int ClientId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PolicyId { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Display(Name = "Endorsement No.")]
        public string EndorsementNumber { get; set; }

        [Range(0.00, 1.00)]
        [DisplayFormat(DataFormatString = "{0:F6}", ApplyFormatInEditMode = true)]
        public double Rebate { get; set; }

        public List<ParticularModel> Particulars { get; set; }

        public InvoiceActionEnum InvoiceAction { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "OR Number")]
        public string ORNumber { get; set; }

        [Display(Name = "Has EWT")]
        public bool HasEWT { get; set; }

        [Display(Name = "Amount Paid")]
        public decimal AmountPaid { get; set; }
    }

    public class ExistingPolicyModel
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
    }

    public class ParticularModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal ParticularAmount { get; set; }

        public string Remarks { get; set; }
        public int InvoiceId { get; set; }
        public int ParticularTypeId { get; set; }
        public string ParticularTypeName { get; set; }
    }
}