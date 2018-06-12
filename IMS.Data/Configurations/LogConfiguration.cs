using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Data.Configurations
{
    class ActivityLogConfiguration : EntityTypeConfiguration<ActivityLog>
    {
        public ActivityLogConfiguration()
        {
            this.HasOptional(p => p.Agent);
            this.HasOptional(p => p.ClaimAttachment);
            this.HasOptional(p => p.Claim);
            this.HasOptional(p => p.ClaimStatus);
            this.HasOptional(p => p.Client);
            this.HasOptional(p => p.DocumentType);
            this.HasOptional(p => p.Form);
            this.HasOptional(p => p.InsuranceProvider);
            this.HasOptional(p => p.Invoice);
            this.HasOptional(p => p.Particular);
            this.HasOptional(p => p.ParticularType);
            this.HasOptional(p => p.Policy);
            this.HasOptional(p => p.PolicyAttachment);
            this.HasOptional(p => p.PolicyType);
            this.HasOptional(p => p.Risk);
            this.HasOptional(p => p.Endorsement);
        }
    } 
}
