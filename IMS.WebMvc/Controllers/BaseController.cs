using IMS.Data;
using IMS.WebMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMS.WebMvc.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Dashboard");
        }

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

        private LoggingService _loggingSvc;
        public LoggingService LoggingSvc
        {
            get
            {
                if (_loggingSvc == null)
                    _loggingSvc = new LoggingService(this);
                return _loggingSvc;
            }
        }

        private ListProviderService _listProviderSvc;
        public ListProviderService ListProviderSvc
        {
            get
            {
                if (_listProviderSvc == null)
                    _listProviderSvc = new ListProviderService(this);
                return _listProviderSvc;
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