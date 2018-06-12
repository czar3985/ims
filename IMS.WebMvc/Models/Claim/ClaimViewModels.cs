using IMS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebMvc.Models
{
    public class ClaimGroupedViewModel
    {
        public string CompanyName { get; set; }
        public List<ClaimItemViewModel> ClaimViewModelList { get; set; }
    }

    public class ClaimItemViewModel : ClaimModel
    {
        [Display(Name = "Policy No.")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Policy Type")]
        public string PolicyTypeName { get; set; }

        public int InsuranceProviderId { get; set; }
        public string StatusName { get; set; }
        // For Create
        public List<ExistingClientsViewModel> Clients { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }
        public string ExpiryDateString { get; set; }

        [Display(Name = "Sum Insured")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumInsured { get; set; }
        
        // For Edit
        public List<ClaimStatus> Statuses { get; set; }
        public List<ClientPoliciesListModel> Policies { get; set; }

        // For Details
        public string ClientName { get; set; }
        public bool IsOrganization { get; set; }
        public string OrganizationName { get; set; }


    }

    public class ClaimModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Claim Date")]
        public DateTime ClaimDate { get; set; }

        [Required]
        [Range(0.01,Double.MaxValue)]
        [Display(Name = "Claim Amount")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal ClaimAmount { get; set; }

        [Display(Name = "Claim No.")]
        [Required]
        public string ClaimNumber { get; set; }

        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Insured")]
        public int ClientId { get; set; }

        [Required]
        public int PolicyId { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public string ReturnUrl { get; set; }
    }
}