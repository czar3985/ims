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
    [RoutePrefix("api/CompanyApi")]
    public class CompanyApiController : BaseApiController
    {
        public HttpResponseMessage Post(InsuranceProvider model)
        {
            try
            {
                Uow.InsuranceProviders.Add(model);

                //// START: Add Default Rebates
                var policyTypeList = GetPolicyTypes();
                foreach (var policyType in policyTypeList)
                {
                    var defaultRebateItem = new DefaultRebate
                    {
                        InsuranceProviderId = model.Id,
                        PolicyTypeId = policyType.Id,
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

        public HttpResponseMessage GetCompanyListPerClient(int id)
        {
            try
            {
                var list = Uow.Policies.GetAll()
                    .Where(p => p.ClientId == id)
                    .Select(p => p.InsuranceProvider)
                    .Distinct().ToList();

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private List<PolicyType> GetPolicyTypes()
        {
            var list = Uow.PolicyTypes.GetAll()
                .OrderBy(p => p.Name).ToList();

            return list;
        }

        #region Logs
        private void LogAdd(InsuranceProvider entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.AddInsuranceProvider);
            creator.AddInsuranceProvider(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(InsuranceProvider entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ModifyInsuranceProvider);
            creator.AddInsuranceProvider(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(InsuranceProvider entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.DeleteInsuranceProvider);
            creator.AddInsuranceProvider(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}