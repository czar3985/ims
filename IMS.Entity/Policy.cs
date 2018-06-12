using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entities
{
    public class Policy
    {
        public int Id { get; set; }
        public decimal SumInsured { get; set; }
        public string Remarks { get; set; }
        public DateTime InceptionDate { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Premium { get; set; }
        public double Rate { get; set; }
        public string PolicyNumber { get; set; }
        public string RenewalPolicyNumber { get; set; }
        public double Rebate { get; set; }
        public decimal Commission { get; set; }
        public string Address { get; set; }
        // Foreign Keys
        public int ClientId { get; set; }
        public int AgentId { get; set; }
        public int InsuranceProviderId { get; set; }
        public int PolicyTypeId { get; set; }
        public int StatusId { get; set; }
        // Navigation Properties
        public virtual Client Client { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual InsuranceProvider InsuranceProvider { get; set; }
        public virtual PolicyType PolicyType { get; set; }
        public virtual PolicyStatus Status { get; set; }
        public virtual IList<PolicyAttachment> PolicyAttachments { get; set; }
        public virtual IList<Risk> Risks { get; set; }
        public virtual IList<Endorsement> Endorsements { get; set; }
        public virtual IList<Invoice> Invoices { get; set; }
        public virtual IList<Claim> Claims { get; set; }
    }

    public class InsuranceProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Navigation Properties
        public virtual IList<Policy> Policies { get; set; }
        public virtual IList<Form> Forms { get; set; }
    }

    public class PolicyType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Agent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Navigation Properties
        public virtual IList<Policy> Policies { get; set; }
    }

    public class PolicyStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PolicyAttachment
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string Remarks { get; set; }
        // Foreign Keys
        public int PolicyId { get; set; }
        // Navigation Properties
        public virtual Policy Policy { get; set; }
    }

    public class Risk
    {
        public int Id { get; set; }
        public decimal AmountInsured { get; set; }
        public double Rate { get; set; }
        public string Remarks { get; set; }
        public decimal Premium { get; set; }
        public string Address { get; set; }
        // Foreign Keys
        public int PolicyId { get; set; }
        // Navigation Properties
        public virtual Policy Policy { get; set; }
    }

    public class Endorsement
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string EndorsementNumber { get; set; }
        public decimal EndorsementAmount { get; set; }
        public decimal TotalEndorsementAmount { get; set; }
        public string Remarks { get; set; }
        public string Mortgagee { get; set; }
        public decimal Mt { get; set; }
        public decimal Pt { get; set; }
        public decimal Dst { get; set; }
        public decimal Fst { get; set; }
        public decimal Vat { get; set; }
        // Foreign Keys
        public int PolicyId { get; set; }
        // Navigation Properties
        public virtual Policy Policy { get; set; }
    }

    public class DefaultRebate
    {
        public int Id { get; set; }
        public double Rate { get; set; }
        // Foreign Keys
        public int InsuranceProviderId { get; set; }
        public int PolicyTypeId { get; set; }
        // Navigation Properties
        public virtual InsuranceProvider InsuranceProvider { get; set; }
        public virtual PolicyType PolicyType { get; set; }
    }
}
