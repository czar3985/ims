using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Data.Configurations
{
    class ClientConfiguration : EntityTypeConfiguration<Client>
    {
        public ClientConfiguration()
        {
            this.HasMany(p => p.Policies)
                .WithRequired(q => q.Client);
            this.HasMany(p => p.Offers)
                .WithRequired(q => q.Client);
        }
    }

    class OfferConfiguration : EntityTypeConfiguration<Offer>
    {
        public OfferConfiguration()
        {
            this.HasRequired(p => p.Client);
        }
    }
}
