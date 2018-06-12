using IMS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMS.WebMvc.Models
{
    public class ClientDetailModel : ClientsListModel
    {
        public List<ClientPoliciesListModel> ClientPoliciesList { get; set; }
        public List<ClientClaimsListModel> ClientClaimsList { get; set; }

        public ClientOffersListModel NewOffer { get; set; }
        public List<ClientOffersListModel> ClientOffersList { get; set; }
        public List<OfferStatus> OfferStatuses { get; set; }
    }

    public class ClientPoliciesListModel
    {
        public int Id { get; set; }
        public int InsuranceProviderId { get; set; }
        public string CompanyName { get; set; }
        public string PolicyNumber { get; set; }
        public string StatusName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int PolicyTypeId { get; set; }
        public string PolicyTypeName { get; set; }
        public string LatestEndorsementNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumInsured { get; set; }

        [DisplayFormat(DataFormatString = "{0:F6}", ApplyFormatInEditMode = true)]
        [Range(0.00, 1.00)]
        public double Rate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Premium { get; set; }

        public double Rebate { get; set; }
    }

    public class ClientClaimsListModel
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyTypeName { get; set; }
        public DateTime ClaimDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal ClaimAmount { get; set; }

        public string Reference { get; set; }
    }

    public class ClientOffersListModel
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        [Required]
        [Display(Name = "File")]
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Remarks { get; set; }
    }
}