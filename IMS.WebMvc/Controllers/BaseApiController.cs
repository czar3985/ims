using IMS.Data;
using IMS.WebMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace IMS.WebMvc.Controllers
{
    public class BaseApiController : ApiController
    {
        private ApplicationUnit _uow;
        public ApplicationUnit Uow
        {
            get
            {
                if (_uow == null)
                    _uow = new ApplicationUnit();
                return _uow;
            }
        }

        private AttributeProviderService _attrProviderSvc;
        public AttributeProviderService AttributeProviderSvc
        {
            get
            {
                if (_attrProviderSvc == null)
                    _attrProviderSvc = new AttributeProviderService(this);
                return _attrProviderSvc;
            }
        }
    }
}