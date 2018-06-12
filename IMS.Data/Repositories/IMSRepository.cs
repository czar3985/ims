using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Data.Repositories
{
    public class ClaimRepository : GenericRepository<Claim>
    {
        public ClaimRepository(DbContext context) : base(context) { }
    }

    public class ClaimStatusRepository : GenericRepository<ClaimStatus>
    {
        public ClaimStatusRepository(DbContext context) : base(context) { }
    }

    public class ClientRepository : GenericRepository<Client>
    {
        public ClientRepository(DbContext context) : base(context) { }
    }

    public class FormRepository : GenericRepository<Form>
    {
        public FormRepository(DbContext context) : base(context) { }
    }

    public class DocumentTypeRepository : GenericRepository<DocumentType>
    {
        public DocumentTypeRepository(DbContext context) : base(context) { }
    } 

    public class InvoiceRepository : GenericRepository<Invoice>
    {
        public InvoiceRepository(DbContext context) : base(context) { }
    }

    public class InvoiceStatusRepository : GenericRepository<InvoiceStatus>
    {
        public InvoiceStatusRepository(DbContext context) : base(context) { }
    } 

    public class RiskRepository : GenericRepository<Risk>
    {
        public RiskRepository(DbContext context) : base(context) { }
    }

    public class EndorsementRepository : GenericRepository<Endorsement>
    {
        public EndorsementRepository(DbContext context) : base(context) { }
    }

    public class ParticularRepository : GenericRepository<Particular>
    {
        public ParticularRepository(DbContext context) : base(context) { }
    }

    public class ParticularTypeRepository : GenericRepository<ParticularType>
    {
        public ParticularTypeRepository(DbContext context) : base(context) { }
    }

    public class PolicyRepository : GenericRepository<Policy>
    {
        public PolicyRepository(DbContext context) : base(context) { }
    }

    public class InsuranceProviderRepository : GenericRepository<InsuranceProvider>
    {
        public InsuranceProviderRepository(DbContext context) : base(context) { }
    }

    public class PolicyTypeRepository : GenericRepository<PolicyType>
    {
        public PolicyTypeRepository(DbContext context) : base(context) { }
    }

    public class PolicyStatusRepository : GenericRepository<PolicyStatus>
    {
        public PolicyStatusRepository(DbContext context) : base(context) { }
    }

    public class AgentRepository : GenericRepository<Agent>
    {
        public AgentRepository(DbContext context) : base(context) { }
    }

    public class PolicyAttachmentRepository : GenericRepository<PolicyAttachment>
    {
        public PolicyAttachmentRepository(DbContext context) : base(context) { }
    }

    public class ActivityLogRepository : GenericRepository<ActivityLog>
    {
        public ActivityLogRepository(DbContext context) : base(context) { }
    }

    public class SoaRepository : GenericRepository<Soa>
    {
        public SoaRepository(DbContext context) : base(context) { }
    }

    public class SoaStatusRepository : GenericRepository<SoaStatus>
    {
        public SoaStatusRepository(DbContext context) : base(context) { }
    }

    public class DefaultRebateRepository : GenericRepository<DefaultRebate>
    {
        public DefaultRebateRepository(DbContext context) : base(context) { }
    }

    public class DirectoryRepository : GenericRepository<Directory>
    {
        public DirectoryRepository(DbContext context) : base(context) { }
    }

    public class OfferRepository : GenericRepository<Offer>
    {
        public OfferRepository(DbContext context) : base(context) { }
    }

    public class OfferStatusRepository : GenericRepository<OfferStatus>
    {
        public OfferStatusRepository(DbContext context) : base(context) { }
    }
}
