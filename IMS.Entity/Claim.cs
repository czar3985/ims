using System;
using System.Collections.Generic;

namespace IMS.Entities
{
    public class Claim
    {
        public int Id { get; set; }
        public DateTime ClaimDate { get; set; }
        public decimal ClaimAmount { get; set; }
        public string ClaimNumber { get; set; }
        public string Remarks { get; set; }
        // Foreign Keys
        public int PolicyId { get; set; }
        public int StatusId { get; set; }
        // Navigation Properties
        public virtual Policy Policy { get; set; }
        public virtual ClaimStatus Status { get; set; }
        public virtual IList<ClaimAttachment> ClaimAttachments { get; set; }
    }

    public class ClaimStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ClaimAttachment
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public string FileUrl { get; set; }
        public string Remarks { get; set; }
        // Foreign Keys
        public int ClaimId { get; set; }
        // Navigation Properties
        public virtual Claim Claim { get; set; }
    }
}
