using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using IMS.Data.Configurations;
using IMS.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace IMS.Data
{
    public class DataContext : DbContext
    {
        public static string ConnectionStringName
        {
            get
            {
                if (ConfigurationManager.AppSettings["ConnectionStringName"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["ConnectionStringName"].ToString();
                }

                return "DefaultConnection";
            }
        }

        public static string AuthConnectionStringName
        {
            get
            {
                if (ConfigurationManager.AppSettings["AuthConnectionStringName"]
                    != null)
                {
                    return ConfigurationManager.
                        AppSettings["AuthConnectionStringName"].ToString();
                }

                return "DefaultConnection";
            }
        }

        public static int ExpiringPolicyDays
        {
            get
            {
                if (ConfigurationManager.AppSettings["ExpiringPolicyDays"]
                    != null)
                {
                    int days = Int32.Parse(ConfigurationManager.AppSettings["ExpiringPolicyDays"]);
                    return days;
                }

                return 60;
            }
        }

        private static string _adminEmailAdd = String.Empty;
        public static string AdminEmailAdd
        {
            get
            {
                if (_adminEmailAdd == String.Empty)
                {
                    if (ConfigurationManager.AppSettings["AdminEmail"] != null)
                    {
                        var adminEmail = ConfigurationManager.AppSettings["AdminEmail"];
                        _adminEmailAdd = adminEmail;
                        return adminEmail;
                    }
                }

                return _adminEmailAdd;
            }
        }

        private static string _serverAddress = String.Empty;
        public static string ServerAddress
        {
            get
            {
                if (_serverAddress == String.Empty)
                {
                    if (ConfigurationManager.AppSettings["ServerAddress"] != null)
                    {
                        var serverAddress = ConfigurationManager.AppSettings["ServerAddress"];
                        _serverAddress = serverAddress;
                        return serverAddress;
                    }
                }

                return _serverAddress;
            }
        }

        static DataContext()
        {
            //Database.SetInitializer(new CustomDatabaseInitializer());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, IMS.Data.Migrations.Configuration>()); 
            //Database.SetInitializer(new DropCreateDatabaseAlways<DataContext>());
        }

        public DataContext()
            : base(nameOrConnectionString: DataContext.ConnectionStringName) 
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;

            _modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            _modelBuilder.Configurations.Add(new ClaimConfiguration());
            _modelBuilder.Configurations.Add(new ClaimAttachmentConfiguration());
            _modelBuilder.Configurations.Add(new ClientConfiguration());
            _modelBuilder.Configurations.Add(new FormConfiguration());
            _modelBuilder.Configurations.Add(new InvoiceConfiguration());
            _modelBuilder.Configurations.Add(new RiskConfiguration());
            _modelBuilder.Configurations.Add(new EndorsementConfiguration());
            _modelBuilder.Configurations.Add(new ParticularConfiguration());
            _modelBuilder.Configurations.Add(new PolicyConfiguration());
            _modelBuilder.Configurations.Add(new InsuranceProviderConfiguration());
            _modelBuilder.Configurations.Add(new PolicyAttachmentConfiguration());
            _modelBuilder.Configurations.Add(new AgentConfiguration());
            _modelBuilder.Configurations.Add(new ActivityLogConfiguration());
            _modelBuilder.Configurations.Add(new SoaConfiguration());
            _modelBuilder.Configurations.Add(new DirectoryConfiguration());
            _modelBuilder.Configurations.Add(new OfferConfiguration());
            //_modelBuilder.Configurations.Add(new RiskRemarkConfiguration());
        }

        public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }
        public DbSet<PolicyStatus> PolicyStatuses { get; set; }
        public DbSet<ClaimStatus> ClaimStatuses { get; set; }
        public DbSet<PolicyType> PolicyTypes { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<ParticularType> ParticularTypes { get; set; }
        public DbSet<SoaStatus> SoaStatuses { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DefaultRebate> DefaultRebates { get; set; }
        public DbSet<Directory> Directories { get; set; }
        public DbSet<OfferStatus> OfferStatuses { get; set; }

        private DbModelBuilder _modelBuilder;
    }
}
