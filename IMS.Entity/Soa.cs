using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entities
{
    public class Soa
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime StartDate { get; set; } //EDWIN
        public DateTime DueDate { get; set; }
        public string Remarks { get; set; }
        public decimal TotalAmountDue { get; set; }

        // Foreign Keys
        public int ClientId { get; set; }
        public int StatusId { get; set; }
        public int InsuranceProviderId { get; set; }

        // Navigation Properties
        public virtual Client Client { get; set; }
        public virtual SoaStatus Status { get; set; }
        public virtual InsuranceProvider InsuranceProvider { get; set; }
    }

    public class SoaStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
