using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebMvc.Models
{
    public class PolicyGroupedViewModel
    {
        public string CompanyName { get; set; }
        public List<PolicyItemViewModel> PolicyViewModelList { get; set; }
    }

    public class PolicyItemViewModel
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public string CompanyName { get; set; }
        public bool IsOrganization { get; set; }
        public string OrganizationName { get; set; }
        public string ClientName { get; set; }
        public int InsuranceProviderId { get; set; }
        public string PolicyTypeName { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal SumInsured { get; set; }

        public DateTime ExpiryDate { get; set; }
        public string StatusName { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Premium { get; set; }
    }
}