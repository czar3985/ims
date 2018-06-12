using IMS.Entities;
using IMS.WebMvc.Models;
using IMS.WebMvc.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace IMS.WebMvc.Controllers
{
    public class FormApiController : BaseApiController
    {
        // Send email
        public HttpResponseMessage Post(EmailFormModel model)
        {
            try
            {
                List<string> destinations = new List<string>();
                var formEntity = Uow.Forms.GetById(model.FormId);
                string attachment = HttpContext.Current.Server.MapPath(formEntity.FileUrl);

                var clientEntity = Uow.Clients.GetById(model.ClientId);
                destinations.Add(clientEntity.Email);
                if (string.IsNullOrEmpty(model.OtherEmail) == false)
                    destinations.Add(model.OtherEmail);

                _emailSvc.SendForm(destinations, model.Subject, model.Body, attachment);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE: api/DashboardApi/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var entity = Uow.Forms.GetAll()
                    .Where(f => f.Id == id)
                    .Include(f => f.InsuranceProvider)
                    .FirstOrDefault();

                if(entity == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                string message = entity.InsuranceProvider.Name + ":" + entity.FileName;
                var localFullPath = HttpContext.Current.Server.MapPath(entity.FileUrl);

                Uow.Forms.Delete(id);
                Uow.SaveChanges();

                if (File.Exists(localFullPath))
                    File.Delete(localFullPath);

                LogDelete(message);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private IMS.WebMvc.Services.EmailService __emailSvc;
        public IMS.WebMvc.Services.EmailService _emailSvc
        {
            get
            {
                if (__emailSvc == null)
                    __emailSvc = new IMS.WebMvc.Services.EmailService();
                return __emailSvc;
            }
        }

        #region Logs
        private void LogAdd(Form entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.AddForm);
            creator.AddForm(entity.Id);
            creator.SaveToLog();
        }

        private void LogEdit(Form entity)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.ModifyForm);
            creator.AddForm(entity.Id);
            creator.SaveToLog(false);
        }

        private void LogDelete(string message)
        {
            var creator = new LogCreatorApi(this, ActivityLogTypeEnum.DeleteForm);
            creator.AddMessage(message);
            creator.SaveToLog(false);
        }
        #endregion
    }
}