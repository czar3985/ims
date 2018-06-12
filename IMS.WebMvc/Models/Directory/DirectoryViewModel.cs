using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebMvc.Models
{
    public class DirectoryViewModel
    {
        public int Id { get; set; }

        [Display(Name ="Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Contact Nos.")]
        public string ContactNumbers { get; set; }

        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
    }
}