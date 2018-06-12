using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Data.Configurations
{
    class ClaimConfiguration : EntityTypeConfiguration<Claim>
    {
        public ClaimConfiguration()
        {
            this.HasRequired(p => p.Policy);
            this.HasRequired(p => p.Status);
            this.HasMany(p => p.ClaimAttachments)
                .WithRequired(q => q.Claim);
        }
    }

    class ClaimAttachmentConfiguration : EntityTypeConfiguration<ClaimAttachment>
    {
        public ClaimAttachmentConfiguration()
        {
            this.HasRequired(p => p.Claim);
        }
    }
}
