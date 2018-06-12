using IMS.Entities;
using IMS.WebMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IMS.WebMvc.Controllers
{
    [RoutePrefix("api/PolicyTypeApi")]
    public class PolicyTypeApiController : BaseApiController
    {
        public HttpResponseMessage Post(PolicyType model)
        {
            try
            {
                Uow.PolicyTypes.Add(model);

                //// START: Add Default Rebates
                var companyList = GetInsuranceProviders();
                foreach (var company in companyList)
                {
                    var defaultRebateItem = new DefaultRebate
                    {
                        InsuranceProviderId = company.Id,
                        PolicyTypeId = model.Id,
                        Rate = 0
                    };
                    Uow.DefaultRebates.Add(defaultRebateItem);
                }
                //// END: Add Default Rebates

                Uow.SaveChanges();

                LogAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private List<InsuranceProvider> GetInsuranceProviders()
        {
            var list = Uow.InsuranceProviders.GetAll()
                .OrderBy(i => i.Name)
                .ToList();
            return list;
        }

        #region Logs
        private void LogAdd(PolicyType entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.AddPolicyType);
            creator.AddPolicyType(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(PolicyType entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ModifyPolicyType);
            creator.AddPolicyType(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(PolicyType entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.DeletePolicyType);
            creator.AddPolicyType(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}