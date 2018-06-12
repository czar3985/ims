using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Data.Configurations
{
    class FormConfiguration : EntityTypeConfiguration<Form>
    {
        public FormConfiguration()
        {
            this.HasRequired(p => p.InsuranceProvider);
            this.HasRequired(p => p.DocumentType);
        }
    } 
}
