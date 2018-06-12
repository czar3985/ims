using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entities
{
    public class Form
    {
        public int Id { get; set; }
        public string Remarks { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        // Foreign Keys
        public int InsuranceProviderId { get; set; }
        public int DocumentTypeId{ get; set; }
        // Navigation Properties
        public virtual InsuranceProvider InsuranceProvider { get; set; }
        public virtual DocumentType DocumentType { get; set; }
    }

    public class DocumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    } 
}
