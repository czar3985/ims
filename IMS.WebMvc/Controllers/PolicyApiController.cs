using AutoMapper.QueryableExtensions;
using IMS.Entities;
using IMS.WebMvc.Models;
using IMS.WebMvc.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IMS.WebMvc.Controllers
{
    [RoutePrefix("api/PolicyApi")]
    public class PolicyApiController : BaseApiController
    {
        // GET: api/DashboardApi/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var list = Uow.Policies.GetAll()
                    .Where(p => p.ClientId == id)
                    .ProjectTo<ClientPoliciesListModel>();

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        public HttpResponseMessage Post(PolicyModel model)
        {
            try
            {
                model.StatusId = GetPolicyStatusId("Active");

                var entity = AutoMapper.Mapper.Map<Policy>(model);
                if (entity.InceptionDate.Year < 1900)
                    entity.InceptionDate = DateTime.Now;

                if (entity.Id == 0)
                    CreateNewPolicy(entity);
                else
                    SaveModifications(entity);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private void CreateNewPolicy(Policy entity)
        {
            Uow.Policies.Add(entity);
            Uow.SaveChanges();

            LogAdd(entity);
        }

        private void SaveModifications(Policy entity)
        {
            var rIdList = Uow.Risks.GetAll()
                .Where(p => p.PolicyId == entity.Id)
                .Select(p => p.Id)
                .ToList();

            foreach (var item in entity.Risks)
            {
                if (item.Id == 0)
                {
                    item.PolicyId = entity.Id;
                    Uow.Risks.Add(item);
                }
                else
                {
                    Uow.Risks.Update(item);
                    rIdList.Remove(item.Id);
                }
            }

            foreach (var item in rIdList)
            {
                Uow.Risks.Delete(item);
            }

            Uow.Policies.Update(entity);
            Uow.SaveChanges();

            LogEdit(entity);
        }

        private int GetPolicyStatusId(string value)
        {
            int ret = Uow.PolicyStatuses.GetAll()
                .Where(p => p.Name == value)
                .Select(p => p.Id)
                .FirstOrDefault();
            return ret;
        }

        [HttpDelete]
        [Route("DeleteAttachment")]
        public HttpResponseMessage DeleteAttachment(int id)
        {
            try
            {
                var entity = Uow.PolicyAttachments.GetAll()
                    .Where(p => p.Id == id)
                    .FirstOrDefault();

                if (entity == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var localFullPath =  HttpContext.Current.Server.MapPath(entity.FileUrl);

                Uow.PolicyAttachments.Delete(id);
                Uow.SaveChanges();

                if(File.Exists(localFullPath))
                    File.Delete(localFullPath);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        #region Logs
        private void LogAdd(Policy entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.AddPolicy);
            creator.AddPolicy(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Policy entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ModifyPolicy);
            creator.AddPolicy(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(Policy entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.DeletePolicy);
            creator.AddPolicy(entity.Id);
            creator.SaveToLog(false);
        }
        #endregion
    }
}