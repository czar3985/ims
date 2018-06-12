using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMS.WebMvc.Models
{
    public class ClientViewModel
    {
        public List<ClientsListModel> ClientsList { get; set; }
    }

    public class ClientsListModel: ClientModel
    {
        [Display(Name ="Client Name")]
        public string ClientName { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Total Business")]
        public decimal? TotalBusiness { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        [Display(Name = "Total Claim")]
        public decimal? TotalClaim { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Balance { get; set; }
    }

    public class ClientModel
    {
        public int Id { get; set; }

        [Display(Name = "Account No.")]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "Is Organization")]
        public bool IsOrganization { get; set; }

        [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Contact No.")]
        public string ContactNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //[DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        //[Range(0.00, Double.MaxValue)]
        //public decimal Rebate { get; set; }

        // From Client
        public string ReturnUrl { get; set; }

        public string Remarks { get; set; }
    }

    public class ClientSimple
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string AccountNumber { get; set; }
        //public decimal Rebate { get; set; }
    }
}