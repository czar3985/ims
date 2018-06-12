using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Data.Configurations
{
    class InvoiceConfiguration : EntityTypeConfiguration<Invoice>
    {
        public InvoiceConfiguration()
        {
            this.HasRequired(p => p.Policy);
            this.HasRequired(p => p.Status);
            this.HasMany(p => p.Particulars)
                .WithRequired(q => q.Invoice);
        }
    }

    class ParticularConfiguration : EntityTypeConfiguration<Particular>
    {
        public ParticularConfiguration()
        {
            this.HasRequired(p => p.Invoice);
            this.HasRequired(p => p.ParticularType);
        }
    } 
}
