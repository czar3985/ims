using IMS.WebMvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.WebMvc.Services
{
    public class AttributeProviderService : BaseService
    {
        public AttributeProviderService(BaseController controller)
            :base(controller)
        {
        }

        public AttributeProviderService(BaseApiController controller)
            :base(controller)
        {
        }

        public string GetPolicyTypeNameFromId(int Id)
        {
            var policyTypeName = _uow.PolicyTypes.GetAll()
                    .Where(p => p.Id == Id)
                    .FirstOrDefault()
                    .Name;
            return policyTypeName;
        }

        public string GetCompanyNameFromId(int Id)
        {
            var companyName = _uow.InsuranceProviders.GetAll()
                    .Where(i => i.Id == Id)
                    .FirstOrDefault()
                    .Name;
            return companyName;
        }

        public string GetSoaStatusNameFromId(int Id)
        {
            var soaStatusName = _uow.SoaStatuses.GetAll()
                    .Where(i => i.Id == Id)
                    .FirstOrDefault()
                    .Name;
            return soaStatusName;
        }

        public int GetInvoiceStatusIdFromName(string statusName)
        {
            var statusId = _uow.InvoiceStatuses.GetAll()
                    .Where(i => i.Name.ToLower() == statusName)
                    .FirstOrDefault()
                    .Id;
            return statusId;
        }

        public int GetSoaStatusIdFromName(string statusName)
        {
            var statusId = _uow.SoaStatuses.GetAll()
                    .Where(i => i.Name.ToLower() == statusName)
                    .FirstOrDefault()
                    .Id;
            return statusId;
        }

        public int GetPolicyStatusIdFromName(string statusName)
        {
            var statusId = _uow.PolicyStatuses.GetAll()
                    .Where(i => i.Name.ToLower() == statusName)
                    .FirstOrDefault()
                    .Id;
            return statusId;
        }

        public int GetClaimStatusIdFromName(string statusName)
        {
            var statusId = _uow.ClaimStatuses.GetAll()
                    .Where(i => i.Name.ToLower() == statusName)
                    .FirstOrDefault()
                    .Id;
            return statusId;
        }
    }
}