using IMS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMS.WebMvc.Models
{
    public class SoaIndexModel : SoaModel
    {
        public List<SoaModel> SoaList { get; set; }
        public List<ExistingClientsViewModel> Clients { get; set; }
        public List<InsuranceProvider> Companies { get; set; }
        public bool HasError { get; set; }
        public string Error { get; internal set; }
        // For Edit
        public List<SoaStatus> Statuses { get; set; }
    }

    public class SoaModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public string Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal TotalAmountDue { get; set; }

        // Foreign Keys
        [Required]
        [Display(Name ="Insured")]
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int InsuranceProviderId { get; set; }

        // Generated
        public string ClientName { get; set; }
        public string CompanyName { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? TotalAmountDueWithEwt { get; set; }

        public string StatusName { get; set; }
        public string Address { get; set; }
        public bool IsOrganization { get; set; }
        public string OrganizationName { get; set; }

        public List<SoaGroupedByType> SoaGroups { get; set; }
    }

    public class SoaGroupedByType
    {
        public string PolicyTypeName { get; set; }
        public List<SoaTableEntry> SoaTableEntries { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? PremiumSum { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? TaxSum { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? TotalSum { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? EwtSum { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? AmountDueSum { get; set; }
    }

    public class SoaTableEntry
    {
        public DateTime IssueDate { get; set; }
        public int PolicyTypeId { get; set; }
        public string PolicyNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Premium { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Tax { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Total { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Ewt { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? AmountDue { get; set; }
    }
}