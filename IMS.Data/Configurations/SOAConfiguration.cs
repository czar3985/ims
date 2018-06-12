using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Data.Configurations
{
    class SoaConfiguration : EntityTypeConfiguration<Soa>
    {
        public SoaConfiguration()
        {
            this.HasRequired(p => p.Client);
            this.HasRequired(p => p.Status);
        }
    } 
}
