using IMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.WebMvc.Services
{
    public class LogCreatorBase
    {
        protected ActivityLog _entity;

        public LogCreatorBase(ActivityLogTypeEnum type)
        {
            _entity = new ActivityLog()
            {
                ActivityLogType = type,
                PostedDate = DateTime.Now
            };
        }

        public void AddAgent(int id)
        {
            _entity.AgentId = id;
        }

        public void AddClaimAttachment(int id)
        {
            _entity.ClaimAttachmentId = id;
        }

        public void AddClaim(int id)
        {
            _entity.ClaimId = id;
        }

        public void AddClaimStatus(int id)
        {
            _entity.ClaimStatusId = id;
        }

        public void AddClient(int id)
        {
            _entity.ClientId = id;
        }

        public void AddDocumentType(int id)
        {
            _entity.DocumentTypeId = id;
        }

        public void AddForm(int id)
        {
            _entity.FormId = id;
        }

        public void AddInsuranceProvider(int id)
        {
            _entity.InsuranceProviderId = id;
        }

        public void AddInvoice(int id)
        {
            _entity.InvoiceId = id;
        }

        public void AddParticular(int id)
        {
            _entity.ParticularId = id;
        }

        public void AddPolicy(int id)
        {
            _entity.PolicyId = id;
        }

        public void AddPolicyAttachment(int id)
        {
            _entity.PolicyAttachmentId = id;
        }

        public void AddPolicyType(int id)
        {
            _entity.PolicyTypeId = id;
        }

        public void AddRisk(int id)
        {
            _entity.RiskId = id;
        }

        public void AddEndorsement(int id)
        {
            _entity.EndorsementId = id;
        }

        public void AddSoa(int id)
        {
            _entity.SoaId = id;
        }

        public void AddMessage(string message)
        {
            _entity.Message = message;
        }

        public void AddDefaultRebate(string message)
        {
            _entity.Message = message;
        }
    }
}