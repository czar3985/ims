using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime PaidDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalAmountDue { get; set; }
        public string EndorsementNumber { get; set; }
        public double Rebate { get; set; }
        public string Remarks { get; set; }
        public string ORNumber { get; set; }
        public bool HasEWT { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DueDate { get; set; }
        // Foreign Keys
        public int PolicyId { get; set; }
        public int StatusId { get; set; }
        // Navigation Properties
        public virtual Policy Policy { get; set; }
        public virtual InvoiceStatus Status { get; set; }
        public virtual IList<Particular> Particulars { get; set; }
    }

    public class InvoiceStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Particular
    {
        public int Id { get; set; }
        public decimal ParticularAmount { get; set; }
        public string Remarks { get; set; }
        // Foreign Keys
        public int InvoiceId { get; set; }
        public int ParticularTypeId { get; set; }
        // Navigation Properties
        public virtual Invoice Invoice { get; set; }
        public virtual ParticularType ParticularType { get; set; }
    }

    public class ParticularType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
