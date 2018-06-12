using IMS.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebMvc.Models
{
    public class FormViewModel
    {
        public FormModel NewForm { get; set; }
        public List<FormModel> FormsList { get; set; }
        public List<DocumentType> DocumentTypes { get; set; }
        public List<InsuranceProvider> InsuranceProviders { get; set; }
        public List<ClientSimple> Clients { get; set; }
    }

    public class FormModel
    {
        public int Id { get; set; }

        public string Remarks { get; set; }

        [Required]
        [Display(Name = "File")]
        public string FileName { get; set; }

        public string FileUrl { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int InsuranceProviderId { get; set; }

        [Required]
        [Display(Name = "Document Type")]
        public int DocumentTypeId { get; set; }

        // generated
        public string DocumentTypeName { get; set; }
        public string CompanyName { get; set; }
    }

    public class EmailFormModel
    {
        public int FormId { get; set; }
        public int ClientId { get; set; }
        public string Email { get; set; }
        public string OtherEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}