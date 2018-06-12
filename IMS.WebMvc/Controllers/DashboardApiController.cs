using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using AutoMapper.QueryableExtensions;
using IMS.WebMvc.Models;

namespace IMS.WebMvc.Controllers
{
    public class DashboardApiController : BaseApiController
    {
        // GET: api/DashboardApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DashboardApi/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = Uow.Policies.GetAll()
                    .Where(p => p.Id == id)
                    .ProjectTo<ExpiringPoliciesModel>()
                    .FirstOrDefault();
                if (model.IsOrganization)
                    model.ClientName = model.OrganizationName;

                _emailSvc.SendRenewalNotice(model);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST: api/DashboardApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/DashboardApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DashboardApi/5
        public void Delete(int id)
        {
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
    }
}
