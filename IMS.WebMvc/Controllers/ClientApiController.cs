using IMS.Entities;
using IMS.WebMvc.Models;
using IMS.WebMvc.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IMS.WebMvc.Controllers
{
    [RoutePrefix("api/ClientApi")]
    public class ClientApiController : BaseApiController
    {
        public HttpResponseMessage Post(ClientModel model)
        {
            try
            {
                var entity = AutoMapper.Mapper.Map<Client>(model);

                Uow.Clients.Add(entity);
                Uow.SaveChanges();

                LogAdd(entity);

                var retEntity = AutoMapper.Mapper.Map<ClientSimple>(entity);
                if (entity.IsOrganization)
                    retEntity.ClientName = entity.OrganizationName;

                return Request.CreateResponse(HttpStatusCode.OK, retEntity);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        #region Logs
        private void LogAdd(Client entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.AddClient);
            creator.AddClient(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Client entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ModifyClient);
            creator.AddClient(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(Client entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.DeleteClient);
            creator.AddClient(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}