using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Entities
{
    public enum ActivityLogTypeEnum
    {
        AddAgent,
        ModifyAgent,
        DeleteAgent,

        AddClaimAttachment,
        ModifyClaimAttachment,
        DeleteClaimAttachment,

        AddClaim,
        ModifyClaim,
        DeleteClaim,

        AddClaimStatus,
        ModifyClaimStatus,
        DeleteClaimStatus,

        AddClient,
        ModifyClient,
        DeleteClient,

        AddDocumentType,
        ModifyDocumentType,
        DeleteDocumentType,

        AddForm,
        ModifyForm,
        DeleteForm,

        AddInsuranceProvider,
        ModifyInsuranceProvider,
        DeleteInsuranceProvider,

        AddInvoice,
        ModifyInvoice,
        DeleteInvoice,

        AddInvoiceStatus,
        ModifyInvoiceStatus,
        DeleteInvoiceStatus,

        AddParticular,
        ModifyParticular,
        DeleteParticular,

        AddParticularType,
        ModifyParticularType,
        DeleteParticularType,

        AddPolicy,
        ModifyPolicy,
        ChangeStatePolicy,
        DeletePolicy,

        AddPolicyAttachment,
        ModifyPolicyAttachment,
        DeletePolicyAttachment,

        AddPolicyStatus,
        ModifyPolicyStatus,
        DeletePolicyStatus,

        AddPolicyType,
        ModifyPolicyType,
        DeletePolicyType,

        AddRisk,
        ModifyRisk,
        DeleteRisk,

        AddEndorsement,
        ModifyEndorsement,
        DeleteEndorsement,

        AddSoa,
        ModifySoa,
        DeleteSoa,

        AddSoaStatus,
        ModifySoaStatus,
        DeleteSoaStatus,

        AddDefaultRebate,
        ModifyDefaultRebate,

        AddOffer,
        ModifyOffer,
        DeleteOffer
    }

    public class ActivityLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ActivityLogTypeEnum ActivityLogType { get; set; }
        public DateTime PostedDate { get; set; }
        public string Message { get; set; }

        public int? AgentId { get; set; }
        public int? ClaimAttachmentId { get; set; }
        public int? ClaimId { get; set; }
        public int? ClaimStatusId { get; set; }
        public int? ClientId { get; set; }
        public int? DocumentTypeId { get; set; }
        public int? FormId { get; set; }
        public int? InsuranceProviderId { get; set; }
        public int? InvoiceId { get; set; }
        public int? InvoiceStatusId { get; set; }
        public int? ParticularId { get; set; }
        public int? ParticularTypeId { get; set; }
        public int? PolicyId { get; set; }
        public int? PolicyAttachmentId { get; set; }
        public int? PolicyStatusId { get; set; }
        public int? PolicyTypeId { get; set; }
        public int? RiskId { get; set; }
        public int? EndorsementId { get; set; }
        public int? SoaId { get; set; }
        public int? SoaStatusId { get; set; }
        public int? OfferId { get; set; }
        public int? OfferStatusId { get; set; }

        // Navigation Properties
        public Agent Agent { get; set; }
        public ClaimAttachment ClaimAttachment { get; set; }
        public Claim Claim { get; set; }
        public ClaimStatus ClaimStatus { get; set; }
        public Client Client { get; set; }
        public DocumentType DocumentType { get; set; }
        public Form Form { get; set; }
        public InsuranceProvider InsuranceProvider { get; set; }
        public Invoice Invoice { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
        public Particular Particular { get; set; }
        public ParticularType ParticularType { get; set; }
        public Policy Policy { get; set; }
        public PolicyAttachment PolicyAttachment { get; set; }
        public PolicyStatus PolicyStatus { get; set; }
        public PolicyType PolicyType { get; set; }
        public Risk Risk { get; set; }
        public Endorsement Endorsement { get; set; }
        public Soa Soa { get; set; }
        public SoaStatus SoaStatus { get; set; }
        public Offer Offer { get; set; }
        public OfferStatus OfferStatus { get; set; }
    } 
}
