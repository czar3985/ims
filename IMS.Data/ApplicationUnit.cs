using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Data.Repositories;
using System.Data.Entity.Infrastructure;

namespace IMS.Data
{
    public class ApplicationUnit : IDisposable
    {
        private DataContext _context = new DataContext();

        private ClaimRepository _claims = null;
        public ClaimRepository Claims
        {
            get
            {
                if (this._claims == null)
                    this._claims = new ClaimRepository(this._context);
                return this._claims;
            }
        }

        private ClaimStatusRepository _claimStatuses = null;
        public ClaimStatusRepository ClaimStatuses
        {
            get
            {
                if (this._claimStatuses == null)
                    this._claimStatuses = new ClaimStatusRepository(this._context);
                return this._claimStatuses;
            }
        }

        private ClientRepository _clients = null;
        public ClientRepository Clients
        {
            get
            {
                if (this._clients == null)
                    this._clients = new ClientRepository(this._context);
                return this._clients;
            }
        }

        private FormRepository _forms = null;
        public FormRepository Forms
        {
            get
            {
                if (this._forms == null)
                    this._forms = new FormRepository(this._context);
                return this._forms;
            }
        }

        private DocumentTypeRepository _DocumentTypes = null;
        public DocumentTypeRepository DocumentTypes
        {
            get
            {
                if (this._DocumentTypes == null)
                    this._DocumentTypes = new DocumentTypeRepository(this._context);
                return this._DocumentTypes;
            }
        }

        private InvoiceRepository _invoices = null;
        public InvoiceRepository Invoices
        {
            get
            {
                if (this._invoices == null)
                    this._invoices = new InvoiceRepository(this._context);
                return this._invoices;
            }
        }

        private InvoiceStatusRepository _InvoiceStatuss = null;
        public InvoiceStatusRepository InvoiceStatuses
        {
            get
            {
                if (this._InvoiceStatuss == null)
                    this._InvoiceStatuss = new InvoiceStatusRepository(this._context);
                return this._InvoiceStatuss;
            }
        }

        private RiskRepository _risks = null;
        public RiskRepository Risks
        {
            get
            {
                if (this._risks == null)
                    this._risks = new RiskRepository(this._context);
                return this._risks;
            }
        }

        private EndorsementRepository _endorsements= null;
        public EndorsementRepository Endorsements
        {
            get
            {
                if (this._endorsements == null)
                    this._endorsements = new EndorsementRepository(this._context);
                return this._endorsements;
            }
        }

        private ParticularRepository _particulars = null;
        public ParticularRepository Particulars
        {
            get
            {
                if (this._particulars == null)
                    this._particulars = new ParticularRepository(this._context);
                return this._particulars;
            }
        }

        private ParticularTypeRepository _particularTypes = null;
        public ParticularTypeRepository ParticularTypes
        {
            get
            {
                if (this._particularTypes == null)
                    this._particularTypes = new ParticularTypeRepository(this._context);
                return this._particularTypes;
            }
        }

        private PolicyRepository _policies = null;
        public PolicyRepository Policies
        {
            get
            {
                if (this._policies == null)
                    this._policies = new PolicyRepository(this._context);
                return this._policies;
            }
        }

        private InsuranceProviderRepository _insuranceProviders = null;
        public InsuranceProviderRepository InsuranceProviders
        {
            get
            {
                if (this._insuranceProviders == null)
                    this._insuranceProviders = new InsuranceProviderRepository(this._context);
                return this._insuranceProviders;
            }
        }

        private PolicyTypeRepository _policyTypes = null;
        public PolicyTypeRepository PolicyTypes
        {
            get
            {
                if (this._policyTypes == null)
                    this._policyTypes = new PolicyTypeRepository(this._context);
                return this._policyTypes;
            }
        }

        private PolicyStatusRepository _policyStatuses = null;
        public PolicyStatusRepository PolicyStatuses
        {
            get
            {
                if (this._policyStatuses == null)
                    this._policyStatuses = new PolicyStatusRepository(this._context);
                return this._policyStatuses;
            }
        }

        private AgentRepository _agents = null;
        public AgentRepository Agents
        {
            get
            {
                if (this._agents == null)
                    this._agents = new AgentRepository(this._context);
                return this._agents;
            }
        }

        private PolicyAttachmentRepository _policyAttachments = null;
        public PolicyAttachmentRepository PolicyAttachments
        {
            get
            {
                if (this._policyAttachments == null)
                    this._policyAttachments = new PolicyAttachmentRepository(this._context);
                return this._policyAttachments;
            }
        }

        private ActivityLogRepository _ActivityLogs = null;
        public ActivityLogRepository ActivityLogs
        {
            get
            {
                if (this._ActivityLogs == null)
                    this._ActivityLogs = new ActivityLogRepository(this._context);
                return this._ActivityLogs;
            }
        }

        private SoaRepository _soas = null;
        public SoaRepository Soas
        {
            get
            {
                if (this._soas == null)
                    this._soas = new SoaRepository(this._context);
                return this._soas;
            }
        }

        private SoaStatusRepository _soaStatuses = null;
        public SoaStatusRepository SoaStatuses
        {
            get
            {
                if (this._soaStatuses == null)
                    this._soaStatuses = new SoaStatusRepository(this._context);
                return this._soaStatuses;
            }
        }

        private DefaultRebateRepository _defaultRebates = null;
        public DefaultRebateRepository DefaultRebates
        {
            get
            {
                if (this._defaultRebates == null)
                    this._defaultRebates = new DefaultRebateRepository(this._context);
                return this._defaultRebates;
            }
        }

        private DirectoryRepository _Directories = null;
        public DirectoryRepository Directories
        {
            get
            {
                if (this._Directories == null)
                    this._Directories = new DirectoryRepository(this._context);
                return this._Directories;
            }
        }

        private OfferRepository _offers = null;
        public OfferRepository Offers
        {
            get
            {
                if (this._offers == null)
                    this._offers = new OfferRepository(this._context);
                return this._offers;
            }
        }

        private OfferStatusRepository _offerStatuses = null;
        public OfferStatusRepository OfferStatuses
        {
            get
            {
                if (this._offerStatuses == null)
                    this._offerStatuses = new OfferStatusRepository(this._context);
                return this._offerStatuses;
            }
        }

        public ApplicationUnit()
        {
            //_context.Database.Initialize(true);
            _context.Database.Log = Console.Write;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (this._context != null)
            {
                this._context.Dispose();
            }
        }
    }
}
