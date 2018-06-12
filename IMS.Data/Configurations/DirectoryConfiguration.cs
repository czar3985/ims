using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using IMS.Entities;

namespace IMS.Data.Configurations
{
    class DirectoryConfiguration : EntityTypeConfiguration<Directory>
    {
        public DirectoryConfiguration()
        {
        }
    }
}
