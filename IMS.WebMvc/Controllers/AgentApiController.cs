using IMS.Entities;
using IMS.WebMvc.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IMS.WebMvc.Controllers
{
    [RoutePrefix("api/AgentApi")]
    public class AgentApiController : BaseApiController
    {
        public HttpResponseMessage Post(Agent model)
        {
            try
            {
                Uow.Agents.Add(model);
                Uow.SaveChanges();

                LogAdd(model);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        #region Logs
        private void LogAdd(Agent entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.AddAgent);
            creator.AddAgent(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Agent entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ModifyAgent);
            creator.AddAgent(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(Agent entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.DeleteAgent);
            creator.AddAgent(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}