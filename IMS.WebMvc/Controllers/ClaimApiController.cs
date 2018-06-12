using IMS.Entities;
using IMS.WebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IMS.WebMvc.Controllers
{
    public class ClaimApiController : BaseApiController
    {
        // GET: api/ClaimApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ClaimApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ClaimApi
        public HttpResponseMessage Post([FromBody]ClaimModel model)
        {
            try
            {
                model.StatusId = AttributeProviderSvc.GetClaimStatusIdFromName("new");

                var entity = AutoMapper.Mapper.Map<Claim>(model);
                Uow.Claims.Add(entity);
                Uow.SaveChanges();

                if (string.IsNullOrEmpty(model.ReturnUrl))
                {
                    //return RedirectToAction("Index");
                    return Request.CreateResponse(HttpStatusCode.OK, new { returnUrl = "Index"});
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { returnUrl = model.ReturnUrl });
                    //return Redirect(model.ReturnUrl);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
                //return View();
            }
        }

        // PUT: api/ClaimApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ClaimApi/5
        public void Delete(int id)
        {
        }
    }
}
