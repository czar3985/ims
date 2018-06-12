using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Data.Configurations
{
    class InsuranceProviderConfiguration : EntityTypeConfiguration<InsuranceProvider>
    {
        public InsuranceProviderConfiguration()
        {
            this.HasMany(p => p.Policies)
                .WithRequired(q => q.InsuranceProvider);
            this.HasMany(p => p.Forms)
                .WithRequired(q => q.InsuranceProvider);
        }
    } 
    class PolicyConfiguration : EntityTypeConfiguration<Policy>
    {
        public PolicyConfiguration()
        {
            this.HasRequired(p => p.Client);
            this.HasRequired(p => p.Agent);
            this.HasRequired(p => p.InsuranceProvider);
            this.HasRequired(p => p.PolicyType);
            this.HasRequired(p => p.Status);
            this.HasMany(p => p.PolicyAttachments)
                .WithRequired(q => q.Policy);
            this.HasMany(p => p.Invoices)
                .WithRequired(q => q.Policy);
            this.HasMany(p => p.Claims)
                .WithRequired(q => q.Policy);
            this.HasMany(p => p.Risks)
                .WithRequired(q => q.Policy);
            this.HasMany(p => p.Endorsements)
                .WithRequired(q => q.Policy);
        }
    }

    class PolicyAttachmentConfiguration : EntityTypeConfiguration<PolicyAttachment>
    {
        public PolicyAttachmentConfiguration()
        {
            this.HasRequired(p => p.Policy);
        }
    }

    class AgentConfiguration : EntityTypeConfiguration<Agent>
    {
        public AgentConfiguration()
        {
            this.HasMany(p => p.Policies)
                .WithRequired(q => q.Agent);
        }
    }

    class RiskConfiguration : EntityTypeConfiguration<Risk>
    {
        public RiskConfiguration()
        {
            this.HasRequired(p => p.Policy);
        }
    }

    class EndorsementConfiguration : EntityTypeConfiguration<Endorsement>
    {
        public EndorsementConfiguration()
        {
            this.HasRequired(p => p.Policy);
        }
    }
}
