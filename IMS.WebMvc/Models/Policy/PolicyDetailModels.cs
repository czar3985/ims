using IMS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMS.WebMvc.Models
{
    public class PolicyDetailModel : PolicyModel
    {
        // Create
        public List<InsuranceProvider> Companies { get; set; }
        public List<PolicyType> PolicyTypes { get; set; }
        public List<Agent> Agents { get; set; }
        public List<ClientSimple> Clients { get; set; }
        public List<DefaultRebate> DefaultRebates { get; set; }
        public List<PolicyStatus> Statuses { get; set; }

        public PolicyModel Policy { get; set; }
        public PolicyAttachmentModel NewAttachment { get; set; }

        // Details

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Policy Type")]
        public string PolicyTypeName { get; set; }

        public bool IsOrganization { get; set; }

        public string OrganizationName { get; set; }

        [Display(Name = "Insured")]
        public string ClientName { get; set; }

        [Display(Name = "Agent")]
        public string AgentName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        public List<PolicyInvoiceModel> PolicyInvoices { get; set; }
        public List<PolicyClaimModel> PolicyClaims { get; set; }
        public List<PolicyAttachmentModel> PolicyAttachments { get; set; }
    }

    public class PolicyModel
    {
        public int Id { get; set; }

        [Display(Name = "Sum Insured")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Sum Insured should be > 0. Add Amount Insured in Risks.")]
        public decimal SumInsured { get; set; }

        public string Remarks { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "InceptionDate")]
        public DateTime InceptionDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Issued")]
        public DateTime DateIssued { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Premium should be > 0.")]
        public decimal Premium { get; set; }

        [DisplayFormat(DataFormatString = "{0:F6}", ApplyFormatInEditMode = true)]
        [Range(0.00, 1.00)]
        public double Rate { get; set; }

        [Required]
        [Display(Name = "Policy No.")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Renewal No.")]
        public string RenewalPolicyNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:F6}", ApplyFormatInEditMode = true)]
        [Range(0.00, 1.00)]
        public double Rebate { get; set; }

        public decimal Commission { get; set; }

        // Foreign Keys
        [Required]
        [Display(Name = "Insured")]
        public int ClientId { get; set; }

        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "Agent")]
        public int AgentId { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int InsuranceProviderId { get; set; }

        [Required]
        [Display(Name = "Policy Type")]
        public int PolicyTypeId { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        public string ReturnUrl { get; set; }
        public EndorsementModel Endt { get; set; }
        public RiskModel Risk { get; set; }
        public List<RiskModel> Risks { get; set; }
        public List<EndorsementModel> Endorsements { get; set; }

    }

    public class RiskModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal AmountInsured { get; set; }

        [DisplayFormat(DataFormatString = "{0:F6}", ApplyFormatInEditMode = true)]
        [Range(0.00, 1.00)]
        public double Rate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Premium { get; set; }

        public string Remarks { get; set; }
        public int PolicyId { get; set; }
        public string Address { get; set; }
    }

    public class ExistingClientsViewModel
    {
        public int Id { get; set; }
        public bool IsOrganization { get; set; }
        public string OrganizationName { get; set; }
        public string AccountNumber { get; set; }
        public string ClientName { get; set; }
    }

    public class PolicyInvoiceModel
    {
        public DateTime IssueDate { get; set; }
        public string InvoiceNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalAmountDue { get; set; }

        public string StatusName { get; set; }
        public string Remarks { get; set; }
    }

    public class PolicyClaimModel
    {
        public DateTime ClaimDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal ClaimAmount { get; set; }

        public string ClaimNumber { get; set; }
        public string Remarks { get; set; }
        public string StatusName { get; set; }
    }

    public class PolicyAttachmentModel
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string Remarks { get; set; }

        // Foreign Keys
        public int PolicyId { get; set; }
    }

    public class EndorsementModel
    {
        public int Id { get; set; }
        public bool IsRet { get; set; }
        public string Remarks { get; set; }
        public string Mortgagee { get; set; }
        public int PolicyId { get; set; }

        [Required]
        [Display(Name ="Issue Date")]
        public DateTime IssueDate { get; set; }

        [Required]
        [Display(Name = "Effective Date")]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [Display(Name = "Endorsement No.")]
        public string EndorsementNumber { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Positive value required. Indicate if Add or Return on the left.")]
        [Display(Name = "Amount of Add. (Ret) Premium")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal EndorsementAmount { get; set; }

        [Display(Name = "Total Amount")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalEndorsementAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Mt { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Pt { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Dst { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Fst { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Vat { get; set; }
    }

    public class DefaultRebateModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:F6}", ApplyFormatInEditMode = true)]
        [Range(0.00, 1.00)]
        public double Rate { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Display(Name = "Policy Type")]
        public string PolicyTypeName { get; set; }

        // Foreign Keys
        public int InsuranceProviderId { get; set; }
        public int PolicyTypeId { get; set; }
    }

    public class DefaultRebateViewModel
    {
        public List<DefaultRebateModel> DefaultRebatesList { get; set; }
    }
}